using MISA.WEB02.GD2.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.GD2.Core.Interfaces.Infrastructure
{
    public interface IVendorRepository:IBaseRepository<Vendor>
    {
        /// <summary>
        /// Lấy nhân viên theo id
        /// </summary>
        /// <returns></returns>
        /// Created by: Thai(10/5/2022)
        public object GetVendorsById(Guid id);

        /// <summary>
        /// Thêm nhà cung cấp
        /// </summary>
        /// <returns></returns>
        /// Created by: Thai(10/5/2022)
        public int InsertVendor(Vendor vendor);

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
