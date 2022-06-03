using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.WEB02.GD2.Core.Interfaces.Infrastructure;

namespace MISA.WEB02.GD2.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VendorGroupsController : ControllerBase
    {
        #region fields
        IVendorGroupRepository _vendorGroupRepository;
        #endregion
        #region constructor
        public VendorGroupsController(IVendorGroupRepository vendorGroupRepository)
        {
            _vendorGroupRepository = vendorGroupRepository;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public IActionResult GetVendorGroups()
        {
            try
            {
                var vendorGroups = _vendorGroupRepository.Get();
                return Ok(vendorGroups);
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
    }
}
