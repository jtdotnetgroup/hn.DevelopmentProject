using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using hn.ArrowInterface.Schedule;
using hn.Common;

namespace hn.Arrow_SyncUI
{
    public partial class FrmMain : Form
    {
        private bool isRunning { get; set; }

        public FrmMain()
        {
            InitializeComponent();
            LogHelper.Init(new TextBoxWriter(txtLog));
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

        }

        private void btnRunOrEnd_Click(object sender, EventArgs e)
        {

            if (isRunning)
            {
                Environment.Exit(0);
            }

            Task task=new Task(() =>
            {
                SyncSchedule schedule=new  SyncSchedule();
                schedule.DoWork();
            });

            task.Start();

            btnRunOrEnd.Text = "结束";
            this.isRunning = true;


        }

        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.notifyIcon1.Visible = true;
                this.ShowInTaskbar = false;
                this.Visible = false;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = false;
            this.ShowInTaskbar = true;
            this.Visible = true;
            this.Show();
            this.Activate();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
