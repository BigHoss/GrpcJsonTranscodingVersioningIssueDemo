
namespace VersioningIssueDemo.Services;

[ApiVersion(2.0)]
public class GreeterServiceV2 : GreeterV2.GreeterV2Base
{
    private readonly ILogger<GreeterServiceV2> _logger;

    public GreeterServiceV2(ILogger<GreeterServiceV2> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReplyV2> SayHello(HelloRequestV2 request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReplyV2
        {
            Message = "Hello " + request.Name + " from V2"
        });
    }
}