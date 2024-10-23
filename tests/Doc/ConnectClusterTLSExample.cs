// EXAMPLE: connect_cluster_tls

// STEP_START connect_cluster_tls

using StackExchange.Redis;

// REMOVE_START
using NRedisStack.Tests;

namespace Doc;
[Collection("DocsTests")]
// REMOVE_END

public class ConnectClusterTLSExample
{

    [SkipIfRedis(Is.Enterprise)]
    public void run()
    {
        ConfigurationOptions options = new ConfigurationOptions{
                EndPoints= {
                    { "redis-15313.c34461.eu-west-2-mz.ec2.cloud.rlrcp.com", 15313 }
                },
                User="default",     // This is ignored if username is not configured.
                Password="MrlnkBuSZqO0s0vicIkLnqJXetbSTCan", // This is ignored if password is not configured.
                Ssl = true,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12 
        };

        options.TrustIssuer("/Users/andrew.stark/Documents/Repos/forks/NRedisStack/tests/Doc/redis_ca.pem");
        
        var muxer = ConnectionMultiplexer.Connect(options);
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
