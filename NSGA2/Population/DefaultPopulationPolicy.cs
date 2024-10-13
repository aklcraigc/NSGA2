using MathNet.Numerics.Random;
using Nsga2.CrossOver;
using Nsga2.Grammar;
using Nsga2.Mutation;
using Nsga2.Selection;
using Random = System.Random;

namespace Nsga2.Population;

public class DefaultPopulationPolicy(string definition, int maxGenerations, int populationSize) : IPopulationPolicy
{
    public int MaxGenerations { get; } = maxGenerations;
    public int PopulationSize { get; } = populationSize;
    public IPopulationEvents Events { get; set; } = new NullPopulationEvents();
    public Random Random { get; } = new MersenneTwister(RandomSeed.Robust());
    public IGrammar Grammar { get; } = new BnfGrammar(definition);
    public ISelection Selection { get; } = new Tournament();
    public ICrossOver CrossOver { get; } = new OnePoint();
    public IMutation Mutation { get; } = new Mutation.Random();
    public double MutationRate => 0.8;
    public int GenotypeLength => 100;
}