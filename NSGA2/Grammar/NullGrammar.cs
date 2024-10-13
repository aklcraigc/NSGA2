using Nsga2.Population;

namespace Nsga2.Grammar;

public class NullGrammar : IGrammar
{
    public string Generate(Individual individual) => "";

    public int MaxRuleDepth => 10;
}