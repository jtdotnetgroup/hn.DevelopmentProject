using System;
using System.Configuration;
using System.Windows.Forms;
using hn.AutoSyncLib.Common;
using hn.AutoSyncLib.Schedule;
using log4net;

namespace hn.MN_SyncUI
{
    public partial class FrmMain : Form
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(FrmMain));

        private MC_OutOfStore_SyncSchedule mcOutOfStore;
        private MC_PickUpGoods_SyncSchedule mcPickUpGoods;

        public FrmMain()
        {
            InitializeComponent();

            LogHelper.Init(new TextBoxWriter(txtLog));

            mcOutOfStore=new MC_OutOfStore_SyncSchedule();
            mcPickUpGoods=new MC_PickUpGoods_SyncSchedule();
            btnStop.Enabled = false;
            this.notifyIcon1.Text = this.Text;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var time = dateTimeRun.Value.ToString("HH:mm");

            var hour = Convert.ToInt32(time.Split(':')[0]);
            var min = Convert.ToInt32(time.Split(':')[1]);

            mcOutOfStore.Start(hour, min);
            mcPickUpGoods.Start(hour, min);

            btnStart.Enabled = false;
            btnStop.Enabled = true;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            var runtime = ConfigurationManager.AppSettings["runtime"];

            var timeValue = DateTime.Now.Date.ToString("yyyy-MM-dd ") + runtime;

            dateTimeRun.Value = DateTime.Parse(timeValue);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            mcOutOfStore.Stop();
            mcPickUpGoods.Stop();

            btnStop.Enabled = false;
            btnStart.Enabled = true;
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.WindowState =  FormWindowState.Normal;
            this.Visible = true;
            this.notifyIcon1.Visible = false;
            this.ShowInTaskbar = true;
        }

        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                this.notifyIcon1.Visible = true;
            }
        }
    }
}
