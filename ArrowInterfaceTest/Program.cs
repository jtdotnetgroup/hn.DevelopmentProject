using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hn.ArrowInterface;

namespace ArrowInterfaceTest
{
    class Program
    {
        static ArrowInterface iInterface = new ArrowInterface();
        static void Main(string[] args)
        {
            TestQueryProdPage();
            
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

    }
}
