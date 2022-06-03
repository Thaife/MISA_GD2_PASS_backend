using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.WEB02.GD2.Core.Entities;
using MISA.WEB02.GD2.Core.Interfaces.Infrastructure;
using MISA.WEB02.GD2.Core.Interfaces.Service;

namespace MISA.WEB02.GD2.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountingsController : ControllerBase
    {
        #region fields
        IAccountingRepository _accountingRepository;
        IAccountingService _accountingService;
        #endregion
        #region constructor
        public AccountingsController(IAccountingRepository accountingRepository, IAccountingService accountingService) { 
            _accountingRepository = accountingRepository;
            _accountingService = accountingService;
        }

        #endregion

        #region method

        /// <summary>
        /// Lấy toàn bộ danh danh sách hạch toán (chi tiết phiếu chi)
        /// </summary>
        /// <returns></returns>
        /// Create By: Thai(10/5/2022)
        [HttpGet()]
        public IActionResult GetAccountings()
        {
            try
            {
                var accountings = _accountingRepository.Get();
                return Ok(accountings);
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
        [HttpPost()]
        public IActionResult InsertAccounting(Accounting accounting)
        {
            try
            {
                var res = _accountingService.InsertService(accounting);   
                if(res >= 1)
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
