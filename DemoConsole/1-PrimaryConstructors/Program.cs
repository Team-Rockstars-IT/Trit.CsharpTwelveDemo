// ReSharper disable All

namespace Trit.DemoConsole._1_PrimaryConstructors;

public static class Demo
{
    public static Task Main()
    {
        var sandwich = new Sandwich(Ingredient.Ham, Ingredient.Jelly, BreadType.WholeGrain);

        WriteLine(
            $"{sandwich}, " +
            $"{(sandwich.IsWholeGrain() ? "it's whole-grain!" : "it's not")}");

        return Task.CompletedTask;
    }

    // BEFORE FEATURE: Primary Constructors
    public class OldSandwich : Food
    {
        private readonly Ingredient? _secondIngredient;
        private readonly BreadType _breadType;

        [WillBeAppliedToTheConstructor]
        public OldSandwich(Ingredient first, Ingredient? second, BreadType bread)
            : base(nameof(OldSandwich))
        {
            FirstIngredient = first;
            _secondIngredient = second;
            _breadType = bread;
        }

        public Ingredient FirstIngredient { get; }

        public Ingredient? SecondIngredient => _secondIngredient;

        public bool IsWholeGrain() => _breadType == BreadType.WholeGrain;

        public override string ToString() =>
            $"{_breadType} bread with {FirstIngredient}" +
            $"{(_secondIngredient.HasValue ? $" and {_secondIngredient}" : "")}";
    }

    [method: WillBeAppliedToTheConstructor]
    // FEATURE: Primary Constructors
    public class Sandwich(Ingredient first, Ingredient? second, BreadType bread)
        : Food(name: nameof(Sandwich))
    {
        // The compiler hints to use this property in ToString,
        // because now two private backing fields are created!
        public Ingredient FirstIngredient { get; } = first;

        public Ingredient? SecondIngredient => second;

        public bool IsWholeGrain() => bread == BreadType.WholeGrain;

        public override string ToString() =>
            $"{bread} bread with {first}{(second.HasValue ? $"and {second}" : "")}";
    }

    public enum Ingredient
    {
        PeanutButter,
        Jelly,
        Ham,
        Cheese
    }

    public enum BreadType
    {
        WholeGrain,
        Sourdough,
        Rye
    }

    public class Food(string name)
    {
        public string Name { get; } = name;
    }

    [AttributeUsage(AttributeTargets.Constructor)]
    public sealed class WillBeAppliedToTheConstructorAttribute : Attribute;
}