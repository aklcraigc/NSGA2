using System;
using System.Collections.Generic;
using Nsga2.Population;

namespace Nsga2.Selection;

public interface ISelection
{
    IReadOnlyList<Individual> Select(Random random, IReadOnlyList<Individual> individuals, int number);
}