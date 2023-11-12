using Microsoft.Extensions.Time.Testing;

namespace Trit.DemoConsole._8_TimeProvider;

public static class Demo
{
    public static async Task Main()
    {
        // FEATURE: API to provide the current system time
        var fakeTime = new FakeTimeProvider();
        ServiceProvider services = new ServiceCollection()
            // .AddSingleton(TimeProvider.System)
            .AddSingleton<TimeProvider>(fakeTime)
            .AddTransient<CleanUpWorker>()
            .BuildServiceProvider();

        await using (var _ = services.GetRequiredService<CleanUpWorker>())
        {
            // When dependency injected:
            fakeTime.Advance(TimeSpan.FromDays(1));
        }

        // When passed to Task methods:
        Task dayOfWaiting = Wait42Days(fakeTime);
        fakeTime.Advance(TimeSpan.FromDays(42));
        await dayOfWaiting;

        fakeTime.AutoAdvanceAmount = TimeSpan.FromDays(42);
        WriteLine($"After auto-advance, first call: {fakeTime.GetUtcNow()}");
        WriteLine($"After auto-advance, second call: {fakeTime.GetUtcNow()}");
    }

    internal static async Task Wait42Days(TimeProvider timeProvider)
    {
        WriteLine($"[{timeProvider.GetUtcNow()}]: Going to wait 42 days...");
        await Task.Delay(TimeSpan.FromDays(1), timeProvider);
        WriteLine($"[{timeProvider.GetUtcNow()}]: Done waiting!");
    }

    private sealed class CleanUpWorker : IDisposable, IAsyncDisposable
    {
        private readonly TimeProvider _timeProvider;
        private readonly ITimer _timer;

        public CleanUpWorker(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
            _timer = timeProvider.CreateTimer(CleanUp,
                state: null,
                dueTime: TimeSpan.Zero,
                period: TimeSpan.FromDays(1));
        }

        private void CleanUp(object? state)
        {
            WriteLine($"[{_timeProvider.GetUtcNow()}]: Cleaning up!");
        }

        public void Dispose() => _timer.Dispose();
        public ValueTask DisposeAsync() => _timer.DisposeAsync();
    }
}