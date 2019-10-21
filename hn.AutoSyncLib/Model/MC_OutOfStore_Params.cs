namespace hn.AutoSyncLib.Model
{
    public class MC_OutOfStore_Params:MC_Request_BaseParams,CommonRequestParams
    {
        public MC_OutOfStore_Params( string token,string rq1,string rq2,int pageSize,int pageIndex=1, string action = "getMN_cp_14" ) : base(action)
        {
            base.token = token;
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
            this.rq1 = rq1;
            this.rq2 = rq2;
        }


        public string rq1 { get; set; }
        public string rq2 { get; set; }
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
    }
}