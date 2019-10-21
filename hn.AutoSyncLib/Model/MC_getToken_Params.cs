namespace hn.AutoSyncLib.Model
{
    public class MC_getToken_Params:MC_Request_BaseParams
    {
        public MC_getToken_Params(string openkey= "AF7E9E381F3D7A7F9AD11E0186306031", string action= "getToken") :base(action)
        {
            this.openkey = openkey;
        }

        public string openkey { get; set; }
    }
}