using System.Runtime.Serialization;

namespace hn.AutoSyncLib.Model
{
    public class MC_getToken_Result:MC_Request_BaseResult<TokenInfo>
    {
        public string token { get; set; }
        public int limit_time { get; set; }
        public TokenInfo tokenInfo { get; set; }

    }

    public class TokenInfo
    {
        public int limitTime { get; set; }
        public string starDate { get; set; }
        public string endDate { get; set; }
        public int residueNum { get; set; }
        public string token { get; set; }
    }
}