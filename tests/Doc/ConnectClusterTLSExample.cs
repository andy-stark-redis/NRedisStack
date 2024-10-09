// EXAMPLE: connect_cluster_tls

// STEP_START connect_cluster_tls

using StackExchange.Redis;
using System.Security.Cryptography.X509Certificates;

// REMOVE_START
using NRedisStack.Tests;
using System.Net.Security;

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
                    {"localhost", 6379},
                    // { "localhost", 6380},  // Specify your own cluster hosts and ports.
                    // { "localhost", 6381}
                },
                User="yourUsername",     // This is ignored if username is not configured.
                Password="yourPassword", // This is ignored if password is not configured.
                Ssl = true,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12 
        };

        options.CertificateValidation += ValidateServerCertificate;

        bool ValidateServerCertificate(
                object sender,
                X509Certificate? certificate,
                X509Chain? chain,
                SslPolicyErrors sslPolicyErrors)
        {
            if (certificate == null) {
                return false;       
            }

            var ca = new X509Certificate2("redis_ca.pem");
            bool verdict = (certificate.Issuer == ca.Subject);
            if (verdict) {
                return true;
            }
            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);
            return false;
        }
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
