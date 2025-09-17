using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMQuanLyHangTonKho.Models.DTO
{
    public class PriceListDTO
    {
        public string PriceId { get; set; }
        public string PriceName { get; set; }
        public string PriceType { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class PriceListDetailDTO
    {
        public string PriceListId { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public decimal FromQty { get; set; }
        public decimal ToQty { get; set; }
        public decimal UnitPrice { get; set; }
        public string Notes { get; set; }
    }
}
