using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core.Constants
{
    public static class MessageConstants
    {
        public const string UsernamePasswordDoNotMatch = "Username/Password do not match.";
        public const string UserNotFound = "No User Found.";
        public const string CategoryAlreadyExist = "Category name already exist.";
        public const string LoginSuccess = "Successfully logged in";
    }
}
