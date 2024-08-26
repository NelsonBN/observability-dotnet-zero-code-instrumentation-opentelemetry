# Demo - .NET zero-code instrumentation with OpenTelemetry

A demo project to show how to use OpenTelemetry to instrument a .NET with zero-code.


## Documentation

- [.NET zero-code instrumentation](https://opentelemetry.io/docs/zero-code/net/)


## How can use it?

### Run project with Docker Compose

```bash
docker compose up --build
```

### Build Images of projects

In the root of the project, run the following command:

```bash
docker build -t <image-name> -f <dockerfile> .
```

#### `Api.Users`

```bash
docker build -t api-users -f ./src/Demo.Api.Users/Dockerfile .
```


#### `Api.Notifications`

```bash
docker build -t api-notifications -f ./src/Demo.Api.Notifications/Dockerfile .
```
