using Nsga2.Population;

namespace Nsga2.CrossOver;

public interface ICrossOver
{
    Individual CrossOver(IPopulationPolicy policy, Individual individual1, Individual individual2);
}