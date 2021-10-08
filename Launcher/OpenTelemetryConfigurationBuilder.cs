using System;
using System.Collections.Generic;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Lightstep.OpenTelemetry.Launcher
{
    public class OpenTelemetryConfigurationBuilder
    {
        string accessToken;
        string serviceName;
        string serviceVersion;
        string tracesEndpoint;
        string propagators;

        List<string> sources = new List<string>();

        const string DEFAULT_ACTIVITY_SOURCE = "*";
        const string LS_ACCESS_TOKEN_HEADER = "lightstep-access-token";

        internal OpenTelemetryConfigurationBuilder()
        {
            ReadEnvironmentVariables();
        }

        void ReadEnvironmentVariables()
        {
            accessToken = VariablesConverter.GetAccessToken();
            serviceVersion = VariablesConverter.GetServiceVersion();
            serviceName = VariablesConverter.GetServiceName();
            tracesEndpoint = VariablesConverter.GetTracesEndpoint();
            propagators = VariablesConverter.GetPropagators();
        }

        public OpenTelemetryConfigurationBuilder SetAccessToken(string accessToken)
        {
            this.accessToken = accessToken;
            return this;
        }

        public OpenTelemetryConfigurationBuilder SetServiceName(string serviceName)
        {
            this.serviceName = serviceName;
            return this;
        }

        public OpenTelemetryConfigurationBuilder SetServiceVersion(string serviceVersion)
        {
            this.serviceVersion = serviceVersion;
            return this;
        }

        public OpenTelemetryConfigurationBuilder SetTracesEndpoint(string tracesEndpoint)
        {
            this.tracesEndpoint = tracesEndpoint;
            return this;
        }

        public OpenTelemetryConfigurationBuilder SetPropagators(string propagators)
        {
            this.propagators = propagators;
            return this;
        }

        public OpenTelemetryConfigurationBuilder AddSource(params string[] names)
        {
            // Be nice.
            if (names != null) 
            {
                foreach (var name in names)
                {
                    if (name == null)
                    {
                        continue;
                    }

                    sources.Add(name);
                }
            }

            return this;
        }

        public OpenTelemetryConfiguration Build()
        {
            var otlpExporterOpts = new OtlpExporterOptions() {
                Endpoint = new Uri(tracesEndpoint),
                Headers = LS_ACCESS_TOKEN_HEADER + "=" + accessToken
            };
            var otlpExporter = new OtlpTraceExporter(otlpExporterOpts);
            var traceProcessor = new BatchActivityExportProcessor(
                    otlpExporter,
                    otlpExporterOpts.BatchExportProcessorOptions.MaxQueueSize,
                    otlpExporterOpts.BatchExportProcessorOptions.ScheduledDelayMilliseconds,
                    otlpExporterOpts.BatchExportProcessorOptions.ExporterTimeoutMilliseconds,
                    otlpExporterOpts.BatchExportProcessorOptions.MaxExportBatchSize);

            var tracerProviderBuilder = Sdk.CreateTracerProviderBuilder()
                .AddProcessor(traceProcessor)
                .SetResourceBuilder(
                        ResourceBuilder.CreateDefault().AddService(serviceName, null, serviceVersion)
                );

            if (sources.Count > 0) {
                foreach (var source in sources)
                {
                    tracerProviderBuilder.AddSource(source);
                }
            } else {
                tracerProviderBuilder.AddSource(DEFAULT_ACTIVITY_SOURCE);
            }

            return new OpenTelemetryConfiguration(
                tracerProviderBuilder.Build(),
                traceProcessor
            );
        }
    }
}
