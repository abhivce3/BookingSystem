using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Utils;

public class TestServerCallContext : ServerCallContext
{
    private TestServerCallContext() { }

    public static ServerCallContext Create()
    {
        return new TestServerCallContext();
    }

    protected override AuthContext AuthContextCore => new AuthContext(string.Empty, new Dictionary<string, List<AuthProperty>>());
    protected override ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions options) => throw new NotImplementedException();
    protected override CancellationToken CancellationTokenCore => CancellationToken.None;
    protected override DateTime DeadlineCore => DateTime.UtcNow.AddMinutes(5);
    protected override string HostCore => "localhost";
    protected override string MethodCore => "/test";
    protected override string PeerCore => "peer";
    protected override IDictionary<object, object> UserStateCore { get; } = new Dictionary<object, object>();
    protected override Metadata RequestHeadersCore => new Metadata();
    protected override Metadata ResponseTrailersCore => new Metadata();
    protected override Status StatusCore { get; set; } = new Status(StatusCode.OK, string.Empty);
    protected override WriteOptions WriteOptionsCore { get; set; }
    protected override Task WriteResponseHeadersAsyncCore(Metadata responseHeaders) => Task.CompletedTask;
}
