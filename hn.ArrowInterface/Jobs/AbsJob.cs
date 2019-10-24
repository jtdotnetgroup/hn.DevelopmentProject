using System;
using System.Configuration;
using System.Linq;
using hn.ArrowInterface.Entities;
using hn.AutoSyncLib.Common;

namespace hn.ArrowInterface.Jobs
{
    public abstract class AbsJob:ISyncJob
    {
        protected ArrowInterface Interface { get; set; }
        protected  OracleDBHelper Helper { get; set; }

        public AbsJob()
        {
            string conStr = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            Helper=new OracleDBHelper(conStr);

            Interface=new ArrowInterface();
        }

        public AuthorizationToken GetToken()
        {
            //从数据库读取Token值
            var token=Helper.GetAll<AuthorizationToken>().FirstOrDefault();
            if (token != null)
            {
                //判断TOKEN是否过期
                if (token.ExpiredTime <= DateTime.Now)
                {
                    //过期重新获取Token并更新数据库值
                    var newtoken = Interface.GetToken();
                    token.Token = newtoken.Token;
                    Helper.Update(token, string.Format(" AND TokenValue='{0}'",token.Token));
                }

                return token;
            }
            //数据库不存在Token，获取并插入
            token = Interface.GetToken();
            token.ExpiredTime = DateTime.Now.AddHours(2);

            Helper.Insert(token);

            return token;
        }

        public abstract bool Sync();
    }
}