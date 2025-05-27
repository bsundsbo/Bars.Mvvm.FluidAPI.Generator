using Bars.Mvvm.FluidApi.Common;
using Microsoft.CodeAnalysis;

namespace Bars.Mvvm.Resource.Generator;

public record ClassModel(string Name, string Namespace, EquatableList<IPropertySymbol> Properties);
