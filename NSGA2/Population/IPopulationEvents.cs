namespace Nsga2.Population;

public interface IPopulationEvents
{
    void NewGeneration(int generation, Population population, double minutesRemaining, double minutesElapsed);
    bool HasConverged(Population population);
    void Convergence(int generation);
    void MaxGenerationsReached(int generation, Population population);
}