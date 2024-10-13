using System;
using System.Collections.Generic;
using MoreLinq;
using Nsga2.Objectives;
using Nsga2.Population;
using SimMetrics.Net.Metric;

namespace UnitTests.GE;

public sealed class GeTestFitness1
{
    public const string Definition = @"@<sentence> ::= <word>{ <word>}
                                           @<word>     ::= <letter>{<letter>}
                                           @<letter>   ::= a|b|c|d|e|f|g|h|i|j|k|l|m|n|o|p|q|r|s|t|u|v|w|x|y|z";

    private static string Target => "the quick brown fox jumped over the lazy dog";

    public static void FitnessFunction(IEnumerable<Individual> individuals)
    {
        var s = new Levenstein();
        individuals.ForEach(i => i.Objectives =
        [
            new Objective(ObjectiveDirection.Minimize, "F", 0, 100, s.GetSimilarity(i.Expression, Target)),
            new Objective(ObjectiveDirection.Minimize, "L", 0, 100, Math.Abs(i.Expression?.Length ?? 0 - Target.Length))
        ]);
    }
}