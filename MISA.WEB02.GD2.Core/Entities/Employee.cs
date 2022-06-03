
using MISA.WEB02.GD2.Core.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.GD2.Core.Entities
{
    public class Employee
    {
        /// <summary>
        /// Id nhân viên
        /// </summary>
        [PrimaryKey]
        public Guid EmployeeId { get; set; }


        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [NotDuplicate]
        [NotEmpty]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        [NotEmpty]
        public string EmployeeFullName { get; set; }


        /// <summary>
        /// Đơn vị nhân viên
        /// </summary>
        [NotMap]
        public string EmployeePosition {get; set;}

        /// <summary>
        /// Số điện thoại nhân viên
        /// </summary>
        [PhoneNumber]
        public string? EmployeePhoneNumber { get; set; }

    }
}
