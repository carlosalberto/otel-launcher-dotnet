# EXPERIMENTAL Lightstep Distro for OpenTelemetry DotNet

This is the Lightstep package for configuring OpenTelemetry

### Run

```shell script
export LS_ACCESS_TOKEN=your-token
```

And in your main block, in order to have all activites traced:

```cs
        static void Main(string[] args)
        {
            using var config = OpenTelemetryConfiguration.CreateConfiguration();
```
