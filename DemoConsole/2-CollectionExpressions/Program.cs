namespace Trit.DemoConsole._2_CollectionExpressions;

public static class Demo
{
    public static Task Main()
    {
        // BEFORE FEATURE: Collection Expressions
        var oldCookies = new List<Cookie>
        {
            new Chocolate(), new Strawberry()
        };

        // FEATURE: Collection Expressions
        List<Cookie> noCookies = [ ];
        List<Cookie> newCookies = [ new Chocolate(), new Strawberry() ];
        List<Cookie> mergedCookies = [
            ..newCookies, new Cola(), ..noCookies
        ];

        // BYOCB, Bring Your Own Collection Builder:
        CookieJar<Cookie> myCookieJar = [ new Chocolate(), new Strawberry() ];

        WriteLine($"{mergedCookies.Count} merged cookies " +
                  $"and {myCookieJar.Count()} in the jar");

        return Task.CompletedTask;
    }

    [CollectionBuilder(typeof(CookieJarFactory), nameof(CookieJarFactory.Create))]
    public class CookieJar<TCookie> : IEnumerable<TCookie> where TCookie : Cookie
    {
        private readonly List<TCookie> _cookies;

        public CookieJar(ReadOnlySpan<TCookie> cookies)
        {
            _cookies = [..cookies];
        }

        public IEnumerator<TCookie> GetEnumerator() => _cookies.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _cookies.GetEnumerator();
    }

    public static class CookieJarFactory
    {
        public static CookieJar<TCookie> Create<TCookie>(ReadOnlySpan<TCookie> items)
            where TCookie : Cookie
        {
            return new CookieJar<TCookie>(items);
        }
    }

    public class Cookie;

    public class Chocolate : Cookie;

    public class Strawberry : Cookie;

    public class Cola : Cookie;
}