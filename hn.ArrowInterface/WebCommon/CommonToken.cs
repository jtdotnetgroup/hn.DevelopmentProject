using System;
using System.Configuration;
using System.Linq;
using hn.ArrowInterface.Entities;
using hn.Common;

namespace hn.ArrowInterface.WebCommon
{
    public static class CommonToken
    {
        //互
        static object lockobj =new object();
        static ArrowInterface Interface=new ArrowInterface();
        private static string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

        private static  OracleDBHelper Helper =new OracleDBHelper(conStr);

        public static AuthorizationToken  GetToken()
        {
            lock (lockobj)
            {
                //从数据库读取Token值
                var token = Helper.GetAll<AuthorizationToken>().FirstOrDefault();
                if (token != null)
                {
                    //判断TOKEN是否过期
                    if (token.ExpiredTime <= DateTime.Now)
                    {
                        //过期重新获取Token并更新数据库值
                        var newtoken = Interface.GetToken();
                        newtoken.ExpiredTime = DateTime.Now.AddHours(2);
                        //token.Token = newtoken.Token;
                        Helper.Update(newtoken, string.Format(" AND TokenValue='{0}'", token.Token));
                    }

                    return token;
                }
                //数据库不存在Token，获取并插入
                token = Interface.GetToken();
                token.ExpiredTime = DateTime.Now.AddHours(2);

                Helper.Insert(token);

                return token;
            }
        }
    }
}