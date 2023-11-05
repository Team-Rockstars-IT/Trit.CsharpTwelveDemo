namespace Trit.DemoConsole._3_QualityOfLife;

// BEFORE FEATURE: Alias any type
using OldCar = System.Tuple<string, string, System.Drawing.Color>;

// FEATURE: Alias any type
using Car = (string Brand, string Model, Color Color);
using unsafe Numbers = int*;

public static class Demo
{
    // BEFORE FEATURE: Support default parameter values in lambdas
    public delegate double OldPowerOf(int x, int y = 2);

    public static Task Main()
    {
        unsafe
        {
            var oldCar = new OldCar("Ferrari", "F40", Color.Red);
            var newCar = new Car("Tesla", "Model Y", Color.Silver);
            Numbers numbers = stackalloc int[] { 1, 2, 3 };

            WriteLine($"Old Car: {oldCar}\nNew Car: {newCar}\nFirst: {*numbers}");
        }

        WriteLine();

        // BEFORE FEATURE: Support default parameter values in lambdas
        OldPowerOf oldPowerOf = (x, y) => Math.Pow(x, y);
        WriteLine($"42 raised to the power of 2, the old way, is: {oldPowerOf(42)}");

        // FEATURE: Support default parameter values in lambdas
        var powerOf = (int x, int y = 2) => Math.Pow(x, y);
        WriteLine($"42 raised to the power of 2 is: {powerOf(42)}");
        powerOf = (int x, int y = 3) => Math.Pow(x, y);
        WriteLine($"42 raised to the power of 3 is(n't): {powerOf(42)}");

        WriteLine();

        WriteLine($"Old name: {DataSize.OldNameOfSmallUnit()}");
        WriteLine($"New name: {DataSize.NameOfSmallUnit()}");

        return Task.CompletedTask;
    }

    public record DataSize(Gibibytes Gibibytes)
    {
        // BEFORE FEATURE: Allow nameof to always access instance members from static context
        public static string OldNameOfSmallUnit() => "Mwbibyte";

        // FEATURE: Allow nameof to always access instance members from static context
        public static string NameOfSmallUnit() => nameof(Gibibytes.Mebibytes);
    }

    public record struct Gibibytes(int Value)
    {
        public int Mebibytes => Value * 1_024;
    }
}