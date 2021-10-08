# EXPERIMENTAL Lightstep Distro for OpenTelemetry DotNet

This is the Lightstep package for configuring OpenTelemetry

### Run

```shell script
export LS_ACCESS_TOKEN=your-token
export OTEL_SERVICE_NAME=your-service
```

And in your main block, in order to have all activites traced:

```cs
        static void Main(string[] args)
        {
            using var config = OpenTelemetryConfiguration.CreateConfiguration();
```

See the [example](Example/Program.cs)
