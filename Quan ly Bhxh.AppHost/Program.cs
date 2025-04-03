var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ApiGateway>("apigateway");
builder.AddProject<Projects.BhxhWasm>("bhxhwasm");

builder.Build().Run();