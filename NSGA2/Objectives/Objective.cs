using System;

namespace Nsga2.Objectives;

public sealed class Objective(ObjectiveDirection direction, string name, double min, double max, double value)
{
    public ObjectiveDirection Direction { get; } = direction;
    public double Value { get; } = value;
    public double Range => Max - Min;
    public string Name { get; } = name;
    public double Min { get; } = min;
    public double Max { get; } = max;

    public override string ToString()
    {
        return $"{Name}: {Math.Round(Value, 2)} {Direction}";
    }
}