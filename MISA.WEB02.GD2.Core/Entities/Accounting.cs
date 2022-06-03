using MISA.WEB02.GD2.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.GD2.Core.Entities
{
    /// <summary>
    /// Chi tiết từng hạch toán trong phiếu chi
    /// </summary>
    public class Accounting
    {

        [PrimaryKey]
        public Guid AccountingId { get; set; }
        //
        public Guid? AccountObjectId { get; set; }
        public Guid PaymentId { get; set; }
        public string? AccountingDetail { get; set; }
        public string? DebitAccountId { get; set; }
        public string? CreditAccountId { get; set; }
        public float? CashAmount { get; set; }
    }
}
