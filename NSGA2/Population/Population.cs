using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MathNet.Numerics.Statistics;

namespace Nsga2.Population;

//Implements the Nsga2 algorithm for multi-objective optimization.
public sealed class Population
{
    private readonly Action<IEnumerable<Individual>> _fitnessFunction;
    private readonly IPopulationPolicy _populationPolicy;

    //The default method of removing duplicates is via string comparision.
    private readonly Func<List<Individual>, List<Individual>> _removeDuplicates = individuals => individuals.GroupBy(i => i.Expression).Select(i => i.First()).ToList();

    private List<Individual> _currentPopulation = [];

    public Population(IPopulationPolicy policy, Action<IEnumerable<Individual>> fitnessFunction, Func<List<Individual>, List<Individual>>? removeDuplicates)
    {
        _fitnessFunction = fitnessFunction;
        _populationPolicy = policy;
        if (removeDuplicates != null) _removeDuplicates = removeDuplicates;

        //Create initial population
        _currentPopulation.AddRange(CreateNewPopulation());
        Debug.Assert(_currentPopulation.Count > 0);
        //Eval fitness
        SetPopulationObjectives(_currentPopulation);
        //Rank by domination
        Individual.SetDominatedBy(_currentPopulation);
    }

    private IEnumerable<Individual> CreateNewPopulation()
    {
        var rv = new List<Individual>();
        var removeDuplicateRetries = 0;
        while (rv.Count < _populationPolicy.PopulationSize)
        {
            while (rv.Count < _populationPolicy.PopulationSize)
            {
                var individual = new Individual(_populationPolicy);
                if (!individual.Valid) continue;

                rv.Add(individual);
            }

            //Make sure we start with no duplicates etc...
            rv = _removeDuplicates(rv);
            if (++removeDuplicateRetries > 20) break;
        }

        return rv;
    }

    private List<Individual> CreateDescendantPopulation()
    {
        var rv = new List<Individual>();
        var removeDuplicateRetries = 0;
        while (rv.Count < _populationPolicy.PopulationSize)
        {
            while (rv.Count < _populationPolicy.PopulationSize)
            {
                var parents = _populationPolicy.Selection.Select(_populationPolicy.Random, _currentPopulation, 2);
                var individual = _populationPolicy.CrossOver.CrossOver(_populationPolicy, parents[0], parents[1]);
                if (_populationPolicy.Random.NextDouble() < _populationPolicy.MutationRate) individual = _populationPolicy.Mutation.Mutate(_populationPolicy, individual);

                if (!individual.Valid) continue;

                rv.Add(individual);
            }

            //Make sure we start with no duplicates etc...
            rv = _removeDuplicates(rv);
            if (++removeDuplicateRetries > 20) break;
        }

        return rv;
    }

    private void CreateNewGeneration()
    {
        var newOffspring = CreateDescendantPopulation();
        SetPopulationObjectives(newOffspring);
        _currentPopulation.AddRange(newOffspring);
        _currentPopulation = _removeDuplicates(_currentPopulation);
        Individual.SetDominatedBy(_currentPopulation);
        Reduce();
        Debug.Assert(_currentPopulation.Count > 0);
    }

    public void Run()
    {
        var times = new List<double>();
        for (var i = 0; i < _populationPolicy.MaxGenerations; ++i)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            CreateNewGeneration();
            var minutesElapsed = stopWatch.Elapsed.TotalMinutes;
            times.Add(minutesElapsed);
            var minutesRemaining = (_populationPolicy.MaxGenerations - i) * times.Percentile(50);
            _populationPolicy.Events?.NewGeneration(i, this, minutesRemaining, minutesElapsed);
            if (_populationPolicy.Events?.HasConverged(this) ?? false)
            {
                _populationPolicy.Events.Convergence(i);
                return;
            }
        }

        _populationPolicy.Events?.MaxGenerationsReached(_populationPolicy.MaxGenerations, this);
    }

    private void Reduce()
    {
        var maxRank = _currentPopulation.Max(i => i.Rank);
        var newPopulation = new List<Individual>();
        for (var rank = 1; rank <= maxRank; ++rank)
        {
            var front = GetFront(rank).ToList();
            if (!front.Any()) continue;

            Individual.SetCrowdingDistance(front);
            newPopulation.AddRange(_populationPolicy.PopulationSize - newPopulation.Count > front.Count
                ? front
                : front.OrderByDescending(i => i.CrowdingDistance).Take(_populationPolicy.PopulationSize - newPopulation.Count));
            if (newPopulation.Count == _populationPolicy.PopulationSize) break;
        }

        _currentPopulation = newPopulation;
        Debug.Assert(_currentPopulation.Count > 0);
    }

    private void SetPopulationObjectives(IEnumerable<Individual> population)
    {
        _fitnessFunction(population);
    }

    private IEnumerable<Individual> GetFront(int front)
    {
        return _currentPopulation.Where(i => i.Rank == front);
    }

    public IReadOnlyCollection<Individual> GetPopulation()
    {
        return _currentPopulation;
    }

    public static string FitnessStats(IReadOnlyCollection<Individual> individuals)
    {
        return string.Join("", individuals.First().Objectives.Select((o, i) => $"[{Math.Round(individuals.Select(c => c.Objectives[i].Value).Percentile(50), 2)}]"));
    }

    public static double MedianFitness(IEnumerable<Individual> individuals, int objective)
    {
        return individuals.Select(c => c.Objectives[objective].Value).Percentile(50);
    }
}