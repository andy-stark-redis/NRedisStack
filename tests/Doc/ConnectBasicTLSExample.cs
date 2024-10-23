// EXAMPLE: connect_basic_tls

// STEP_START connect_basic_tls

using StackExchange.Redis;
using System.Security.Authentication;
// REMOVE_START
using NRedisStack.Tests;

namespace Doc;
[Collection("DocsTests")]
// REMOVE_END

public class ConnectBasicTLSExample
{

    [SkipIfRedis(Is.OSSCluster)]
    public void run()
    {
        ConfigurationOptions configurationOptions = new ConfigurationOptions{
                EndPoints= { { "<host>", <port> } },
                User="default",     // This is ignored if username is not configured.
                Password="<password>", // This is ignored if password is not configured.
                Ssl = true,
                SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13
        };

        configurationOptions.TrustIssuer("<path_to_redis_ca.pem_file");

        var muxer = ConnectionMultiplexer.Connect(configurationOptions);
        var db = muxer.GetDatabase();
        //REMOVE_START
        // Clear any keys here before using them in tests.
        db.KeyDelete("foo");
        //REMOVE_END

        db.StringSet("foo", "bar");
        RedisValue result = db.StringGet("foo");
        Console.WriteLine(result); // >>> bar
        
        // REMOVE_START
        // Tests for 'basic_connect' step.
        Assert.Equal("bar", result);
        // REMOVE_END
    }
}
// STEP_END
