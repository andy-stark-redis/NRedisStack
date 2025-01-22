// EXAMPLE: cmds_set
// HIDE_START
using StackExchange.Redis;
// HIDE_END
// REMOVE_START
namespace Doc;
[Collection("DocsTests")]
// REMOVE_END

// HIDE_START
public class CmdsSet
{

    public void run()
    {
        var muxer = ConnectionMultiplexer.Connect("localhost:6379");
        var db = muxer.GetDatabase();
        // REMOVE_START
        // Clear any keys here before using them in tests.
        db.KeyDelete("myset");
        // REMOVE_END
        // HIDE_END

        // STEP_START sadd
        bool sAddResult1 = db.SetAdd("myset", "Hello");
        Console.WriteLine(sAddResult1); // >>> True

        bool sAddResult2 = db.SetAdd("myset", "World");
        Console.WriteLine(sAddResult2); // >>> True

        bool sAddResult3 = db.SetAdd("myset", "World");
        Console.WriteLine(sAddResult2); // >>> False

        RedisValue[] sAddResult4 = db.SetMembers("myset");
        Console.WriteLine(string.Join(", ", sAddResult4));
        // >>> Hello, World
        // STEP_END
        // REMOVE_START
        Assert.True(sAddResult1);
        Assert.True(sAddResult2);
        Assert.False(sAddResult3);
        Assert.Equal("Hello, World", string.Join(", ", sAddResult4));
        db.KeyDelete("myset");
        // REMOVE_END

        // STEP_START smembers
        long sMembersResult1 = db.SetAdd(
            "myset", new RedisValue[] {"Hello", "World"}
        );
        Console.WriteLine(sMembersResult1); // >>> 2

        RedisValue[] sMembersResult2 = db.SetMembers("myset");
        Console.WriteLine(string.Join(", ", sMembersResult2));
        // >>> Hello, World
        // STEP_END
        // REMOVE_START
        Assert.Equal(2, sMembersResult1);
        Assert.Equal("Hello, World", string.Join(", ", sMembersResult2));
        // REMOVE_END
    }
}