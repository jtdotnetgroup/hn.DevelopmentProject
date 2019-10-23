using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace hn.ArrowInterface.Entities
{
    [Table("LH_ArrowAuthorizationToken")]
    public class AuthorizationToken
    {
            
            public  DateTime ExpireTime { get; set; }

            public String Token { get; set; }

    }

    public class AutorizationTokenDTO:AbsRequestResult
    {
        public String Token { get; set; }
    }
}