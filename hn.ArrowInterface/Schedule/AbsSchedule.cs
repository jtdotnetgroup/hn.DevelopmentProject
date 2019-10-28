using System;
using System.Linq;
using System.Threading.Tasks;
using hn.ArrowInterface.Entities;
using hn.AutoSyncLib.Common;
using Quartz;
using Quartz.Impl;

namespace hn.ArrowInterface.Schedule
{
    public abstract class AbsSchedule<T> 
    {

        protected OracleDBHelper Helper { get; set; }

        public  async Task DoWork()
        {
            

        }

        
    }
}