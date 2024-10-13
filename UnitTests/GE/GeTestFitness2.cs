using System;
using System.Collections.Generic;
using MoreLinq;
using Nsga2.Objectives;
using Nsga2.Population;
using SimMetrics.Net.Metric;

namespace UnitTests.GE;

public sealed class GeTestFitness2
{
    public const string Definition = @"@<expression>   ::=  <number> | (<expression> <operator> <expression>)
                                           @<operator>     ::=  +|-|*|/
                                           @<number>       ::=  <digit>{<digit>}
                                           @<digit>        ::=  0|1|2|3|4|5|6|7|8|9";

    private static string Target => "((27 + 3) * (1 / 2) - 5)";

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