using System;
using System.Diagnostics;
using System.Linq;
using Nsga2.Population;

namespace Nsga2.CrossOver;

public class OnePoint : ICrossOver
{
    public Individual CrossOver(IPopulationPolicy policy, Individual individual1, Individual individual2)
    {
        Debug.Assert(individual1.Valid);
        Debug.Assert(individual2.Valid);
        Debug.Assert(individual2.Chromosome.Count > 0);
        Debug.Assert(individual2.Chromosome.Count > 0);
        //Picking a random crossover point in the codon may well miss all the generating codons depending on how
        //many get 'used' to generate an expression. So we need to pick our point where it's going to cause
        //a difference in the generation. Current index is the next of the next codon to be used.
        //So if CurrentIndex is 5 (for instance), this means we have used indexes [0..4] or the first 5 elements,
        //we have 2 constraints for the number we take before the crossover point.
        //1) more than 0 to avoid children being a swap of parents
        //2) less than 5 for the same reason in 1) again
        //This still won't prevent duplication outright due to differing grammar structures etc.
        var maxToTake = Math.Max(1, Math.Min(individual1.CurrentIndex, individual2.CurrentIndex) - 1);
        var crossOverPoint = maxToTake > 1 ? policy.Random.Next(1, maxToTake) : 1;
        var rv = CrossOverImpl(policy, crossOverPoint, individual1, individual2);
        return rv;
    }

    public Individual CrossOver(IPopulationPolicy policy, int crossOverPoint, Individual individual1, Individual individual2)
    {
        return CrossOverImpl(policy, crossOverPoint, individual1, individual2);
    }

    private static Individual CrossOverImpl(IPopulationPolicy policy, int crossOverPoint, Individual individual1, Individual individual2)
    {
        var rv = new Individual(policy, individual1.Chromosome.Take(crossOverPoint).Concat(individual2.Chromosome.Skip(crossOverPoint)).ToList());
        Debug.Assert(rv.Chromosome.Count > 0);
        return rv;
    }
}