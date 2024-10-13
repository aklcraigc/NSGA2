using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using MoreLinq;
using Nsga2.Grammar;
using Nsga2.Objectives;

namespace Nsga2.Population;

public class Individual
{
    public Individual(IPopulationPolicy policy)
    {
        for (var i = 0; i < policy.GenotypeLength; ++i) Chromosome.Add(policy.Random.Next(policy.Grammar.MaxRuleDepth));

        GenerateExpression(policy.Grammar);
    }

    public Individual(IPopulationPolicy policy, List<int> values)
    {
        Chromosome = values;
        GenerateExpression(policy.Grammar);
    }

    public bool Valid { get; private set; } = true;
    public List<Objective> Objectives { get; set; } = [];
    public List<Objective> Constraints { private get; init; } = [];
    public double CrowdingDistance { get; private set; } = double.NaN;
    public string Tag { get; set; } = string.Empty;

    //Nsga2 sort
    private int DominationCount { get; set; }
    public int Rank { get; private set; }
    private List<Individual> DominatedIndividuals { get; } = [];

    //GE stuff
    public string? Expression { get; private set; }
    public List<int> Chromosome { get; } = [];
    public int CurrentIndex { get; private set; }
    public int Current => Chromosome[CurrentIndex];

    private bool HasConstraintViolations()
    {
        return Constraints.Any(constraint => constraint.Value < constraint.Min || constraint.Value > constraint.Max);
    }

    public static void SetCrowdingDistance(List<Individual> individuals)
    {
        //Reset crowding distance
        individuals.ForEach(i => i.CrowdingDistance = 0);
        //Get the objective set from the first individual (all the others should have the same structure).
        var objectives = individuals.First().Objectives;
        //For each objective
        for (var objectiveIndex = 0; objectiveIndex < objectives.Count; ++objectiveIndex)
        {
            //Sort the population by the current objective
            individuals.Sort((i1, i2) => i1.Objectives[objectiveIndex].Value.CompareTo(i2.Objectives[objectiveIndex].Value));
            //Set the ends to infinity
            individuals.First().CrowdingDistance = double.PositiveInfinity;
            individuals.Last().CrowdingDistance = double.PositiveInfinity;
            //Set the others as normalized distance between the individuals on either side
            for (var popIndex = 1; popIndex < individuals.Count - 1; ++popIndex)
                individuals[popIndex].CrowdingDistance += (individuals[popIndex + 1].Objectives[objectiveIndex].Value - individuals[popIndex - 1].Objectives[objectiveIndex].Value)
                                                          / individuals[popIndex].Objectives[objectiveIndex].Range;
        }
    }

    public static void SetDominatedBy(IReadOnlyList<Individual> individuals)
    {
        //Reset dominated-by
        individuals.ForEach(i =>
        {
            i.DominationCount = 0;
            i.Rank = 0;
            i.DominatedIndividuals.Clear();
        });
        //Brute force, compare every individual to every other individual
        for (var p = 0; p < individuals.Count; ++p)
        for (var q = 0; q < individuals.Count; ++q)
        {
            if (p == q) continue;

            if (CompareObjectives(individuals[p], individuals[q]) == Domination.First)
                individuals[p].DominatedIndividuals.Add(individuals[q]);
            else if (CompareObjectives(individuals[q], individuals[p]) == Domination.First) individuals[p].DominationCount++;
        }

        var currentFront = new List<Individual>();
        var rank = 1;
        individuals.Where(i => i.DominationCount == 0).ForEach(i =>
        {
            i.Rank = 1;
            currentFront.Add(i);
        });
        while (currentFront.Any())
        {
            var nextFront = new List<Individual>();
            foreach (var p in currentFront)
            foreach (var q in p.DominatedIndividuals)
                if (--q.DominationCount == 0)
                {
                    q.Rank = rank + 1;
                    nextFront.Add(q);
                }

            ++rank;
            currentFront = nextFront;
        }
    }

    public static Domination CompareObjectives(Individual lhs, Individual rhs)
    {
        if (lhs.Objectives.Count != rhs.Objectives.Count) throw new ArgumentException($"{MethodBase.GetCurrentMethod()?.Name}, Objectives have different lengths");

        var c1 = lhs.HasConstraintViolations();
        var c2 = rhs.HasConstraintViolations();
        if (c1 && c2) return IsDominated(lhs.Constraints, rhs.Constraints);

        if (c1) return Domination.Second;

        return c2 ? Domination.First : IsDominated(lhs.Objectives, rhs.Objectives);
    }

    private static Domination IsDominated(IReadOnlyList<Objective> lhs, IReadOnlyList<Objective> rhs)
    {
        var flag1 = false;
        var flag2 = false;
        Debug.Assert(lhs.Count == rhs.Count);
        for (var i = 0; i < lhs.Count; ++i)
        {
            if (lhs[i].Direction != rhs[i].Direction) throw new ArgumentException($"{MethodBase.GetCurrentMethod()?.Name}, Objectives have different directions");

            if (lhs[i].Name != rhs[i].Name) throw new ArgumentException($"{MethodBase.GetCurrentMethod()?.Name}, Objectives have different names");

            if (lhs[i].Direction == ObjectiveDirection.Minimize)
            {
                if (lhs[i].Value < rhs[i].Value) flag1 = true;

                if (lhs[i].Value > rhs[i].Value) flag2 = true;
            }
            else
            {
                if (lhs[i].Value > rhs[i].Value) flag1 = true;

                if (lhs[i].Value < rhs[i].Value) flag2 = true;
            }
        }

        if (flag1 && !flag2) return Domination.First;

        if (!flag1 && flag2) return Domination.Second;

        return Domination.Neither;
    }

    public bool Increment()
    {
        if (++CurrentIndex >= Chromosome.Count) Valid = false;

        return Valid;
    }

    public void Reset()
    {
        CurrentIndex = 0;
        Valid = true;
    }

    private void GenerateExpression(IGrammar grammar)
    {
        Expression = grammar.Generate(this);
    }

    public override string ToString()
    {
        return
            $"O=[{string.Join("|", Objectives)}]{(Constraints.Any() ? $" C=[{string.Join("|", Constraints)}]" : "")} DB=[{DominationCount}] CD=[{Math.Round(CrowdingDistance, 4)}] Used=[{CurrentIndex}] Expression=[{Expression}]";
    }
}