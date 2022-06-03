
using MISA.WEB02.GD2.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.GD2.Core.Entities
{
    /// <summary>
    /// Thông tin cá nhân của nhà cung câ
    /// Created by: Thai(13/1/2022)
    public class Vendor : BaseEntity
    {
        /// <summary>
        /// Id nhà cung cấp
        /// </summary>
        [PrimaryKey]
        public Guid VendorId { get; set; }


        /// <summary>
        /// Mã nhà cung cấp
        /// </summary>
        [NotEmpty]
        [NotDuplicate]
        [AllowFilter]
        public string VendorCode { get; set; }

        /// <summary>
        /// Tên nhà cung cấp
        /// </summary>
        [NotEmpty]
        [AllowFilter]
        public string? VendorFullName { get; set; }

        /// <summary>
        /// Mã số thuế
        /// </summary>
        [AllowFilter]
        public string? VendorTaxCode { get; set; }
        /// <summary>
        /// Số điện thoại nhà cung cấp
        /// </summary>
        [AllowFilter]
        public string? VendorPhoneNumber { get; set; }
        /// <summary>
        /// địa chỉ liên hệ
        /// </summary>s
        [AllowFilter]
        public string? ContactAddress { get; set; }

        /// <summary>
        /// Nhân viên mua hàng
        /// </summary>
        public string? PurchasingStaffId { get; set; }
        public string? Website { get; set; }
        /// <summary>
        /// Loại nhà cung cấp: 0 - tổ chức; 1 - cá nhân
        /// </summary>
        public int VendorType { get; set; }
        /// <summary>
        /// Là khách hàng ?
        /// </summary>
        public bool IsCustomer { get; set; }

        /// <summary>
        /// Xưng hô ( người liên hệ)
        /// </summary>
        public string? ContactPronounId { get; set; }
        public string? ContactName { get; set; }
        /// <summary>
        /// Đại diện pháp luật
        /// </summary>
        public string? ContactLegalRep { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhoneNumber { get; set; }

        /// <summary>
        /// Người nhận chứng hoá điện tử
        /// </summary>
        /// Tên người nhận
        public string? EinvoiceRecipient { get; set; }
        /// <summary>
        /// Danh sách email (string)
        /// </summary>
        public string? EinvoiceRecipientEmails { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string? EinvoiceRecipientPhoneNumber { get; set; }
        /// <summary>
        /// Căn Cước
        /// </summary>
        [AllowFilter]
        public string? VendorIdentityNumber { get; set; }
        /// <summary>
        /// Ngày cấp
        /// </summary>
        public DateTime? DateOfIssue { get; set; }
        /// <summary>
        /// Nơi cấp
        /// </summary>
        public string? AddressOfIssue { get; set; }


        #region Điều khoản  
        public string? ContractId { get; set; }

        /// <summary>
        /// Số ngày nợ tối đa
        /// </summary>
        public int? MaxDebitDateCount { get; set; }

        /// <summary>
        /// Số nợ tối đa
        /// </summary>
        public float? MaxDebitAmount { get; set; }

        /// <summary>
        /// Tài khoản công nợ nhận
        /// </summary>
        public string? DebitReceiptAccountId { get; set; }


        /// <summary>
        /// Tài khoản công nợ trả
        /// </summary>
        public string? DebitPaymentAccountId { get; set; }

        #endregion

        /// <summary>
        /// list tài khoản ngân hàng
        /// </summary>
        #region Tài khoản ngân hàng ( JSON)
        public string? BankAccounts { get; set; }
        #endregion

        #region Địa chỉ khác
        public string? CountryId { get; set; }
        public string? ProvinceId { get; set; }
        public string? DistrictId { get; set; }
        public string? WardId { get; set; }

        /// <summary>
        /// Địa chỉ giao hàng (JSON)
        /// </summary>
        public string? DeliveryAddresses { get; set; }
        #endregion

        #region Ghi chú
        public string? TextNote { get; set; }
        #endregion

        #region Cá nhân (thêm)
        /// <summary>
        /// Điện thoại cố định
        /// </summary>
        public string? LandlineNumber { get; set; }
        public DateTime? IdentityDate { get; set; }
        public string? IdentityPlace { get; set;}

        [NotMap]
        public List<Guid>?VendorGroups { get; set; }

        #endregion

    }
}

