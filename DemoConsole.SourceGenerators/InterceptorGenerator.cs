﻿using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Trit.DemoConsole.SourceGenerators;

[Generator]
public partial class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<SyntaxAndPosition> classDeclarations =
            context.SyntaxProvider
                .CreateSyntaxProvider(

                    predicate: static (syntaxNode, _) =>
                        syntaxNode is InvocationExpressionSyntax { Expression: IdentifierNameSyntax member } invocation
                        && member.Identifier.ToString() == nameof(Console.WriteLine)
                        && invocation.ArgumentList.Arguments.Count == 1,

                    transform: static (genContext, cancellationToken) => new SyntaxAndPosition(
                        (InvocationExpressionSyntax)genContext.Node,
                        genContext.Node.SyntaxTree.GetLineSpan(genContext.Node.Span, cancellationToken))
                );

        IncrementalValueProvider<(Compilation, ImmutableArray<SyntaxAndPosition>)> compilationAndClasses
            = context.CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses, (spc, source) => OnGenerate(spc, source.Item2));
    }

    private static void OnGenerate(
        SourceProductionContext context,
        ImmutableArray<SyntaxAndPosition> input)
    {
        var builder = new StringBuilder();

        builder.AppendLine("// <auto-generated />");
        builder.AppendLine("#nullable enable");
        builder.AppendLine("#pragma warning disable CS9113");
        builder.AppendLine();
        builder.AppendLine("using System.Runtime.CompilerServices;");
        builder.AppendLine();
        builder.AppendLine("namespace System.Runtime.CompilerServices");
        builder.AppendLine("{");
        builder.AppendLine("    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]");
        builder.AppendLine("    internal sealed class InterceptsLocationAttribute(string filePath, int line, int column) : Attribute;");
        builder.AppendLine("}");
        builder.AppendLine();
        builder.AppendLine("namespace Hijacked.DemoConsole.SourceGenerators");
        builder.AppendLine("{");
        builder.AppendLine("    public static class DateAndTimeAppenderIntercepterFactory");
        builder.AppendLine("    {");
        builder.AppendLine("        // FEATURE: Interceptors");
        foreach (SyntaxAndPosition? item in input)
        {
            var line = item.Position.Span.Start.Line + 1;
            var character = item.Position.Span.Start.Character + 1;

            if (item.Position.Path.Contains("Interceptors"))
            {
                builder.AppendFormat("        [InterceptsLocation(@\"{0}\", {1}, {2})]\n", item.Position.Path.Replace(".\\", ""), line, character);
            }
        }
        builder.AppendLine("        public static void WriteLine(string? value) => System.Console.WriteLine($\"[{System.DateTime.UtcNow:o}]: {value}\");");
        builder.AppendLine("    }");
        builder.AppendLine("}");

        context.AddSource("Interceptors.generated.cs", builder.ToString());
    }
}