using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMQuanLyHangTonKho.Models.DTO
{



    public class PIDTO
    {
        public string Id { get; set; }
        public string DocDate { get; set; }
        public string SupplierId { get; set; }
        public string PriceListId { get; set; }
        public string Notes { get; set; }
        public string TotalMoney { get; set; }
        public string TotalQty { get; set; }
        public string UserCreate { get; set; }
        public DateTime DateCreate { get; set; }
        public string UserUpdate { get; set; }
        public DateTime DateUpdate { get; set; }
    }

    public class PIDetailDTO
    {
        public string PIMasterId { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Qty
        {
            get;
            set;
        }
        public string UnitPrice { get; set; }
        public string LineAmount { get; set; }
        public string Notes { get; set; }
    }
}
