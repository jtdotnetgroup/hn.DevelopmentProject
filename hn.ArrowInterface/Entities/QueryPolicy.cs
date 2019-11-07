﻿using System.ComponentModel.DataAnnotations.Schema;

namespace hn.ArrowInterface.Entities
{
    [Table("LH_Policy")]
    public class QueryPolicy
    {
        [Column("HeadId")]
        public string Id { get; set; }
        public string PolicyName { get; set; }
        public string OrderType { get; set; }
        public string OrderSubType { get; set; }
        public string ProdChannel { get; set; }
        public string DeptName { get; set; }
        public string ItemId { get; set; }
        public string PolicyItemType { get; set; }
        public string ProdCode { get; set; }
        public decimal MinimumQuantity { get; set; }
        public decimal CappingQUantity { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal SpecialOffer { get; set; }


    }
}