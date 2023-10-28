// ReSharper disable All
namespace Trit.DemoConsole._1_PrimaryConstructors;

public static class Demo
{
    public static Task Main()
    {
        var sandwich = new Sandwich(Ingredient.Ham, Ingredient.Jelly, BreadType.WholeGrain);

        WriteLine($"{sandwich}, {(sandwich.IsWholeGrain() ? "It's whole-grain!" : "It's not")}");

        return Task.CompletedTask;
    }

    #region Before

    // BEFORE FEATURE: Primary Constructors
    public class OldSandwich : Food
    {
        private readonly Ingredient? _secondIngredient;
        private readonly BreadType _breadType;

        [WillBeAppliedToTheConstructor]
        public OldSandwich(Ingredient firstIngredient, Ingredient? secondIngredient, BreadType breadType)
            : base(nameof(OldSandwich))
        {
            FirstIngredient = firstIngredient;
            _secondIngredient = secondIngredient;
            _breadType = breadType;
        }

        public Ingredient FirstIngredient { get; }

        public Ingredient? SecondIngredient => _secondIngredient;

        public bool IsWholeGrain() => _breadType == BreadType.WholeGrain;

        public override string ToString() =>
            $"{_breadType} bread with {FirstIngredient}{(_secondIngredient.HasValue ? $"and {_secondIngredient}" : "")}";
    }

    #endregion

    #region After

    [method: WillBeAppliedToTheConstructor]
    // FEATURE: Primary Constructors
    public class Sandwich(Ingredient firstIngredient, Ingredient? secondIngredient, BreadType breadType)
        : Food(name: nameof(Sandwich))
    {
        public Sandwich() : this(Ingredient.PeanutButter, Ingredient.Jelly, BreadType.Sourdough) { }

        // The compiler hints to use this property in ToString,
        // because with how it's written now: two private backing fields are created!
        public Ingredient FirstIngredient { get; } = firstIngredient;

        public Ingredient? SecondIngredient => secondIngredient;

        public bool IsWholeGrain() => breadType == BreadType.WholeGrain;

        public override string ToString() =>
            $"{breadType} bread with {firstIngredient}{(secondIngredient.HasValue ? $"and {secondIngredient}" : "")}";
    }

    #endregion

    #region Support

    public enum Ingredient { PeanutButter, Jelly, Ham, Cheese }

    public enum BreadType { WholeGrain, Sourdough, Rye }

    public class Food(string name);

    [AttributeUsage(AttributeTargets.Constructor)]
    public sealed class WillBeAppliedToTheConstructorAttribute : Attribute;

    #endregion
}