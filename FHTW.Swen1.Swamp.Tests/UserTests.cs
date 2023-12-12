using System;



namespace FHTW.Swen1.Swamp.Tests
{
    public class UserTests
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // test methods                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Test user creation and retreival.</summary>
        [Test][Order(0)]
        public void Test_UserCreation()
        {
            User test = User.Create("test", "Test", "test");
            User? vest = User.ByID("test");

            if(vest == null)
            {
                Assert.Fail("User 'test' could not be retreived.");
                return;
            }

            Assert.That(test.ID, Is.EqualTo(vest.ID));
            Assert.That(test.Name, Is.EqualTo(vest.Name));
        }


        /// <summary>Test changing and verifying passwords.</summary>
        [Test][Order(1)]
        public void Test_UserPassword()
        {
            User? vest = User.ByID("test");

            if(vest == null)
            {
                Assert.Fail("User 'test' could not be retreived.");
                return;
            }

            vest.ChangePasswordTo("Zorgon");

            Assert.IsTrue(vest.VerifyPassword("Zorgon"));
        }


        /// <summary>Test changing and verifying passwords.</summary>
        [Test][Order(2)]
        public void Test_UserDelete()
        {
            User? vest = User.ByID("test");

            if(vest == null)
            {
                Assert.Fail("User 'test' could not be retreived.");
                return;
            }

            vest.Delete();

            try
            {
                User.ByID("test");
                Assert.Fail("User not deleted.");
            }
            catch(Exception) 
            {}
        }
    }
}
