namespace CK.Rest.Proxy.Tests
{
    public enum TestMode
    {
        Proxy,
        Direct
    }

    public static class TestConfiguration
    {
        // Change this to switch between modes
        public const TestMode CurrentTestMode = TestMode.Direct;

        private const string ProxyHost = "http://localhost:8080";
        private const string UsersHost = "http://localhost:8084";
        private const string PostsHost = "http://localhost:8083";
        private const string LanguagesHost = "http://localhost:8082";

        public static string GetHost(string service)
        {
            if (CurrentTestMode == TestMode.Proxy)
            {
                return $"{ProxyHost}/{service}";
            }

            return service switch
            {
                "Auth" => UsersHost,
                "User" => UsersHost,
                "Post" => PostsHost,
                "Language" => LanguagesHost,
                _ => throw new System.ArgumentOutOfRangeException(nameof(service), "Invalid service name")
            };
        }
    }
}
