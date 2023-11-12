using System.Text.Json;
using System.Text.Json.Serialization;

namespace Trit.DemoConsole._9_Json;

// FEATURE: Feature partity between JsonSerializerOptions and JsonSourceGenerationOptionsAttribute
[JsonSourceGenerationOptions(WriteIndented = true, UseStringEnumConverter = true)]
[JsonSerializable(typeof(Demo.Computer[]))]
internal sealed partial class ComputerStateSerializerContext
    : JsonSerializerContext;

public static class Demo
{
    public enum ComputerState { Crashed, On, Off, Restarting }

    public record Computer(int Id, ComputerState State);

    public static async Task Main()
    {
        var rnd = new Random();
        var c = ComputerStateSerializerContext.Default;
        const string fileName = "temp.json";

        await using (FileStream outputFile = File.Open(fileName, FileMode.Create))
        {
            Computer[] computers = Enumerable.Range(0, 1000)
                .Select(i => new Computer(i, (ComputerState)(rnd.Next() & 3)))
                .ToArray();

            await JsonSerializer
                .SerializeAsync(outputFile, computers, c.ComputerArray);
        }

        WriteLine($"From the file:");
        WriteLine($"{(await File.ReadAllTextAsync(fileName))[..52]}...");
        WriteLine();
        WriteLine($"Reflection enabled by default? " +
                  $"{(JsonSerializer.IsReflectionEnabledByDefault ? "Yes" : "No")}");
        WriteLine();

        // FEATURE: Asynchronously deserialize IAsyncEnumerable<T>
        await using (FileStream outputFile = File.Open(fileName, FileMode.Open))
        {
            IAsyncEnumerable<Computer> computers = JsonSerializer
                .DeserializeAsyncEnumerable(outputFile, c.Computer)!;

            await foreach (Computer computer in computers)
            {
                WriteState(computer);
            }
        }

        File.Delete(fileName);
    }

    private static void WriteState(Computer computer)
    {
        Write(computer.State switch
        {
            ComputerState.Crashed => 'x',
            ComputerState.On => '!',
            ComputerState.Off => '-',
            ComputerState.Restarting => 'r',
            _ => throw new InvalidOperationException("Nope")
        });
    }
}