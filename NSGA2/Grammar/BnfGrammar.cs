using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Nsga2.Population;

namespace Nsga2.Grammar;

public class BnfGrammar : IGrammar
{
    public const string Empty = "EMPTY";
    private const string NonTerminalRegex = "(\\<([^\\<^\\>|^\\s]+)\\>)";
    private const string OptionalRegex = "\\[(.*?)\\]";
    private const string RepetitionRegex = "\\{(.*?)\\}";

    public BnfGrammar(string definition)
    {
        var generatedRules = new Dictionary<string, List<string>>();
        var lines = definition.Split('@', StringSplitOptions.RemoveEmptyEntries);
        foreach (var rules in lines.Select(line => line.Split("::=", StringSplitOptions.None)))
        {
            var name = rules[0].Trim();
            var options = rules[1].Split('|').Select(r => r.Trim()).ToList();
            ProcessOptionals(options, generatedRules);
            ProcessRepetitions(options, generatedRules);
            Rules.Add(name, options);
        }

        Rules = Rules.Concat(generatedRules).ToDictionary(x => x.Key, x => x.Value);
        MaxRuleDepth = Rules.Values.Max(r => r.Count) - 1;
    }

    private int OptionalCount { get; set; }
    private int RepetitionCount { get; set; }
    public IDictionary<string, List<string>> Rules { get; } = new Dictionary<string, List<string>>();
    public int MaxRuleDepth { get; }

    public string Generate(Individual individual)
    {
        var rv = Rules.First().Key;
        var match = Regex.Match(rv, NonTerminalRegex);
        individual.Reset();
        while (match.Success)
        {
            var options = Rules[match.Value];
            rv = rv.SubstituteString(match.Index, match.Length, options[individual.Current % options.Count]);
            match = Regex.Match(rv, NonTerminalRegex);
            if (!match.Success) continue;

            if (!individual.Increment()) return "";
        }

        return rv.Replace(Empty, "");
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var rule in Rules) sb.AppendLine($"{rule.Key} => {string.Join(" | ", rule.Value)}");

        return sb.ToString();
    }

    private void ProcessOptionals(IList<string> options, IDictionary<string, List<string>> generatedRules)
    {
        for (var i = 0; i < options.Count; ++i)
        {
            var match = Regex.Match(options[i], OptionalRegex);
            while (match.Success)
            {
                var name = $"<O{OptionalCount++}>";
                generatedRules.Add(name, [Empty, match.Value.TrimStart('[').TrimEnd(']')]);
                options[i] = options[i].SubstituteString(match.Index, match.Length, $"{name}");
                match = Regex.Match(options[i], OptionalRegex);
            }
        }
    }

    private void ProcessRepetitions(IList<string> options, IDictionary<string, List<string>> generatedRules)
    {
        for (var i = 0; i < options.Count; ++i)
        {
            var match = Regex.Match(options[i], RepetitionRegex);
            while (match.Success)
            {
                var name = $"<R{RepetitionCount++}>";
                generatedRules.Add(name, [Empty, $"{name}{match.Value.TrimStart('{').TrimEnd('}')}"]);
                options[i] = options[i].SubstituteString(match.Index, match.Length, $"{name}");
                match = Regex.Match(options[i], RepetitionRegex);
            }
        }
    }
}