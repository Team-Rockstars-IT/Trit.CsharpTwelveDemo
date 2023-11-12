﻿using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Trit.DemoConsole.SourceGenerators;

[Generator]
public partial class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<FileLinePositionSpan> classDeclarations =
            context.SyntaxProvider
                .CreateSyntaxProvider(

                    predicate: static (syntaxNode, _) =>
                        syntaxNode is InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax access } invocation
                        && access.Name.ToString() == nameof(Console.WriteLine)
                        && invocation.ArgumentList.Arguments.Count == 1,

                    transform: static (genContext, cancellationToken) =>
                        genContext.Node.SyntaxTree.GetLineSpan(
                            ((MemberAccessExpressionSyntax)((InvocationExpressionSyntax)genContext.Node).Expression).Name.Span,
                            cancellationToken)
                );

        IncrementalValueProvider<(Compilation, ImmutableArray<FileLinePositionSpan>)> compilationAndClasses
            = context.CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses, (spc, source) => OnGenerate(spc, source.Item2));
    }

    private static void OnGenerate(
        SourceProductionContext context,
        ImmutableArray<FileLinePositionSpan> input)
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
        foreach (FileLinePositionSpan position in input)
        {
            var line = position.Span.Start.Line + 1;
            var character = position.Span.Start.Character + 1;

            if (position.Path.Contains("Interceptors"))
            {
                builder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "        [InterceptsLocation(@\"{0}\", {1}, {2})]\n",
                    position.Path.Replace(".\\", ""), line, character);
            }
        }
        builder.AppendLine("        public static void WriteLine(string? value) => System.Console.WriteLine($\"[{System.DateTime.UtcNow:o}]: {value}\");");
        builder.AppendLine("    }");
        builder.AppendLine("}");

        context.AddSource("Interceptors.generated.cs", builder.ToString());
    }
}