using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DataStructures
{
    public class TokenInfo
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public string ErrorMsg { get; set; }

        public TokenInfo()
        {
            Token = string.Empty;
            Success = false;
            ErrorMsg = string.Empty;
        }
    }
}
