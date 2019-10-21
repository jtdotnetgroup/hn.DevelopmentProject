namespace hn.AutoSyncLib.Model
{
    /// <summary>
    /// 提货单接口请求参数
    /// </summary>
    public class MC_PickUpGoods_Params:MC_Request_BaseParams,CommonRequestParams
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action">接口名称</param>
        /// <param name="comid">库ID</param>
        /// <param name="khhm">客户代码</param>
        /// <param name="rq1">开始日期</param>
        /// <param name="rq2">结束日期</param>
        /// <param name="pageSize">单页数据条数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pzhm">审批号</param>
        /// <param name="dhno">订货单号</param>
        public MC_PickUpGoods_Params(string token,string rq1,string rq2,
            int pageSize,int pageIndex, string action = "getMN_cp_13", string pzhm="",string dhno="")
            : base(action)
        {
            this.dhno = dhno;
            this.pzhm = pzhm;
            this.rq1 = rq1;
            this.rq2 = rq2;
            this.pageSize = pageSize;
            this.pageIndex = pageIndex;
            this.token = token;
        }

        public string pzhm { get; set; }
        public string dhno { get; set; }

        public string rq1 { get; set; }
        public string rq2 { get; set; }
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
    }
}