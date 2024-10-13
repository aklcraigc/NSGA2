using Nsga2.Population;

namespace Nsga2.Grammar;

public interface IGrammar
{
    int MaxRuleDepth { get; }
    string Generate(Individual individual);
}