using MISA.WEB02.GD2.Core.Entities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.GD2.Core.Interfaces.Service
{
    public interface IPaymentService:IBaseService<Payment>
    {
        public byte[] ExportService(int currentPage, int pageSize, string? filterText, List<TableInfo> columns);
    }
}
