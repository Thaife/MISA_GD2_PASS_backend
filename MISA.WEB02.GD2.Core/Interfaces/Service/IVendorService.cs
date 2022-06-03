using MISA.WEB02.GD2.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.GD2.Core.Interfaces.Service
{
    public interface IVendorService : IBaseService<Vendor>
    {
        ///// <summary>
        ///// Xử lý nghiệp vụ thêm record
        ///// Created by: Thai(19/4/2022)
        ///// </summary>
        ///// <param name="employee"></param>
        ///// <returns></returns>
        public int InsertVendorService(Vendor vendor);

        ///// <summary>
        ///// Xử lý nghiệp vụ sửa dữ liệu
        ///// Created by: Thai(19/4/2022)
        ///// </summary>
        ///// <param name="employee"></param>
        ///// <param name="employeeId"></param>
        ///// <returns></returns>
        public int UpdateVendorService(Vendor vendor, Guid id);

        ///// <summary>
        ///// Xử lý nghiệp vụ xóa record qua Id
        ///// Created by: Thai(19/4/2022)
        ///// </summary>
        ///// <param name="employeeId"></param>
        ///// <returns></returns>
        //public int DeleteEmployeeById(Guid employeeId);

        ///// <summary>
        ///// Xử lý nghiệp vụ xóa nhiều record qua nhiều Id
        ///// Created by: Thai(19/4/2022)
        ///// </summary>
        ///// <param name="listEmployeeId"></param>
        ///// <returns></returns>
        //public int DeleteMultiEmployeeByIds(List<Guid> listEmployeeId);

        ///// <summary>
        ///// Xuất dữ liệu ra file Excel
        ///// Created by: Thai(19/4/2022)
        ///// </summary>
        ///// <param name=""></param>
        ///// <returns></returns>
        //public Stream ExportExcel();
    }
}
