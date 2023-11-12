namespace Trit.DemoConsole._5_Interceptors;

public static class Demo
{
    public static async Task Main()
    {
        Console.WriteLine("This will be prefixed by the date and time");
        await Task.Delay(20);
        Console.WriteLine("And this too");
    }
}