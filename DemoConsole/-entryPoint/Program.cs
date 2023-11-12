using DemoConsole = Trit.DemoConsole;

while (true)
{
    Clear();
    WriteLine("Pick a hex number: ");
    var picked = ReadKey().KeyChar;
    Clear();

    // All sources available
    // @ https://github.com/Team-Rockstars-IT/Trit.CsharpTwelveDemo
    await (char.ToUpperInvariant(picked) switch
    {
        '1' => DemoConsole._1_PrimaryConstructors.Demo.Main(),
        '2' => DemoConsole._2_CollectionExpressions.Demo.Main(),
        '3' => DemoConsole._3_QualityOfLife.Demo.Main(),
        '4' => DemoConsole._4_Performance.Demo.Main(),
        '5' => DemoConsole._5_Interceptors.Demo.Main(),
        '6' => DemoConsole._6_Reflection.Demo.Main(),
        '7' => DemoConsole._7_Frozen.Demo.Main(),
        '8' => DemoConsole._8_TimeProvider.Demo.Main(),
        '9' => DemoConsole._9_Json.Demo.Main(),
        _ => Task.CompletedTask
    });

    ReadLine();
}