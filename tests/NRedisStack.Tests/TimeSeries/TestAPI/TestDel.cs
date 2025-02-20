using NRedisStack.DataTypes;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using Xunit;


namespace NRedisStack.Tests.TimeSeries.TestAPI
{
    public class TestDel : AbstractNRedisStackTest, IDisposable
    {
        private readonly string key = "DEL_TESTS";

        public TestDel(EndpointsFixture endpointsFixture) : base(endpointsFixture) { }


        private List<TimeSeriesTuple> CreateData(ITimeSeriesCommands ts, int timeBucket) //TODO: check this
        {
            var tuples = new List<TimeSeriesTuple>();
            for (int i = 0; i < 10; i++)
            {
                TimeStamp timeStamp = ts.Add(key, i * timeBucket, i);
                tuples.Add(new TimeSeriesTuple(timeStamp, i));
            }
            return tuples;
        }

        [Fact]
        public void TestDelNotExists()
        {
            IDatabase db = GetCleanDatabase();
            var ts = db.TS();
            var ex = Assert.Throws<RedisServerException>(() => ts.Del(key, "-", "+"));
            Assert.Equal("ERR TSDB: the key does not exist", ex.Message);
        }

        [Fact]
        public void TestDelRange()
        {
            IDatabase db = GetCleanDatabase();
            var ts = db.TS();
            var tuples = CreateData(ts, 50);
            TimeStamp from = tuples[0].Time;
            TimeStamp to = tuples[5].Time;
            Assert.Equal(6, ts.Del(key, from, to));

            // check that the operation deleted the timestamps
            IReadOnlyList<TimeSeriesTuple> res = ts.Range(key, from, to);
            Assert.Empty(res);
            Assert.NotNull(ts.Get(key));
        }
    }
}
