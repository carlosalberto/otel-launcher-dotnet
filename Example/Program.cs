using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

using OpenTelemetry.Trace;
using Lightstep.OpenTelemetry.Launcher;

namespace Example
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();
        static readonly Random rand = new Random();

        static readonly string[] urls = new string[]{
            "https://docs.microsoft.com/",
            "https://github.com/carlosalberto/otel-launcher-dotnet",
            "https://github.com/open-telemetry/opentelemetry-dotnet",
        };

        // Force sync behavior, to get a few errors now and then.
        static void Main(string[] args)
        {
            using var config = OpenTelemetryConfiguration.CreateConfiguration();
            OpenTelemetryConfiguration.Current = config;

            using (var span = config.TracerProvider.GetTracer("mainApp").StartActiveSpan("main-operation")) {
                for (int i = 0; i < 13; i++) {
                    using (var childSpan = config.TracerProvider.GetTracer("mainApp").StartActiveSpan("request")) {
                        var task = client.GetAsync(urls[rand.Next(urls.Length)]);
                        task.Wait();
                        var res = task.Result;
                        childSpan.SetAttribute("code", res.StatusCode.ToString());
                    }
                }
            }
        }
    }
}
