#pragma warning disable CS8604

using System;
using System.Net.Http;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Presentation;

public class IntegrationTestBase : IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;

    public readonly GrpcChannel channel;

    public IntegrationTestBase()
    {
        _factory = new WebApplicationFactory<Program>(); // In Memory Host

        HttpClient client = _factory.CreateDefaultClient();
        
        channel = GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions {
            HttpClient = client
        });
    }

    public void Dispose() => _factory.Dispose();
}