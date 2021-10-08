using System;
using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Trace;

namespace Lightstep.OpenTelemetry.Launcher
{
    public class OpenTelemetryConfiguration : IDisposable
    {
        internal OpenTelemetryConfiguration(
            TracerProvider tracerProvider,
            BaseExportProcessor<Activity> traceProcessor
        )
        {
            TracerProvider = tracerProvider;
            TraceProcessor = traceProcessor;
        }

        public TracerProvider TracerProvider { get; }
        public BaseExportProcessor<Activity> TraceProcessor { get; }

        public void Dispose()
        {
            TracerProvider.Dispose();
        }

        // TODO: Make this thread-local and interchangable
        // (or have OTel DotNet TRULY implement a global instance).
        public static OpenTelemetryConfiguration Current { get; set; }

        public static OpenTelemetryConfiguration CreateConfiguration()
        {
            return new OpenTelemetryConfigurationBuilder().Build();
        }

        public static OpenTelemetryConfigurationBuilder CreateConfigurationBuilder()
        {
            return new OpenTelemetryConfigurationBuilder();
        }
    }
}
