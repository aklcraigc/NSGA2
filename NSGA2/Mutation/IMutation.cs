using Nsga2.Population;

namespace Nsga2.Mutation;

public interface IMutation
{
    Individual Mutate(IPopulationPolicy policy, Individual individual);
}