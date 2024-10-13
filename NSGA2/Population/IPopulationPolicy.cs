using Nsga2.CrossOver;
using Nsga2.Grammar;
using Nsga2.Mutation;
using Nsga2.Selection;
using Random = System.Random;

namespace Nsga2.Population;

public interface IPopulationPolicy
{
    int MaxGenerations { get; }
    int PopulationSize { get; }
    IPopulationEvents? Events { get; }
    Random Random { get; }
    IGrammar Grammar { get; }
    ISelection Selection { get; }
    ICrossOver CrossOver { get; }
    IMutation Mutation { get; }
    double MutationRate { get; }
    int GenotypeLength { get; }
}