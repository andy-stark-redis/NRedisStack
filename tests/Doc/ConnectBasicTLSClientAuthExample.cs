// EXAMPLE: connect_basic_tls_client_auth

// STEP_START connect_basic_tls_client_auth

using StackExchange.Redis;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;

// REMOVE_START
using NRedisStack.Tests;

namespace Doc;
[Collection("DocsTests")]
// REMOVE_END

public class ConnectBasicTLSClientAuthExample
{

    [SkipIfRedis(Is.OSSCluster)]
    public void run()
    {
        var configurationOptions = new ConfigurationOptions
        {
            EndPoints = { {"redis-12296.c34486.eu-west-2-mz.ec2.cloud.rlrcp.com", 12296} },
            Ssl = true,
            SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13,
            User = "default",
            Password = "umB4WwGj3Awk8oR94HyBysaubc2QNG3q"
        };

        configurationOptions.TrustIssuer("/Users/andrew.stark/Documents/Repos/forks/NRedisStack/tests/Doc/redis_ca.pem");

        configurationOptions.CertificateSelection += delegate
        {
            return new X509Certificate2(
                "/Users/andrew.stark/Documents/Repos/forks/NRedisStack/tests/Doc/redis.pfx",
                "secret"
            ); // use the password you specified for pfx file
        };

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
