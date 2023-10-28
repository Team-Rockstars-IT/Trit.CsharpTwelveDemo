namespace Trit.DemoConsole._3_QualityOfLife;

// BEFORE FEATURE: Alias any type
using OldCar = System.Tuple<string, string, System.Drawing.Color>;

// FEATURE: Alias any type
using Car = (string Brand, string Model, Color Color);
using unsafe Numbers = int*;

public static class Demo
{
    public static Task Main()
    {
        unsafe
        {
            var oldCar = new OldCar("Ferrari", "F40", Color.Red);
            var newCar = new Car("Tesla", "Model Y", Color.Silver);
            Numbers numbers = stackalloc int[] { 1, 2, 3 };

            WriteLine($"Old Car: {oldCar}\nNew Car: {newCar}\nFirst: {*numbers}");

            return Task.CompletedTask;
        }
    }
}