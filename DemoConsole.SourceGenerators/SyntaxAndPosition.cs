using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Trit.DemoConsole.SourceGenerators;

public record SyntaxAndPosition(InvocationExpressionSyntax Syntax, FileLinePositionSpan Position);