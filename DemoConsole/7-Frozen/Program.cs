using System.Collections.Frozen;

namespace Trit.DemoConsole._7_Frozen;

public static class Demo
{
    // FEATURE: Provide optimized read-only collections
    private static readonly FrozenDictionary<string, int> PlanetsBydistinceToTheSun;
    private static readonly FrozenDictionary<int, string> DistincesToTheSun;
    private static readonly FrozenDictionary<string, long> RandomNumbers;
    private static readonly FrozenSet<string> AllPlanets;

    static Demo()
    {
        // BEFORE FEATURE: Provide optimized read-only collections
        var distinceToTheSun = new Dictionary<string, int>
        {
            { "Mercury", 3 },
            { "Venus", 6 },
            { "Earth", 8 },
            { "Mars", 12 },
            { "Jupiter", 43 },
            { "Saturn", 80 },
            { "Uranus", 160 },
            { "Neptune", 250 },
        };

        PlanetsBydistinceToTheSun = distinceToTheSun.ToFrozenDictionary();
        DistincesToTheSun = distinceToTheSun.ToDictionary(kvp => kvp.Value, kvp => kvp.Key).ToFrozenDictionary();
        AllPlanets = distinceToTheSun.Keys.ToFrozenSet();

        var rnd = new Random();
        RandomNumbers = Enumerable.Range(0, 1000)
            .ToDictionary(i => i.ToString(CultureInfo.InvariantCulture), _ => rnd.NextInt64())
            .ToFrozenDictionary();
    }

    public static Task Main()
    {
        WriteLine($"When using just a few keys, we can get: {PlanetsBydistinceToTheSun.GetType().Name}");
        WriteLine($"When using numeric keys, we can get: {DistincesToTheSun.GetType().Name}");
        WriteLine($"When using a lot more keys, we can get: {RandomNumbers.GetType().Name}");

        WriteLine($"Our distinance to the sun, in light-minutes: {PlanetsBydistinceToTheSun["Earth"]}");

        WriteLine($"Is Pluto a planet? {(AllPlanets.Contains("Pluto") ? "Yes" : "No")}.");

        return Task.CompletedTask;
    }
}