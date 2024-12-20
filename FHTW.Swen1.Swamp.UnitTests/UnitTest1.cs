using FHTW.Swen1.Swamp.Handlers;
using FHTW.Swen1.Swamp.Server;

using NSubstitute;

namespace FHTW.Swen1.Swamp.UnitTests
{
    public class Tests
    {
        private ThreadHandler _Th;


        [SetUp]
        public void Setup()
        {
            _Th = new();
        }


        [Test]
        public void Test1()
        {
            HttpSvrEventArgs e = Substitute.For<HttpSvrEventArgs>();
            e.Method.Returns("GET");
            e.Path.Returns("/threads/1");

            _Th.Handle(e);

            e.Received().Reply(200);
        }

        [TearDown]
        public void TearDown()
        {}
    }
}