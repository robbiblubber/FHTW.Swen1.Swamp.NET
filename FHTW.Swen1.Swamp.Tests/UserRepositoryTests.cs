using System.Data;
using System.Reflection;



namespace FHTW.Swen1.Swamp.Tests
{
    /// <summary>This class implements tests for the user repository.</summary>
    public class UserRepositoryTests
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private members                                                                                                  //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>User repository.</summary>
        private UserRepository? _Repository;

        /// <summary>Database connection.</summary>
        private IDbConnection? _Cn;

        /// <summary>Test user.</summary>
        private User? _TestUser;



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // setup methods                                                                                                    //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Sets up the test environment.</summary>
        [OneTimeSetUp]
        public void Setup()
        {
            _Repository = (UserRepository?) typeof(User).GetField("_Repository", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);
            _Cn = (IDbConnection?) typeof(Repository<User>).GetField("_Cn", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null);

            if(_Cn != null)
            {
                using(IDbCommand cmd = _Cn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM USERS WHERE ID = 'test'";
                    cmd.ExecuteNonQuery();
                }
            }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // test methods                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Test user creation.</summary>
        [Test][Order(0)]
        public void Test_CreateUser()
        {
            if(_Repository == null)
            {
                Assert.Fail("Failed to access repository.");
                return;
            }
            if(_Cn == null)
            {
                Assert.Fail("Failed to access database.");
                return;
            }

            _Repository.Create("test", "Test", "test");

            using(IDbCommand cmd = _Cn.CreateCommand())
            {
                cmd.CommandText = "SELECT Count(*) FROM USERS WHERE ID = 'test'";
                Assert.That(Convert.ToInt32(cmd.ExecuteScalar()), Is.EqualTo(1));
            }
        }


        /// <summary>Test user creation.</summary>
        [Test][Order(1)]
        public void Test_GetUser()
        {
            if(_Repository == null)
            {
                Assert.Fail("Failed to access repository.");
                return;
            }
            if(_Cn == null)
            {
                Assert.Fail("Failed to access database.");
                return;
            }

            _TestUser = _Repository.Get("test");

            if(_TestUser == null)
            {
                Assert.Fail("User 'test' could not be retreived.");
                return;
            }

            Assert.That(_TestUser.Name, Is.EqualTo("Test"));
        }


        /// <summary>Tests user password verification.</summary>
        [Test][Order(2)]
        public void Test_VerifyPassword()
        {
            if(_Repository == null)
            {
                Assert.Fail("Failed to access repository.");
                return;
            }
            if(_Cn == null)
            {
                Assert.Fail("Failed to access database.");
                return;
            }

            if(_TestUser == null)
            {
                Assert.Fail("User 'test' could not be retreived.");
                return;
            }

            Assert.IsTrue(_Repository.VerifyPassword(_TestUser, "test"));
            Assert.IsFalse(_Repository.VerifyPassword(_TestUser, "Test"));
            Assert.IsFalse(_Repository.VerifyPassword(_TestUser, ""));
            Assert.IsFalse(_Repository.VerifyPassword(_TestUser, "X3b"));
        }


        /// <summary>Tests user password verification.</summary>
        [Test][Order(3)]
        public void Test_ChangePassword()
        {
            if(_Repository == null)
            {
                Assert.Fail("Failed to access repository.");
                return;
            }
            if(_Cn == null)
            {
                Assert.Fail("Failed to access database.");
                return;
            }

            if(_TestUser == null)
            {
                Assert.Fail("User 'test' could not be retreived.");
                return;
            }

            _Repository.ChangePassword(_TestUser, "Pinky");

            Assert.IsTrue(_Repository.VerifyPassword(_TestUser, "Pinky"));
            Assert.IsFalse(_Repository.VerifyPassword(_TestUser, "pinky"));
            Assert.IsFalse(_Repository.VerifyPassword(_TestUser, ""));
            Assert.IsFalse(_Repository.VerifyPassword(_TestUser, "X3b"));
        }


        /// <summary>Tests user saving.</summary>
        [Test][Order(4)]
        public void Test_SaveUserChanges()
        {
            if(_Repository == null)
            {
                Assert.Fail("Failed to access repository.");
                return;
            }
            if(_Cn == null)
            {
                Assert.Fail("Failed to access database.");
                return;
            }

            if(_TestUser == null)
            {
                Assert.Fail("User 'test' could not be retreived.");
                return;
            }

            _TestUser.Name = "Hans the Test";

            _Repository.Save(_TestUser);

            using(IDbCommand cmd = _Cn.CreateCommand())
            {
                cmd.CommandText = "SELECT Count(*) FROM USERS WHERE ID = 'test' AND NAME = 'Hans the Test'";
                Assert.That(Convert.ToInt32(cmd.ExecuteScalar()), Is.EqualTo(1));
            }
        }


        [Test][Order(5)]
        public void Test_DeleteUser()
        {
            if(_Repository == null)
            {
                Assert.Fail("Failed to access repository.");
                return;
            }
            if(_Cn == null)
            {
                Assert.Fail("Failed to access database.");
                return;
            }

            if(_TestUser == null)
            {
                Assert.Fail("User 'test' could not be retreived.");
                return;
            }

            _Repository.Delete(_TestUser);

            using(IDbCommand cmd = _Cn.CreateCommand())
            {
                cmd.CommandText = "SELECT Count(*) FROM USERS WHERE ID = 'test'";
                Assert.That(Convert.ToInt32(cmd.ExecuteScalar()), Is.EqualTo(0));
            }
        }
    }
}