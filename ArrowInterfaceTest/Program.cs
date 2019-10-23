using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hn.ArrowInterface;
using hn.ArrowInterface.Entities;
using hn.AutoSyncLib.Common;

namespace ArrowInterfaceTest
{
    class Program
    {
        static ArrowInterface iInterface = new ArrowInterface();
        static void Main(string[] args)
        {
            TestDelete();
            
        }

        static void TestLogin()
        {
            iInterface.GetToken();
        }

        static void TestQueryLHInventoryPage()
        {
            var token = iInterface.GetToken();
            var result= iInterface.QueryLHInventoryPage(token.Token);
            Console.WriteLine("ok");
        }

        static void TestQueryProdPage()
        {
            var token = iInterface.GetToken();
            var result = iInterface.QueryProdPage(token.Token);
            Console.WriteLine("ok");          
        }

        static void TestQueryPolicy()
        {
            var token = iInterface.GetToken();
            var result = iInterface.QueryPolicy(token.Token);

            Console.WriteLine("OK");
        }

        static void TestDelete()
        {
            OracleDBHelper helper = new OracleDBHelper("");
            AuthorizationToken token=new AuthorizationToken();

            helper.Delete<AuthorizationToken>("");

        }

    }
}
