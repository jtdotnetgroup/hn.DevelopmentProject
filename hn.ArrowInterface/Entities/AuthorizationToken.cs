using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hn.ArrowInterface.Entities
{
    /// <summary>
    /// 此类为写入数据库用，用于把请求到的Token保存
    /// </summary>
    [Table("LH_AuthorizationToken")]
    public class AuthorizationToken: AbsRequestResult
    {
            //有效时长为2小时
            public  DateTime ExpiredTime { get; set; }
            [Key]
            [Column("TokenValue")]
            public String Token { get; set; }

    }

    /// <summary>
    /// 网络请求返回结果
    /// </summary>
    public class AutorizationTokenDTO:AbsRequestResult
    {
        public String Token { get; set; }
    }
}