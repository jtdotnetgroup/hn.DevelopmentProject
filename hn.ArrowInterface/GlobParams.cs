using System.Configuration;

namespace hn.ArrowInterface
{
    public class GlobParams
    {
        //登录获取TOKEN
        public static string ApiLogin = ConfigurationManager.AppSettings["ApiLogin"];
        //	库存下载接口
        public static string QueryLHInventoryPageURL = ConfigurationManager.AppSettings["QueryLHInventoryPage"];
        // 华耐日销出库下载
        public static string Inventory_BatchInsertURL = ConfigurationManager.AppSettings["Inventory_BatchInsert"];
        // 库存结存数据下载
        public static string ObOrderDay_BatchInsertURL = ConfigurationManager.AppSettings["ObOrderDay_BatchInsert"];
        //五、	可下单物料信息
        public static string Item_QueryProdPage = ConfigurationManager.AppSettings["Item_QueryProdPage"];
        //六、	折扣政策下载
        public static string QueryPolicyList = ConfigurationManager.AppSettings["QueryPolicyList"];
        //七、	销售订单上传
        public static string SaleSaleUpload = ConfigurationManager.AppSettings["SaleSaleUpload"];
        //审核状态回传
        public static string QueryAcctOAStatus = ConfigurationManager.AppSettings["QueryAcctOAStatus"];
        //九、	定制订单下载
        public static string QueryCustomOrderPage = ConfigurationManager.AppSettings["QueryCustomOrderPage"];
        //十、	物流部开单记录下载
        public static string QueryObPage = ConfigurationManager.AppSettings["QueryObPage"];
    }
}