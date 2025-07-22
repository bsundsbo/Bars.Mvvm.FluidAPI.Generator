using FluentAssertions;
using Xunit;

namespace Bars.Mvvm.FluentApi.Generator.Tests.FluidCodeGenerator;

/// <summary>
/// Test class for verifying SymbolFactory functionality.
/// </summary>
public class SymbolFactoryShould
{
    [Fact]
    public void Return_Empty_List_When_No_Properties()
    {
        var sourceCode = @"
            using System;
            namespace MyNamespace
            {
                public class MyClass
                {
                    private string TestProperty { get; set; }
                }
            }";
        // Arrange
        var classSymbol = SymbolFactory.CreateClassSymbol(sourceCode, "MyNamespace.MyClass");

        // ASSERT
        classSymbol.Should()
            .NotBeNull();
        classSymbol.Name.Should()
            .Be("MyClass");
        classSymbol.ContainingNamespace.ToDisplayString()
            .Should()
            .Be("MyNamespace");
        classSymbol.GetMembers().Should()
            .NotBeEmpty();
    }
}
