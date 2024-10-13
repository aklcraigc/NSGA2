namespace Nsga2.Population;

public class NullPopulationEvents : IPopulationEvents
{
    public void NewGeneration(int generation, Population population, double minutesRemaining, double minutesElapsed)
    {
    }

    public bool HasConverged(Population population)
    {
        return false;
    }

    public void Convergence(int generation)
    {
    }

    public void MaxGenerationsReached(int generation, Population population)
    {
    }
}