using VersioningIssueDemo.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddGrpc();
services.AddGrpcSwagger();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "versioning issue demo", Version = "v1" });
});

_ = services.AddApiVersioning(setup =>
    {
        setup.DefaultApiVersion = new(2, 0);
        setup.AssumeDefaultVersionWhenUnspecified = true;
        setup.ReportApiVersions = true;
    })
    .AddApiExplorer(options =>
    {
        options.SubstituteApiVersionInUrl = true;
        options.GroupNameFormat = "'v'VVV";
    });

var app = builder.Build();

var versionSet = app.NewApiVersionSet()
    .HasApiVersion(new(1.0))
    .HasApiVersion(new(2.0))
    .ReportApiVersions()
    .Build();



app.MapGrpcService<GreeterService>()
    .MapToApiVersion(1.0)
    .WithApiVersionSet(versionSet);

app.MapGrpcService<GreeterServiceV2>()
    .MapToApiVersion(2.0)
    .WithApiVersionSet(versionSet);


app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    foreach (var description in app.DescribeApiVersions())
    {
        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
    }
});

app.Run();