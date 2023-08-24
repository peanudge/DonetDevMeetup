using System.Net;
using System.Text;
using System.Text.RegularExpressions;



// https://gist.github.com/stanroze/11008822
public sealed class SimpleAsyncHost : IDisposable
{
    private HttpListener _httpListener;
    private Lazy<HttpClient> _httpClientFactory;
    private bool _disposed;

    public SimpleAsyncHost(string prefix = "http://localhost:10090/")
    {
        Console.Out.WriteLine($"Prefix: {prefix}");
        _httpListener = new HttpListener();
        _httpListener.Prefixes.Add(prefix);
        _httpClientFactory = new Lazy<HttpClient>();
    }

    public async Task ListenAsync()
    {
        _httpListener.Start();

        while (!_disposed)
        {
            HttpListenerContext ctx = null;
            try
            {
                ctx = await _httpListener.GetContextAsync();
            }
            catch (HttpListenerException ex)
            {
                if (ex.ErrorCode == 995) return;
            }

            if (ctx == null) continue;

            var client = _httpClientFactory.Value;
            var content = await client.GetStringAsync("http://www.example.com/");
            content = Regex.Replace(content, @"<a[^>]*>[^<]*</a>", "<p>링크 치환됨</p>", RegexOptions.Compiled);

            var response = ctx.Response;
            response.Headers.Add(HttpResponseHeader.CacheControl, "private, no-store");
            response.ContentType = "text/html";
            response.StatusCode = (int)HttpStatusCode.OK;

            var messageBytes = Encoding.UTF8.GetBytes(content);
            response.ContentLength64 = messageBytes.Length;
            await response.OutputStream.WriteAsync(messageBytes, 0, messageBytes.Length);
            response.OutputStream.Close();
            response.Close();
        }
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        if (_httpListener.IsListening)
            _httpListener.Stop();

        _disposed = true;
    }
}
