using JetBrains.Annotations;
using System;

namespace Bars.Mvvm.FluidApi.Generator;

/// <summary>
/// Represents a module information for reference.
/// </summary>
/// <param name="Name"></param>
/// <param name="Version"></param>
public record ModuleInfo(string Name, [UsedImplicitly(Reason = "Use for debugging")] Version Version);
