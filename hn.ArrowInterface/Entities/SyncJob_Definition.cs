using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hn.ArrowInterface.Entities
{
    [Table("LH_SYNCJOBRECORD")]
    public class SyncJob_Definition
    {
        /// <summary>
        /// 业务类名称
        /// </summary>
        [Column("JOBNAME")]
        [Key]
        public string JobClassName { get; set; }
        
        /// <summary>
        /// 最后执行时间
        /// </summary>
        [Column("LASTSYNCTIME")]
        public DateTime? LastExecute { get; set; }


        public string ParsJson { get; set; }
    }
}