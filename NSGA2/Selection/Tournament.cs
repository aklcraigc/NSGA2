using System;
using System.Collections.Generic;
using Nsga2.Population;

namespace Nsga2.Selection;

public sealed class Tournament : ISelection
{
    public IReadOnlyList<Individual> Select(Random random, IReadOnlyList<Individual> individuals, int number)
    {
        var rv = new List<Individual>();
        while (rv.Count < number)
        {
            var i1 = individuals[random.Next(individuals.Count)];
            var i2 = individuals[random.Next(individuals.Count)];
            rv.Add(BinaryTournament(random, i1, i2));
        }

        return rv;
    }

    private static Individual BinaryTournament(Random random, Individual individual1, Individual individual2)
    {
        if (Individual.CompareObjectives(individual1, individual2) == Domination.First) return individual1;

        if (Individual.CompareObjectives(individual2, individual1) == Domination.First) return individual2;

        if (individual1.CrowdingDistance > individual2.CrowdingDistance) return individual1;

        if (individual1.CrowdingDistance < individual2.CrowdingDistance) return individual2;

        return random.NextDouble() > 0.5 ? individual1 : individual2;
    }
}