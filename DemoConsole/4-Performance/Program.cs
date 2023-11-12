namespace Trit.DemoConsole._4_Performance;

public static class Demo
{
    // BEFORE FEATURE: ref readonly method parameters
    public static ReadOnlySpan<int> OldCreateSpan(ref int c)
    {
        c++;
        return new ReadOnlySpan<int>(ref c);
    }

    // FEATURE: ref readonly method parameters
    public static ReadOnlySpan<int> NewCreateSpan(ref readonly int c)
        => new ReadOnlySpan<int>(in c);

    // BEFORE FEATURE: Inline arrays
    public struct OldFourBeatMeasure
    {
        internal unsafe fixed int Notes[4];
    }

    // FEATURE: Inline arrays
    [InlineArray(4)]
    public struct FourBeatMeasure
    {
        private MusicalNote _note0;
    }

    public static Task Main()
    {
        ref int mutableReferenceToFortyTwo = ref new FortyTwoHolder().FortyTwo;
        ReadOnlySpan<int> oldSpan = OldCreateSpan(ref mutableReferenceToFortyTwo);
        WriteLine($"Old 42: {oldSpan[0]}");

        ref readonly int readOnlyReferenceToFortyTwo = ref new FortyTwoHolder().FortyTwo;
        ReadOnlySpan<int> newSpan = NewCreateSpan(in readOnlyReferenceToFortyTwo);
        WriteLine($"New 42: {newSpan[0]}");

        unsafe
        {
            var oldMeasure = new OldFourBeatMeasure();
            oldMeasure.Notes[0] = (int)MusicalNote.G;
            oldMeasure.Notes[1] = (int)MusicalNote.E;
            oldMeasure.Notes[2] = (int)MusicalNote.G;
            oldMeasure.Notes[3] = (int)MusicalNote.A;
            var n = oldMeasure.Notes;
            WriteLine($"The old notes are: {n[0]}, {n[1]}, {n[2]}, {n[3]}");
        }

        // Ideally: FourBeatMeasure newMeasure = [MusicalNote.G, ...];
        var newMeasure = new FourBeatMeasure();
        newMeasure[0] = MusicalNote.G;
        newMeasure[1] = MusicalNote.E;
        newMeasure[2] = MusicalNote.G;
        newMeasure[3] = MusicalNote.A;
        WriteLine($"The new notes are: {string.Join(", ", newMeasure[..4].ToArray())}");

        return Task.CompletedTask;
    }

    public enum MusicalNote { A, B, C, D, E, F, G }

    private sealed class FortyTwoHolder { public int FortyTwo = 42; }
}