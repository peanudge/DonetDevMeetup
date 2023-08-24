// Example 1. DomainLanguageExample.cs
// DomainLanguageExample.Run();

// Example 2. LinkExtractorExample.cs
// await LinkExtractorExample.Run();

// Example 3. ReverseProxyExample.cs
using var host = new SimpleAsyncHost();
host.ListenAsync().Wait();
