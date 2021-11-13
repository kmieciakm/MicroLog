using MicroLog.Core.Abstractions;

namespace MicroLog.Core;

/// <summary>
/// Base <see cref="ILogProperty"/> record.
/// </summary>
public record LogProperty : ILogProperty
{
    public string Name { get; init; }
    public string Value { get; init; }
}