using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.WEB02.GD2.Core.Entities;
using MISA.WEB02.GD2.Core.Exceptions;
using MISA.WEB02.GD2.Core.Interfaces.Infrastructure;
using MISA.WEB02.GD2.Core.Interfaces.Service;

namespace MISA.WEB02.GD2.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        #region fields
        IPaymentRepository _paymentRepository;
        IPaymentService _paymentService;
        #endregion
        #region constructor
        public PaymentsController(IPaymentRepository paymentRepository, IPaymentService paymentService) {
            _paymentRepository = paymentRepository;
            _paymentService = paymentService;
        }

        #endregion

        #region method
        /// <summary>
        /// Lấy mã phiếu chi mới
        /// </summary>
        /// <returns></returns>
        /// Create By: Thai(10/5/2022)
        [HttpGet("NewCode")]
        public IActionResult GetNewCode()
        {
            try
            {
                var newCode = _paymentRepository.GetNewCode();
                return Ok(newCode);
            }
            catch (Exception ex)
            {
                var mess = new
                {
                    devMsg = ex.Message,
                    userMsg = Core.Properties.Resources.ExceptionMISA
                };
                return StatusCode(500, mess);
            }
        }

        /// <summary>
        /// Lấy danh sách phiếu chi phân trang
        /// </summary>
        /// <returns></returns>
        /// Create By: Thai(10/5/2022)
        [HttpGet("paging")]
        public IActionResult GetPaymentsPaging(int pageSize, int pageNumber, string? textSearch)
        {
            try
            {
                var vendors = _paymentRepository.GePaymentPaging(pageSize, pageNumber, textSearch);
                return Ok(vendors);
            }
            catch (Exception ex)
            {
                var mess = new
                {
                    devMsg = ex.Message,
                    userMsg = Core.Properties.Resources.ExceptionMISA
                };
                return StatusCode(500, mess);
            }
        }
        /// <summary>
        /// Lấy toàn bộ phiếu chi
        /// </summary>
        /// <returns></returns>
        /// Create By: Thai(10/5/2022)
        [HttpGet()]
        public IActionResult GetPayments()
        {
            try
            {
                var payments = _paymentRepository.GetPayments();
                return Ok(payments);
            }
            catch (Exception ex)
            {
                var mess = new
                {
                    devMsg = ex.Message,
                    userMsg = Core.Properties.Resources.ExceptionMISA
                };
                return StatusCode(500, mess);
            }
        }

        /// <summary>
        /// Lấy phiếu chi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// /// Create By: Thai(10/5/2022)
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var payment = _paymentRepository.Get(id);
                return Ok(payment);
            }
            catch (Exception ex)
            {
                var mess = new
                {
                    devMsg = ex.Message,
                    userMsg = Core.Properties.Resources.ExceptionMISA
                };
                return StatusCode(500, mess);
            }
        }

        /// <summary>
        /// thêm mới phiếu chi
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        /// /// Create By: Thai(10/5/2022)
        [HttpPost()]
        public IActionResult InsertPayment(Payment payment)
        {
            try
            {
                var res = _paymentService.InsertService(payment);
                if (res >= 1)
                {
                    return StatusCode(201, res);
                }
                return Ok(res);
            }
            catch (MISAValidateException ex)
            {

                return StatusCode(400, ex.Data);
            }
            catch (Exception ex)
            {
                var mess = new
                {
                    devMsg = ex.Message,
                    userMsg = Core.Properties.Resources.ExceptionMISA
                };
                return StatusCode(500, mess);
            }
        }

        /// <summary>
        /// Sửa phiếu chi
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// /// Create By: Thai(10/5/2022)
        [HttpPut("{id}")]
        public IActionResult UpdatePayment(Payment payment, Guid id)
        {
            try
            {
                var res = _paymentService.UpdateService(payment, id);
                if (res >= 1)
                {
                    return StatusCode(201, res);
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                var mess = new
                {
                    devMsg = ex.Message,
                    userMsg = Core.Properties.Resources.ExceptionMISA
                };
                return StatusCode(500, mess);
            }
        }

        /// <summary>
        /// Xoá 1 phiếu chi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Create By: Thai(10/5/2022)
        [HttpDelete("{id}")]
        public IActionResult DeletePayment(Guid id)
        {
            try
            {
                var res = _paymentRepository.Delete(id);
                if (res >= 1)
                {
                    return StatusCode(200, res);
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                var mess = new
                {
                    devMsg = ex.Message,
                    userMsg = Core.Properties.Resources.ExceptionMISA
                };
                return StatusCode(500, mess);
            }
        }

        /// <summary>
        /// Xoá nhiều
        /// </summary>
        /// <param name="listIds"></param>
        /// <returns></returns>
        [HttpPut("multi")]
        public IActionResult DeleteMultiPayment(List<Guid> listIds)
        {
            try
            {
                var res = _paymentRepository.DeleteMulti(listIds);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var mess = new
                {
                    devMsg = ex.Message,
                    userMsg = Core.Properties.Resources.ExceptionMISA
                };
                return StatusCode(500, mess);
            }
        }


        /// <summary>
        /// Lấy thông tin hiển thị trên grid
        /// </summary>
        /// <returns></returns>
        /// Create By: Thai(10/5/2022)
        [HttpGet("table/infor")]
        public IActionResult GetCustomTable()
        {
            try
            {
                var res = _paymentRepository.GetCustomTable();
                return Ok(res);
            }
            catch (Exception ex)
            {
                var mess = new
                {
                    devMsg = ex.Message,
                    userMsg = Core.Properties.Resources.ExceptionMISA
                };
                return StatusCode(500, mess);
            }
        }


        /// <summary>
        /// chỉnh sửa thông tin hiển thị trên grid
        /// </summary>
        /// <returns></returns>
        /// Create By: Thai(10/5/2022)
        [HttpPut("table/infor")]
        public IActionResult UpdateCustomTable(dynamic infor)
        {
            try
            {
                var res = _paymentRepository.UpdateCustomTable(infor.data.ToString());
                return Ok(res);
            }
            catch (Exception ex)
            {
                var mess = new
                {
                    devMsg = ex.Message,
                    userMsg = Core.Properties.Resources.ExceptionMISA
                };
                return StatusCode(500, mess);
            }
        }



        /// <summary>
        /// export EX
        /// </summary>
        /// <returns></returns>
        /// Create By: Thai(10/5/2022)
        [HttpPost("export")]
        public IActionResult ExportExcel(int pageSize, int pageNumber, string? textSearch, List<TableInfo> infor)
        {
            try
            {
                var res = _paymentService.ExportService(1, 1000, textSearch, infor);
                return File(res, "xlsx/xls", "myFile.xlsx");
            }
            catch (Exception ex)
            {
                var mess = new
                {
                    devMsg = ex.Message,
                    userMsg = Core.Properties.Resources.ExceptionMISA
                };
                return StatusCode(500, mess);
            }
        }
        #endregion
    }
}
