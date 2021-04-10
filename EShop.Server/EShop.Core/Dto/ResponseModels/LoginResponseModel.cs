using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core.Dto.ResponseModels
{
    public class LoginResponseModel
    {
        public string Token { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set; }
        public List<string> Permissions { get; set; }
        public UserResponseModel UserProfile { get; set; }
    }
}
