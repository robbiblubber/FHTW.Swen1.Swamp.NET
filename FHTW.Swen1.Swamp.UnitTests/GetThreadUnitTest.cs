using FHTW.Swen1.Swamp.Handlers;
using FHTW.Swen1.Swamp.Server;

using NSubstitute;



namespace FHTW.Swen1.Swamp.UnitTests
{
    /// <summary>Implements tests for getting threads.</summary>
    [TestFixture]
    public class GetThreadUnitTest
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private members                                                                                                  //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Thread handler instance.</summary>
        private ThreadHandler? _Th;



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // test configuration                                                                                               //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Prepares the test.</summary>
        [SetUp]
        public void Setup()
        {
            _Th = new();
        }


        /// <summary>Cleans up the test.</summary>
        [TearDown]
        public void TearDown()
        {
            _Th = null;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // test configuration                                                                                               //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>This method tests reading a thread.</summary>
        [Test]
        public void Test1()
        {
            HttpSvrEventArgs e = Substitute.For<HttpSvrEventArgs>();
            e.Method.Returns("GET");
            e.Path.Returns("/threads/1");

            _Th!.Handle(e);

            e.Received().Reply(200, Arg.Any<string?>());
        }
    }
}