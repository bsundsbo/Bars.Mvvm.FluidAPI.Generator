using Bars.Mvvm.FluidGenerator.Models;
using Bars.Mvvm.FluidApi.Generator;
using FluentAssertions;
using Xunit;

namespace Bars.Mvvm.FluidGenerator.Tests.FluidCodeGenerator;

public class FluidExtensionPropertyParserShould
{
    [Fact]
    public void Return_Empty_When_No_Public_Properties()
    {
        var sourceCode = @"
            public class MyClass
            {
                private string TestProperty { get; set; }
                public static string OtherTestProperty { get; set; }
            }";

        // Arrange
        var classSymbol = SymbolFactory.CreateClassSymbol(sourceCode, "MyClass");
        var result = FluidExtensionPropertyParser.GetPropertyTemplateModels(classSymbol!);

        // Assert
        result.Should()
            .BeEmpty();
    }

    [Fact]
    public void Parse_ICommand_Without_CommandParameter()
    {
        var sourceCode = @"
        using System.Windows.Input;
        public class MyClass
        {
            public ICommand TestCommand { get; set; }
        }";

        // Arrange
        var classSymbol = SymbolFactory.CreateClassSymbol(sourceCode, "MyClass");
        var result = FluidExtensionPropertyParser.GetPropertyTemplateModels(classSymbol!);

        // Assert
        result.Should().HaveCount(1);
        result[0].Should().BeOfType<PropertyTemplateModel>();
        result[0].PropertyName.Should().Be("TestCommand");
    }

    [Fact]
    public void Parse_ICommand_With_CommandParameter()
    {
        var sourceCode = @"
        using System.Windows.Input;
        public class MyClass
        {
            public ICommand TestCommand { get; set; }
            public object TestCommandParameter { get; set; }
        }";

        // Arrange
        var classSymbol = SymbolFactory.CreateClassSymbol(sourceCode, "MyClass");
        var result = FluidExtensionPropertyParser.GetPropertyTemplateModels(classSymbol!);

        // Assert
        result.Should().HaveCount(2);
        var commandWithParameter = result.OfType<CommandWithParameterTemplateModel>()
            .Single();
        commandWithParameter.Should().NotBeNull();
        commandWithParameter.PropertyName.Should().Be("TestCommand");
        commandWithParameter.ParameterPropertyName.Should().Be("TestCommandParameter");
        var commandParameterProperty = result.Except([commandWithParameter])
            .Single();
        commandParameterProperty.Should().NotBeNull();
        commandParameterProperty.PropertyName.Should().Be("TestCommandParameter");
    }

    [Fact]
    public void Parse_Boolean_Properties()
    {
        var sourceCode = @"
        public class MyClass
        {
            public bool IsEnabled { get; set; }
        }";

        // Arrange
        var classSymbol = SymbolFactory.CreateClassSymbol(sourceCode, "MyClass");
        var result = FluidExtensionPropertyParser.GetPropertyTemplateModels(classSymbol!);

        // Assert
        result.Should().HaveCount(1);
        result[0].Should().BeOfType<BooleanPropertyTemplateModel>();
        result[0].PropertyName.Should().Be("IsEnabled");
    }

    [Fact]
    public void Parse_NullableBoolean_Properties()
    {
        var sourceCode = @"
        public class MyClass
        {
            public bool? IsEnabled { get; set; }
        }";

        // Arrange
        var classSymbol = SymbolFactory.CreateClassSymbol(sourceCode, "MyClass");
        var result = FluidExtensionPropertyParser.GetPropertyTemplateModels(classSymbol!);

        // Assert
        result.Should().HaveCount(1);
        result[0].Should().BeOfType<BooleanPropertyTemplateModel>();
        result[0].PropertyName.Should().Be("IsEnabled");
    }

    [Fact]
    public void Parse_ReadOnly_ObservableCollection()
    {
        var sourceCode = @"
        using System.Collections.ObjectModel;
        namespace MyNamespace;

        public class MyClass
        {
            public System.Collections.ObjectModel.ObservableCollection<string> Items { get; } = new();
        }";

        // Arrange
        var classSymbol = SymbolFactory.CreateClassSymbol(sourceCode, "MyNamespace.MyClass");
        var result = FluidExtensionPropertyParser.GetPropertyTemplateModels(classSymbol!);

        // Assert
        result.Should().HaveCount(1);
        result[0].Should().BeOfType<ObservableCollectionTemplateModel>();
        result[0].PropertyName.Should().Be("Items");
    }

    [Fact]
    public void Parse_Mixed_Properties()
    {
        var sourceCode = @"
        using System.Collections.ObjectModel;
        using System.Windows.Input;
        public class MyClass
        {
            public bool IsEnabled { get; set; }
            public ICommand TestCommand { get; set; }
            public ObservableCollection<string> Items { get; }
        }";

        // Arrange
        var classSymbol = SymbolFactory.CreateClassSymbol(sourceCode, "MyClass");
        var result = FluidExtensionPropertyParser.GetPropertyTemplateModels(classSymbol!);

        // Assert
        result.Should().HaveCount(3);
        result.Should().ContainSingle(p => p is BooleanPropertyTemplateModel && p.PropertyName == "IsEnabled");
        result.Should().ContainSingle(p => p != null && p.PropertyName == "TestCommand");
        result.Should().ContainSingle(p => p is ObservableCollectionTemplateModel && p.PropertyName == "Items");
    }
}
