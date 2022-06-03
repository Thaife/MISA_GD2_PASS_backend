using MISA.WEB02.GD2.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.GD2.Core.Interfaces.Infrastructure
{
    public interface IPaymentRepository :IBaseRepository<Payment>
    {
        /// <summary>
        /// Lấy toàn bộ phiếu chi
        /// </summary>
        /// <returns></returns>
        /// Created by: Thai(10/5/2022)
        public IEnumerable<Payment> GetPayments();

        /// <summary>
        /// thêm phiếu chi
        /// </summary>
        /// <returns></returns>
        /// Created by: Thai(10/5/2022)
        public int InsertPayment();

        /// <summary>
        /// Lấy danh sách phiếu chi phân trang
        /// </summary>
        /// <returns></returns>
        /// Created by: Thai(10/5/2022)
        public object GePaymentPaging(int pageSize,int pageNumber, string? textSearch);

        public float GetTotalMoney(Guid paymentId);

        /// <summary>
        /// Lấy thông tin giao diện của grid
        /// </summary>
        /// <returns></returns>
        /// Created by: Thai(10/5/2022)
        public string GetCustomTable();

        /// <summary>
        /// Lấy thông tin giao diện của grid
        /// </summary>
        /// <returns></returns>
        /// Created by: Thai(10/5/2022)
        public int UpdateCustomTable(string infor);
    }
}
