using System;
using writely.Models.Dto;
using Xunit;

namespace writely.tests
{
    public class UnitTest1
    {
        [Fact]
        public void Register_UniqueUser_Successful()
        {
        }

        [Fact]
        public void Register_PasswordMismatch_Fail()
        { 
        }

        [Fact]
        public void Register_UserNotUnique_Fail()
        { 
        }

        [Fact]
        public void DeleteAccount_UserDeleted_Successful()
        { 
        }

        [Fact]
        public void DeleteAccount_UserNotFound_Fail()
        { 
        }

        [Fact]
        public void DisableAccount_UserFound_Disable_Successful()
        { 
        }

        [Fact]
        public void DisableAccount_UserNotFound_Fail()
        { 
        }

        [Fact]
        public void GetUserData_UserFound_DataReturned_Success()
        { 
        }

        [Fact]
        public void GetUserData_UserNotFound_Fail()
        { 
        }
    }
}
