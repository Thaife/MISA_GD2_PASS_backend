using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.GD2.Core.Entities
{
    public class Bank
    {
        /// <summary>
        /// id tài khoản ngân hàng
        /// </summary>
        public Guid BankId { get; set; }
        /// <summary>
        /// số tài khoản
        /// </summary>
        public string? BankAccount { get; set; }
        /// <summary>
        /// tên tài khoản
        /// </summary>
        public string? BankName { get; set; }

        /// <summary>
        /// chi nhánh ngân hàng
        /// </summary>
        public string? BankBranch { get; set; }

        /// <summary>
        /// tỉnh/thành phố của ngân hàng
        /// </summary>
        public string? CityOfBank { get; set; }

    }
}
