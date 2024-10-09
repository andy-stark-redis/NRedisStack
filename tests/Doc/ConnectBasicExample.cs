// EXAMPLE: connect_basic

// STEP_START connect_basic

// REMOVE_START
using NRedisStack.Tests;
// REMOVE_END
using StackExchange.Redis;

// REMOVE_START
namespace Doc;
[Collection("DocsTests")]
// REMOVE_END

public class ConnectBasicExample
{

    [SkipIfRedis(Is.OSSCluster)]
    public void run()
    {
        var muxer = ConnectionMultiplexer.Connect(
            new ConfigurationOptions{
                EndPoints= {"localhost", "6379"},
                User="yourUsername",    // This is ignored if username is not configured.
                Password="yourPassword" // This is ignored if password is not configured.
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
