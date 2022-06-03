using MISA.WEB02.GD2.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.GD2.Core.Entities
{
    public class Payment : BaseEntity
    {
        /// <summary>
        /// Id phiếu chi
        /// </summary>
        [PrimaryKey]
        public Guid? PaymentId { get; set; }
        /// <summary>
        /// Mã phiếu chi
        /// </summary>
        [NotEmpty]
        [NotDuplicate]
        [AllowFilter]
        public string? PaymentCode { get; set; }
        /// <summary>
        /// id đối tượng
        /// </summary>
        public Guid? AccountObjectId { get; set; }
        /// <summary>
        /// mã đối tượng
        /// </summary>
        [NotMap]
        [AllowFilter]
        public string? AccountObjectCode { get; set; }
        /// <summary>
        /// tên đối tượng
        /// </summary>
        [AllowFilter]
        public string? AccountObjectName { get; set; }

        /// <summary>
        /// Người nhận
        /// </summary>
        public string? ReceiverName { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [AllowFilter]
        public string? Address { get; set; }
        /// <summary>
        /// Ngày hạch toán
        /// </summary>
        [Date]
        //[AllowFilter]
        public DateTime? AccountingDate { get; set; }
        /// <summary>
        /// Ngày phiếu chi
        /// </summary>
        [Date]
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Ngày chứng từ
        /// </summary>
        [Date]
        //[AllowFilter]
        public DateTime? DateOfVouchers { get; set; }

        /// <summary>
        /// Lý do chi
        /// </summary>
        [AllowFilter]
        public string? PaymentReason { get; set; }

        /// <summary>
        /// mã nhân viên
        /// </summary>
        public Guid? EmployeeId { get; set; }

        /// <summary>
        /// kèm theo (số lượng)
        /// </summary>
        public int? AttachDocumentAmount { get; set; }

        /// <summary>
        /// Tổng số tiền của 1 phiếu chi
        /// </summary>
        [NotMap]
        public float? Cash { get; set; }

        /// <summary>
        /// tiền tệ
        /// </summary>
        public string? CurrencyId { get; set; }
        public float? ExchangeRate { get; set; }

        [Date]
        public DateTime? DateOfFunc { get; set; }

        //Danh sách hạch toán (JSON)
        public string? Accountings { get; set; }

    }
}
