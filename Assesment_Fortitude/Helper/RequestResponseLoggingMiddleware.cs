 
using log4net; 
using System.Text; 
using System.Text.RegularExpressions;

namespace Assesment_Fortitude.Helper
{

    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(RequestResponseLoggingMiddleware));

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Log request body
            context.Request.EnableBuffering();

            var requestBodyStream = new MemoryStream();
            await context.Request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);

            var requestBodyText = await new StreamReader(requestBodyStream).ReadToEndAsync();
            requestBodyStream.Seek(0, SeekOrigin.Begin);

            context.Request.Body.Position = 0;

            string safeRequestBody = EncodePasswordsBase64(requestBodyText);
            _logger.Info($"Request {context.Request.Method} {context.Request.Path} Body: {safeRequestBody}");

            // Swap response body to capture response stream
            var originalResponseBody = context.Response.Body;
            using var newResponseBody = new MemoryStream();
            context.Response.Body = newResponseBody;

            await _next(context);

            // Read response body
            newResponseBody.Seek(0, SeekOrigin.Begin);
            string responseBodyText = await new StreamReader(newResponseBody).ReadToEndAsync();

            string safeResponseBody = EncodePasswordsBase64(responseBodyText);
            _logger.Info($"Response {context.Request.Method} {context.Request.Path} Body: {safeResponseBody}");

            // Copy the contents back to the original stream
            newResponseBody.Seek(0, SeekOrigin.Begin);
            await newResponseBody.CopyToAsync(originalResponseBody);
            context.Response.Body = originalResponseBody;
        }

        private string EncodePasswordsBase64(string json)
        {
            if (string.IsNullOrEmpty(json)) return json;

            return Regex.Replace(json,
                @"""PartnerPassword""\s*:\s*""([^""]*)""",
                match =>
                {
                    var original = match.Groups[1].Value;
                    var encoded = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(original));
                    return $"\"PartnerPassword\":\"{encoded}\"";
                },
                RegexOptions.IgnoreCase);
        }
    }
}
