using MISA.WEB02.GD2.Core.Entities;
using MISA.WEB02.GD2.Core.Interfaces.Infrastructure;
using MISA.WEB02.GD2.Core.Interfaces.Service;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace MISA.WEB02.GD2.Core.Service
{
    public class PaymentService:BaseService<Payment>, IPaymentService
    {
        IBaseRepository<Payment> _baseRepository;
        IPaymentRepository _paymentRepository;
        public PaymentService(IBaseRepository<Payment> baseRepository, IPaymentRepository paymentRepository):base(baseRepository) { 
            _baseRepository = baseRepository;
            _paymentRepository = paymentRepository;
        }

        public byte[] ExportService(int currentPage, int pageSize, string? filterText, List<TableInfo> columns)
        {
            dynamic res = _paymentRepository.GePaymentPaging(pageSize, currentPage, filterText);
            List<object>data = res.GetType().GetProperty("data").GetValue(res, null);
            var json = JsonConvert.SerializeObject(data);
            List<Payment> list = JsonConvert.DeserializeObject<List<Payment>>(json);
            //gọi hàm xuất dữ liệu
            var result = Export(list, columns);
            return result;
        }

        /// <summary>
        /// xuất dữ liệu file ex cel theo tên cột và dan gsacsh dữ liệu truyền vào
        /// </summary>
        /// <typeparam name="T">class tương ứng với dữ liệu</typeparam>
        /// <param name="data">danh sách dữ liệu</param>
        /// <param name="columns">thông tin các cột cần xuất</param>
        /// <returns></returns>
        public byte[] Export(List<Payment> data, List<TableInfo> columns)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var excelPackage = new ExcelPackage(new FileInfo("C:\\Users\\Admin\\Desktop\\EX.xlsx")))
            {
                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = "Danh sách";
                //thêm 1 sheet để làm việc với tệp excel
                excelPackage.Workbook.Worksheets.Add("Danh sách");
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                int row = 1;
                int col = 1;
                BuildHeader(columns, workSheet, ref row, col);
                row += 1;
                BuildData(columns, workSheet, data, ref row, col);
                col = columns.Count;
                //tự động dãn cột
                workSheet.Cells.AutoFitColumns();
                //return file
                var file = excelPackage.GetAsByteArray();
                //dừng ile excel
                excelPackage.Dispose();
                return file;
            }
        }
        /// <summary>
        /// build header cho worksheet
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="workSheet"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void BuildHeader(List<TableInfo> columns, ExcelWorksheet workSheet, ref int row, int col)
        {
            //header cho bảng
            foreach (var column in columns)
            {
                if (column.Align == "center")
                {
                    workSheet.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;//  căn giữa
                }
                if (column.Align == "right")
                {
                    workSheet.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;//  căn phải
                }
                if (column.Align == "left")
                {
                    workSheet.Column(col).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;//  căn trái
                }
                workSheet.Cells[row, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[row, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[row, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[row, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[row, col].Value = column.Name;
                workSheet.Cells[row, col].Style.Font.Bold = true;//In đậm
                workSheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#BBB"));//background-color
                col++;
            }
            workSheet.Row(row).Height = 20;
        }
        /// <summary>
        /// build data cho worksheet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <param name="workSheet"></param>
        /// <param name="data"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private void BuildData(List<TableInfo> columns, ExcelWorksheet workSheet, List<Payment> data, ref int row, int col)
        {
            //header cho bảng
            foreach (var item in data)
            {
                foreach (var column in columns)
                {
                    workSheet.Cells[row, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[row, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[row, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[row, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    var prop = item.GetType().GetProperty(column.Key);
                    if (prop == null) continue;
                    var propName = prop.Name;
                    var valueProp = prop.GetValue(item);
                    if(propName == "Cash")
                    {
                        //float money = _paymentRepository.GetTotalMoney(Guid.Parse("d4aa7aa4-c7fe-439f-b8ac-6082874f16ae"));
                        workSheet.Cells[row, col].Style.Numberformat.Format = "#,##0.00";
                        workSheet.Cells[row, col].Value = "";
                    }
                    else if (valueProp == null)
                    {
                        workSheet.Cells[row, col].Value = "";
                    }
                    else
                    {
                        if (valueProp.GetType() == typeof(DateTime))
                        {
                            workSheet.Cells[row, col].Value = ((DateTime)valueProp).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            workSheet.Cells[row, col].Value = valueProp;
                        }
                        
                    }

                    col++;
                }
                row++;
                col = 1;
            }
        }



    }
}
