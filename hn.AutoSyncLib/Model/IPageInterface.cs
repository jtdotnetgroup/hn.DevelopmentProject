namespace hn.AutoSyncLib.Model
{
    public interface IPageInterface
    {
        int pageIndex { get; set; }
        int pageSize { get; set; }
    }
}