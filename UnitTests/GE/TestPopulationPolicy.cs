using MathNet.Numerics.Random;
using Nsga2.CrossOver;
using Nsga2.Grammar;
using Nsga2.Population;
using Nsga2.Selection;
using Random = System.Random;
using GEMut = Nsga2.Mutation;

namespace UnitTests.GE;

public class TestPopulationPolicy : IPopulationPolicy
{
    public int MaxCodonValue { get; set; } = 255;
    public int MaxGenerations { get; set; }
    public int PopulationSize { get; set; }
    public IPopulationEvents? Events { get; set; }
    public Random Random { get; set; } = new MersenneTwister(RandomSeed.Robust());
    public IGrammar Grammar { get; set; } = new NullGrammar();
    public ISelection Selection { get; set; } = new Tournament();
    public ICrossOver CrossOver { get; set; } = new OnePoint();
    public GEMut.IMutation Mutation { get; set; } = new GEMut.Random();
    public double MutationRate { get; set; } = 0.1;
    public int GenotypeLength { get; set; } = 100;
}