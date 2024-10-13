using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nsga2.CrossOver;
using Nsga2.Grammar;
using Nsga2.Objectives;
using Nsga2.Population;

// ReSharper disable StringLiteralTypo

namespace UnitTests.GE;

[TestClass]
public class GeTests
{
    [TestMethod]
    public void ObjectiveGroupTest1()
    {
        var i1 = new Individual(new TestPopulationPolicy())
        {
            Objectives = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, 1)]
        };
        var i2 = new Individual(new TestPopulationPolicy())
        {
            Objectives = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, 0.5)]
        };
        Assert.AreEqual(Individual.CompareObjectives(i1, i2), Domination.First);
        Assert.AreEqual(Individual.CompareObjectives(i2, i1), Domination.Second);
    }

    [TestMethod]
    public void ObjectiveGroupTest1C1()
    {
        var i1 = new Individual(new TestPopulationPolicy())
        {
            Objectives = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, 1)],
            Constraints = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, -1)]
        };
        var i2 = new Individual(new TestPopulationPolicy())
        {
            Objectives = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, 0.5)]
        };
        Assert.AreEqual(Individual.CompareObjectives(i1, i2), Domination.Second);
        Assert.AreEqual(Individual.CompareObjectives(i2, i1), Domination.First);
    }

    [TestMethod]
    public void ObjectiveGroupTest1C2()
    {
        var i1 = new Individual(new TestPopulationPolicy())
        {
            Objectives = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, 1)]
        };
        var i2 = new Individual(new TestPopulationPolicy())
        {
            Objectives = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, 0.5)],
            Constraints = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, -1.5)]
        };
        Assert.AreEqual(Individual.CompareObjectives(i1, i2), Domination.First);
        Assert.AreEqual(Individual.CompareObjectives(i2, i1), Domination.Second);
    }

    [TestMethod]
    public void ObjectiveGroupTest1C3()
    {
        var i1 = new Individual(new TestPopulationPolicy())
        {
            Objectives = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, 1)],
            Constraints = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, -1)]
        };
        var i2 = new Individual(new TestPopulationPolicy())
        {
            Objectives = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, 0.5)],
            Constraints = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, -1.5)]
        };
        Assert.AreEqual(Individual.CompareObjectives(i1, i2), Domination.First);
        Assert.AreEqual(Individual.CompareObjectives(i2, i1), Domination.Second);
    }

    [TestMethod]
    public void ObjectiveGroupTest1C4()
    {
        var i1 = new Individual(new TestPopulationPolicy())
        {
            Objectives = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, 0.5)],
            Constraints = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, -1.5)]
        };
        var i2 = new Individual(new TestPopulationPolicy())
        {
            Objectives = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, 1)],
            Constraints = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, -1)]
        };
        Assert.AreEqual(Individual.CompareObjectives(i1, i2), Domination.Second);
        Assert.AreEqual(Individual.CompareObjectives(i2, i1), Domination.First);
    }

    [TestMethod]
    public void ObjectiveGroupTest1C5()
    {
        var i1 = new Individual(new TestPopulationPolicy())
        {
            Objectives = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, 1)],
            Constraints = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, -1)]
        };
        var i2 = new Individual(new TestPopulationPolicy())
        {
            Objectives = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, 1)],
            Constraints = [new Objective(ObjectiveDirection.Maximize, "A", 0, 5, -1)]
        };
        Assert.AreEqual(Individual.CompareObjectives(i1, i2), Domination.Neither);
        Assert.AreEqual(Individual.CompareObjectives(i2, i1), Domination.Neither);
    }

    [TestMethod]
    public void ObjectiveGroupTest2()
    {
        var i1 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Maximize, "A", 0, 5, 1),
                new Objective(ObjectiveDirection.Maximize, "B", 0, 5, 1)
            ]
        };

        var i2 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Maximize, "A", 0, 5, 2),
                new Objective(ObjectiveDirection.Maximize, "B", 0, 5, 1)
            ]
        };

        var i3 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Maximize, "A", 0, 5, 1),
                new Objective(ObjectiveDirection.Maximize, "B", 0, 5, 2)
            ]
        };

        Assert.AreEqual(Individual.CompareObjectives(i2, i3), Domination.Neither);
        Assert.AreEqual(Individual.CompareObjectives(i3, i2), Domination.Neither);

        Assert.AreEqual(Individual.CompareObjectives(i2, i1), Domination.First);
        Assert.AreEqual(Individual.CompareObjectives(i3, i1), Domination.First);

        Assert.AreEqual(Individual.CompareObjectives(i1, i2), Domination.Second);
        Assert.AreEqual(Individual.CompareObjectives(i1, i3), Domination.Second);
    }

    [TestMethod]
    public void ObjectiveGroupTest3()
    {
        var i1 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "A", 0, 5, 1),
                new Objective(ObjectiveDirection.Minimize, "B", 0, 5, 1)
            ]
        };

        var i2 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "A", 0, 5, 2),
                new Objective(ObjectiveDirection.Minimize, "B", 0, 5, 1)
            ]
        };

        var i3 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "A", 0, 5, 1),
                new Objective(ObjectiveDirection.Minimize, "B", 0, 5, 2)
            ]
        };

        Assert.AreEqual(Individual.CompareObjectives(i1, i3), Domination.First);
        Assert.AreEqual(Individual.CompareObjectives(i1, i2), Domination.First);

        Assert.AreEqual(Individual.CompareObjectives(i2, i3), Domination.Neither);
        Assert.AreEqual(Individual.CompareObjectives(i3, i2), Domination.Neither);

        Assert.AreEqual(Individual.CompareObjectives(i2, i1), Domination.Second);
        Assert.AreEqual(Individual.CompareObjectives(i3, i1), Domination.Second);
    }

    [TestMethod]
    public void ObjectiveGroupTest4()
    {
        var i1 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "A", 0, 5, 1),
                new Objective(ObjectiveDirection.Maximize, "B", 0, 5, 1)
            ]
        };

        var i2 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "A", 0, 5, 2),
                new Objective(ObjectiveDirection.Maximize, "B", 0, 5, 1)
            ]
        };

        var i3 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "A", 0, 5, 1),
                new Objective(ObjectiveDirection.Maximize, "B", 0, 5, 2)
            ]
        };

        Assert.AreEqual(Individual.CompareObjectives(i1, i3), Domination.Second);
        Assert.AreEqual(Individual.CompareObjectives(i1, i2), Domination.First);

        Assert.AreEqual(Individual.CompareObjectives(i2, i3), Domination.Second);
        Assert.AreEqual(Individual.CompareObjectives(i3, i2), Domination.First);

        Assert.AreEqual(Individual.CompareObjectives(i2, i1), Domination.Second);
        Assert.AreEqual(Individual.CompareObjectives(i3, i1), Domination.First);
    }

    [TestMethod]
    public void ObjectiveGroupTest5()
    {
        var i1 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Maximize, "A", 0, 5, 1.45),
                new Objective(ObjectiveDirection.Minimize, "B", 0, 100, 33),
                new Objective(ObjectiveDirection.Maximize, "C", 0, 1500, 429)
            ]
        };

        var i2 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Maximize, "A", 0, 5, 0.02),
                new Objective(ObjectiveDirection.Minimize, "B", 0, 100, 34),
                new Objective(ObjectiveDirection.Maximize, "C", 0, 1500, 790)
            ]
        };

        var i3 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Maximize, "A", 0, 5, 0.77),
                new Objective(ObjectiveDirection.Minimize, "B", 0, 100, 35),
                new Objective(ObjectiveDirection.Maximize, "C", 0, 1500, 1032)
            ]
        };

        var i4 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Maximize, "A", 0, 5, 1.74),
                new Objective(ObjectiveDirection.Minimize, "B", 0, 100, 35),
                new Objective(ObjectiveDirection.Maximize, "C", 0, 1000, 162)
            ]
        };

        Assert.AreEqual(Individual.CompareObjectives(i4, i1), Domination.Neither);
        Assert.AreEqual(Individual.CompareObjectives(i4, i2), Domination.Neither);
        Assert.AreEqual(Individual.CompareObjectives(i4, i3), Domination.Neither);
    }

    [TestMethod]
    public void ObjectiveGroupTest6()
    {
        var i1 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Maximize, "A", 0, 10, 1.47),
                new Objective(ObjectiveDirection.Maximize, "B", 0, 1000, 277),
                new Objective(ObjectiveDirection.Minimize, "C", 0, 100, 56.8)
            ]
        };

        var i2 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Maximize, "A", 0, 10, 1.47),
                new Objective(ObjectiveDirection.Maximize, "B", 0, 1000, 277),
                new Objective(ObjectiveDirection.Minimize, "C", 0, 100, 56.8)
            ]
        };

        Assert.AreEqual(Individual.CompareObjectives(i1, i2), Domination.Neither);
        Assert.AreEqual(Individual.CompareObjectives(i2, i1), Domination.Neither);
    }

    [TestMethod]
    public void NonDominatedFrontTest1()
    {
        var i1 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 1),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 1)
            ]
        };

        var i2 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 2),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 2)
            ]
        };

        var individuals = new List<Individual> { i1, i2 };
        Individual.SetDominatedBy(individuals);
        Assert.AreEqual(i1.Rank, 1);
        Assert.AreEqual(i2.Rank, 2);
    }

    [TestMethod]
    public void NonDominatedFrontTest2()
    {
        var i1 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 1),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 1)
            ]
        };

        var i2 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 2),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 2)
            ]
        };

        var i3 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 3),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 3)
            ]
        };

        var individuals = new List<Individual> { i1, i2, i3 };
        Individual.SetDominatedBy(individuals);
        Assert.AreEqual(i1.Rank, 1);
        Assert.AreEqual(i2.Rank, 2);
        Assert.AreEqual(i3.Rank, 3);
    }

    [TestMethod]
    public void NonDominatedFrontTest3()
    {
        var i1 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 1),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 5)
            ]
        };

        var i2 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 1),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 3)
            ]
        };

        var i3 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 1),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 1)
            ]
        };

        var i4 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 3),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 1)
            ]
        };

        var i5 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 5),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 1)
            ]
        };

        var i6 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 2),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 4)
            ]
        };

        var i7 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 4),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 2)
            ]
        };

        var i8 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 4),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 4)
            ]
        };

        var individuals = new List<Individual> { i1, i2, i3, i4, i5, i6, i7, i8 };
        Individual.SetDominatedBy(individuals);
        Assert.AreEqual(i1.Rank, 3);
        Assert.AreEqual(i2.Rank, 2);
        Assert.AreEqual(i3.Rank, 1);
        Assert.AreEqual(i4.Rank, 2);
        Assert.AreEqual(i5.Rank, 3);
        Assert.AreEqual(i6.Rank, 3);
        Assert.AreEqual(i7.Rank, 3);
        Assert.AreEqual(i8.Rank, 4);
    }

    [TestMethod]
    public void CrowdingDistanceTest1()
    {
        var i1 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 1),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 5)
            ]
        };

        var i2 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 1),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 3)
            ]
        };

        var i3 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 1),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 1)
            ]
        };

        var i4 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 3),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 1)
            ]
        };

        var i5 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 5),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 1)
            ]
        };

        var i6 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 2),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 4)
            ]
        };

        var i7 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 4),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 2)
            ]
        };

        var i8 = new Individual(new TestPopulationPolicy())
        {
            Objectives =
            [
                new Objective(ObjectiveDirection.Minimize, "X", 0, 10, 4),
                new Objective(ObjectiveDirection.Minimize, "Y", 0, 10, 4)
            ]
        };

        var individuals = new List<Individual> { i1, i2, i3, i4, i5, i6, i7, i8 };
        Individual.SetCrowdingDistance(individuals);
        Assert.IsTrue(double.IsInfinity(i1.CrowdingDistance));
        Assert.AreEqual(i2.CrowdingDistance, 0.2);
        Assert.IsTrue(double.IsInfinity(i3.CrowdingDistance));
        Assert.AreEqual(i4.CrowdingDistance, 0.2);
        Assert.IsTrue(double.IsInfinity(i5.CrowdingDistance));
        Assert.AreEqual(Math.Round(i6.CrowdingDistance, 1), 0.3);
        Assert.AreEqual(Math.Round(i7.CrowdingDistance, 1), 0.3);
        Assert.AreEqual(i8.CrowdingDistance, 0.2);
    }

    [TestMethod]
    public void OptionalTest1()
    {
        const string grammar = @"@<x> ::= a [b]";
        var bnf = new BnfGrammar(grammar);
        Assert.AreEqual(bnf.Rules["<x>"][0], "a <O0>");
        Assert.AreEqual(bnf.Rules["<O0>"][0], BnfGrammar.Empty);
        Assert.AreEqual(bnf.Rules["<O0>"][1], "b");
    }

    [TestMethod]
    public void OptionalTest2()
    {
        const string grammar = @"@<x> ::= a [b] [c]";
        var bnf = new BnfGrammar(grammar);
        Assert.AreEqual(bnf.Rules["<x>"][0], "a <O0> <O1>");
        Assert.AreEqual(bnf.Rules["<O0>"][0], BnfGrammar.Empty);
        Assert.AreEqual(bnf.Rules["<O0>"][1], "b");
        Assert.AreEqual(bnf.Rules["<O1>"][0], BnfGrammar.Empty);
        Assert.AreEqual(bnf.Rules["<O1>"][1], "c");
    }

    [TestMethod]
    public void OptionalTest3()
    {
        const string grammar = @"@<x> ::= a [b ]";
        var bnf = new BnfGrammar(grammar);
        Assert.AreEqual(bnf.Rules["<x>"][0], "a <O0>");
        Assert.AreEqual(bnf.Rules["<O0>"][0], BnfGrammar.Empty);
        Assert.AreEqual(bnf.Rules["<O0>"][1], "b ");
    }

    [TestMethod]
    public void RepetitionTest1()
    {
        const string grammar = @"@<x> ::= a {b}";
        var bnf = new BnfGrammar(grammar);
        Assert.AreEqual(bnf.Rules["<x>"][0], "a <R0>");
        Assert.AreEqual(bnf.Rules["<R0>"][0], BnfGrammar.Empty);
        Assert.AreEqual(bnf.Rules["<R0>"][1], "<R0>b");
    }

    [TestMethod]
    public void RepetitionTest2()
    {
        const string grammar = @"@<x> ::= a {b} {c}";
        var bnf = new BnfGrammar(grammar);
        Assert.AreEqual(bnf.Rules["<x>"][0], "a <R0> <R1>");
        Assert.AreEqual(bnf.Rules["<R0>"][0], BnfGrammar.Empty);
        Assert.AreEqual(bnf.Rules["<R0>"][1], "<R0>b");
        Assert.AreEqual(bnf.Rules["<R1>"][0], BnfGrammar.Empty);
        Assert.AreEqual(bnf.Rules["<R1>"][1], "<R1>c");
    }

    [TestMethod]
    public void RepetitionTest3()
    {
        const string grammar = @"@<x> ::= a {<b>}";
        var bnf = new BnfGrammar(grammar);
        Assert.AreEqual(bnf.Rules["<x>"][0], "a <R0>");
        Assert.AreEqual(bnf.Rules["<R0>"][0], BnfGrammar.Empty);
        Assert.AreEqual(bnf.Rules["<R0>"][1], "<R0><b>");
    }

    [TestMethod]
    public void RepetitionTest4()
    {
        const string grammar = @"@<x> ::= a { b}";
        var bnf = new BnfGrammar(grammar);
        Assert.AreEqual(bnf.Rules["<x>"][0], "a <R0>");
        Assert.AreEqual(bnf.Rules["<R0>"][0], BnfGrammar.Empty);
        Assert.AreEqual(bnf.Rules["<R0>"][1], "<R0> b");
    }

    [TestMethod]
    public void RepetitionAndOptionalTest1()
    {
        const string grammar = @"@<x> ::= a [b] {c}";
        var bnf = new BnfGrammar(grammar);
        Assert.AreEqual(bnf.Rules["<x>"][0], "a <O0> <R0>");
        Assert.AreEqual(bnf.Rules["<O0>"][0], BnfGrammar.Empty);
        Assert.AreEqual(bnf.Rules["<O0>"][1], "b");
        Assert.AreEqual(bnf.Rules["<R0>"][0], BnfGrammar.Empty);
        Assert.AreEqual(bnf.Rules["<R0>"][1], "<R0>c");
    }

    [TestMethod]
    public void GrammarTest1()
    {
        const string grammar = @"@<tradingrule>   ::= if (<signal>) <trade> else <trade>
                                     @<signal>        ::= <value> <relop> <var> | (<signal>) AND (<signal>) | (<signal>) OR (<signal>)
                                     @<value>         ::= <int-const> | <real-const>
                                     @<relop>         ::= <= | >=
                                     @<trade>         ::= buy|sell|do-nothing
                                     @<int-const>     ::= <int-const><int-const>|1|2|3|4|5|6|7|8|9
                                     @<real-const>    ::= 0.<int-const>
                                     @<var>           ::= var0 | var1 | var2 | var3 | var4 | var5 | var6 | var7 | var8 | var9";
        var p = new DefaultPopulationPolicy(grammar, 0, 0);
        var i = new Individual(p, [42, 22, 6, 104, 70, 31, 13, 4, 25, 9, 3, 86, 44, 48, 3, 27, 4, 111, 56, 2]);
        //They fuck this up in the book, the (86) <real-const> => (44) 0.<int-const> => 0.4, not to 0.64.
        Assert.AreEqual(i.Expression, "if ((13 <= var5) AND (0.4 <= var3)) buy else sell");
    }

    [TestMethod]
    public void GrammarTest2()
    {
        const string grammar = @"@<S> ::= <C> | <C><C> | <C><B><C>
                                     @<B> ::= <D> | <D><E> | <E>
                                     @<C> ::= g | h
                                     @<D> ::= j | k
                                     @<E> ::= k | l | m";
        var p = new DefaultPopulationPolicy(grammar, 0, 0);
        var i = new Individual(p, [44, 246, 13, 49, 21, 3]);
        //They fuck this one up too http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.452.7023&rep=rep1&type=pdf (gjkh).
        Assert.AreEqual(i.Expression, "gkkh");
    }

    [TestMethod]
    public void GrammarTest3()
    {
        const string grammar = @"@<Seq>   ::= <vowel> | <Seq> <vowel>
                                     @<vowel> ::= A | E | I | O | U";
        var p = new DefaultPopulationPolicy(grammar, 0, 0);
        var i = new Individual(p, [43, 118, 144, 17]);
        //Finally somebody gets one right! http://bds.ul.ie/libGE/libGE/Example-of-Mapping-Process.html#Example-of-Mapping-Process
        Assert.AreEqual(i.Expression, "U I");
    }

    [TestMethod]
    public void GrammarTest4()
    {
        var grammar = @"@<expr>     ::= <expr> <op> <expr> | (<expr> <op> <expr>) | <pre_op>(<expr>) | x
                            @<op>       ::= + | - | * | /
                            @<pre_op>   ::= sin | exp";
        var p = new DefaultPopulationPolicy(grammar, 0, 0);
        var i = new Individual(p, [0, 2, 1, 3, 2, 3]);
        //These guys almost get it right. http://www.complex-systems.com/pdf/14-4-1.pdf
        Assert.AreEqual(i.Expression, "exp(x) * x");
    }

    [TestMethod]
    public void IndividualTest1()
    {
        var i = new Individual(new TestPopulationPolicy(), [43, 118, 144, 17]);
        Assert.IsTrue(i.Current == 43);
        Assert.IsTrue(i.Increment());
        Assert.IsTrue(i.Current == 118);
        Assert.IsTrue(i.Increment());
        Assert.IsTrue(i.Current == 144);
        Assert.IsTrue(i.Increment());
        Assert.IsTrue(i.Current == 17);
        Assert.IsFalse(i.Increment());
        i.Reset();
        Assert.IsTrue(i.Current == 43);
    }

    [TestMethod]
    public void OnePointCrossOverTest1()
    {
        var i1 = new Individual(new TestPopulationPolicy(), [6, 7, 8, 9, 10]);
        var i2 = new Individual(new TestPopulationPolicy(), [1, 2, 3, 4, 5]);
        var c = new OnePoint();
        var offSpring = c.CrossOver(new TestPopulationPolicy(), 2, i1, i2);
        Assert.IsTrue(offSpring.Chromosome.SequenceEqual(new List<int> { 6, 7, 3, 4, 5 }));
    }

    [TestMethod]
    public void OnePointCrossOverTest2()
    {
        var i1 = new Individual(new TestPopulationPolicy(), [6, 7, 8, 9, 10]);
        var i2 = new Individual(new TestPopulationPolicy(), [1, 2, 3, 4, 5]);
        var c = new OnePoint();
        var offSpring = c.CrossOver(new TestPopulationPolicy(), 1, i1, i2);
        Assert.IsTrue(offSpring.Chromosome.SequenceEqual(new List<int> { 6, 2, 3, 4, 5 }));
    }

    [TestMethod]
    public void OnePointCrossOverTest3()
    {
        var i1 = new Individual(new TestPopulationPolicy(), [6, 7, 8, 9, 10]);
        var i2 = new Individual(new TestPopulationPolicy(), [1, 2, 3, 4, 5]);
        var c = new OnePoint();
        var offSpring = c.CrossOver(new TestPopulationPolicy(), 3, i1, i2);
        Assert.IsTrue(offSpring.Chromosome.SequenceEqual(new List<int> { 6, 7, 8, 4, 5 }));
    }

    //Note: these next two just swap the contents...
    [TestMethod]
    public void OnePointCrossOverTest5()
    {
        var i1 = new Individual(new TestPopulationPolicy(), [6, 7, 8, 9, 10]);
        var i2 = new Individual(new TestPopulationPolicy(), [1, 2, 3, 4, 5]);
        var c = new OnePoint();
        var offSpring = c.CrossOver(new TestPopulationPolicy(), 0, i1, i2);
        Assert.IsTrue(offSpring.Chromosome.SequenceEqual(new List<int> { 1, 2, 3, 4, 5 }));
    }

    [TestMethod]
    public void OnePointCrossOverTest6()
    {
        var i1 = new Individual(new TestPopulationPolicy(), [6, 7, 8, 9, 10]);
        var i2 = new Individual(new TestPopulationPolicy(), [1, 2, 3, 4, 5]);
        var c = new OnePoint();
        var offSpring = c.CrossOver(new TestPopulationPolicy(), 5, i1, i2);
        Assert.IsTrue(offSpring.Chromosome.SequenceEqual(new List<int> { 6, 7, 8, 9, 10 }));
    }

    [TestMethod]
    public void PopulationTest1()
    {
        var policy = new DefaultPopulationPolicy(GeTestFitness1.Definition, 100, 100);
        var population = new Population(policy, GeTestFitness1.FitnessFunction, null);
        var startFitness = Population.MedianFitness(population.GetPopulation(), 0);
        population.Run();
        var endFitness = Population.MedianFitness(population.GetPopulation(), 0);
        Assert.IsTrue(endFitness < startFitness);
    }

    [TestMethod]
    public void PopulationTest2()
    {
        var policy = new DefaultPopulationPolicy(GeTestFitness2.Definition, 100, 100);
        var population = new Population(policy, GeTestFitness2.FitnessFunction, null);
        var startFitness = Population.MedianFitness(population.GetPopulation(), 0);
        population.Run();
        var endFitness = Population.MedianFitness(population.GetPopulation(), 0);
        Assert.IsTrue(endFitness < startFitness);
    }
}