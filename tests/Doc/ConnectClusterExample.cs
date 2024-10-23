// EXAMPLE: connect_cluster

// STEP_START connect_cluster

// REMOVE_START
using NRedisStack.Tests;
// REMOVE_END
using StackExchange.Redis;

// REMOVE_START
namespace Doc;
[Collection("DocsTests")]
// REMOVE_END

public class ConnectClusterExample
{
    [SkipIfRedis(Is.Enterprise)]
    public void run()
    {
        var muxer = ConnectionMultiplexer.Connect(
            new ConfigurationOptions{
                EndPoints= {
                    {"redis-13891.c34425.eu-west-2-mz.ec2.cloud.rlrcp.com", 13891}
                },
                User="default",    // This is ignored if username is not configured.
                Password="wtpet4pI5EgyJHyldPwR7xM7GaZB0EcG" // This is ignored if password is not configured.
            }
        );
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
