using DemoConsole = Trit.DemoConsole;

while (true)
{
    Clear();
    WriteLine("Pick a hex number: ");
    var picked = ReadKey().KeyChar;
    Clear();

    /*
     * Language features:
     * ref readonly parameters as a replacement for `in` parameters, to disallow rvalue
     * collection expressions (i.e. var a = [] {}), in combination with a custom collection builder?
     * interceptors
     * inline arrays
     * nameof with instance members
     * alias for any type
     * Primary Constructors
     * Lambda optional parameters
     *
     * Other features to consider:
     * Private field access via attributes
     * System.Collections.Frozen
     * TimeProvider
     * Tensor primitives
     * System.Text.Json improvements: GetFromJsonAsAsyncEnumerable, JsonNode.ParseAsync, JsonSourceGenerationOptionsAttribute, JsonSerializer.Serialize<Memory<int>>, [JsonConverter(typeof(JsonStringEnumConverter<MyEnum>))], JsonSerializer.IsReflectionEnabledByDefault
     *      [JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
     * ZipFile.CreateFromDirectory ?
     * ASP.NET Core being prepared for AOT: [OptionsValidator] AddSingleton<IValidateOptions<FirstModelNoNamespace>, FirstValidatorNoNamespace>()
     * SHA3_256?
     * Build to container as .tar.gz?
     * AddMetrics() with Meter
     * dotnet build /tl
     * <NuGetAuditLevel>moderate</NuGetAuditLevel>
     * Vector512
     */

    // All sources available
    // @ https://github.com/Team-Rockstars-IT/Trit.CsharpTwelveDemo
    await (char.ToUpperInvariant(picked) switch
    {
        '1' => DemoConsole._1_PrimaryConstructors.Demo.Main(),
        '2' => DemoConsole._2_CollectionExpressions.Demo.Main(),
        _ => Task.CompletedTask
    });

    ReadLine();
}