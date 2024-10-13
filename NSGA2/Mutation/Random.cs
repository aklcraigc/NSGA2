using System.Diagnostics;
using System.Linq;
using Nsga2.Population;

namespace Nsga2.Mutation;

public class Random : IMutation
{
    public Individual Mutate(IPopulationPolicy policy, Individual individual)
    {
        var codons = individual.Chromosome.ToList();
        Debug.Assert(codons.Count > 0);
        Debug.Assert(individual.CurrentIndex > 0);
        var currentExpression = individual.Expression;
        while (true)
        {
            codons[policy.Random.Next(individual.CurrentIndex)] = policy.Random.Next(policy.Grammar.MaxRuleDepth);
            var rv = new Individual(policy, codons);
            if (rv.Expression != currentExpression) return rv;
        }
    }
}