using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.WEB02.GD2.Core.Entities;
using MISA.WEB02.GD2.Core.Interfaces.Infrastructure;
using MISA.WEB02.GD2.Core.Interfaces.Service;

namespace MISA.WEB02.GD2.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        #region fields
        IEmployeeRepository _employeeRepository;
        IEmployeeService _employeeService;
        #endregion
        #region constructor
        public EmployeesController(IEmployeeRepository paymentRepository, IEmployeeService employeeService)
        {
            _employeeRepository = paymentRepository;
            _employeeService = employeeService;
        }

        #endregion

        #region method
        /// <summary>
        /// Lấy toàn bộ danh danh sách nhân viên
        /// </summary>
        /// <returns></returns>
        /// Create By: Thai(10/5/2022)
        [HttpGet()]
        public IActionResult Get()
        {
            try
            {
                var payments = _employeeRepository.Get();
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
        /// Thêm nhân viên
        /// </summary>
        /// <returns></returns>
        /// Create By: Thai(10/5/2022)
        [HttpPost()]
        public IActionResult InsertPayment(Employee employee)
        {
            try
            {
                var res = _employeeService.InsertService(employee);
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

        #endregion

    }
}
