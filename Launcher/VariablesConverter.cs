using System;

namespace Lightstep.OpenTelemetry.Launcher
{
    public static class VariablesConverter
    {
        const string LS_ACCESS_TOKEN = "LS_ACCESS_TOKEN";
        const string LS_SERVICE_VERSION = "LS_SERVICE_VERSION";

        const string OTEL_SERVICE_NAME = "OTEL_SERVICE_NAME";
        const string OTEL_EXPORTER_OTLP_TRACES_ENDPOINT = "OTEL_EXPORTER_OTLP_TRACES_ENDPOINT";
        const string OTEL_PROPAGATORS = "OTEL_PROPAGATORS";

        const string DEFAULT_EXPORTER_OTLP_TRACES_ENDPOINT = "https://ingest.lightstep.com:443";
        const string DEFAULT_PROPAGATOR = "b3multi";

        public static string GetAccessToken()
        {
            return Environment.GetEnvironmentVariable(LS_ACCESS_TOKEN);
        }

        public static string GetServiceVersion()
        {
            return Environment.GetEnvironmentVariable(LS_SERVICE_VERSION);
        }

        public static string GetServiceName()
        {
            return Environment.GetEnvironmentVariable(OTEL_SERVICE_NAME);
        }

        public static string GetTracesEndpoint()
        {
            string value = Environment.GetEnvironmentVariable(OTEL_EXPORTER_OTLP_TRACES_ENDPOINT);
            if (value == null)
            {
                value = DEFAULT_EXPORTER_OTLP_TRACES_ENDPOINT;
            }

            return value;
        }

        public static string GetPropagators()
        {
            string value = Environment.GetEnvironmentVariable(OTEL_PROPAGATORS);
            if (value == null)
            {
                value = DEFAULT_PROPAGATOR;
            }

            return value;
        }
    }
}
