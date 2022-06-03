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
    public class VendorsController : ControllerBase
    {
        #region fields
        IVendorRepository _vendorRepository;
        IVendorService _vendorService;
        #endregion
        #region constructor
        public VendorsController(IVendorRepository vendorRepository, IVendorService vendorService)
        {
            _vendorRepository = vendorRepository;
            _vendorService = vendorService;
        }

        #endregion
        /// <summary>
        /// Lấy mã nhà cung cấp mới
        /// </summary>
        /// <returns></returns>
        /// Create By: Thai(10/5/2022)
        [HttpGet("NewCode")]
        public IActionResult GetNewCode()
        {
            try
            {
                var newCode = _vendorRepository.GetNewCode();
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
        /// Lấy toàn bộ nhà cung cấp
        /// </summary>
        /// <returns></returns>
        /// Create By: Thai(10/5/2022)
        [HttpGet()]
        public IActionResult GetVendors()
        {
            try
            {
                var vendors = _vendorRepository.Get();
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
        /// Lấy danh sách nhà cung cấp phân trang
        /// </summary>
        /// <returns></returns>
        /// Create By: Thai(10/5/2022)
        [HttpGet("paging")]
        public IActionResult GetVendorsPaging(int pageSize, int pageNumber, string? textSearch)
        {
            try
            {
                var vendors = _vendorRepository.GetPaging(pageSize, pageNumber, textSearch);
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
        /// Lấy nhà cung cấp theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Create By: Thai(10/5/2022)
        [HttpGet("{id}")]
        public IActionResult GetVendorById(Guid id)
        {
            try
            {
                var accountings = _vendorRepository.GetVendorsById(id);
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

        /// <summary>
        /// Thêm mới nhà cung cấp
        /// </summary>
        /// <param name="vendor"></param>
        /// <returns></returns>
        [HttpPost()]
        public IActionResult InsertVendor(Vendor vendor)
        {
            try
            {
                var res = _vendorService.InsertVendorService(vendor);
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
        /// Chỉnh sửa nhà cung cấp
        /// </summary>
        /// <param name="vendor"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult UpdateVendor(Vendor vendor, Guid id)
        {
            try
            {
                var res = _vendorService.UpdateVendorService(vendor, id);
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
        /// Xoá nhà cung cấp theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteVendor(Guid id)
        {
            try
            {
                var res = _vendorRepository.Delete(id);
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
        public IActionResult DeleteMultiVendor(List<Guid> listIds)
        {
            try
            {
                var res = _vendorRepository.DeleteMulti(listIds);
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
                var res = _vendorRepository.GetCustomTable();
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
        public IActionResult UpdateCustomTable([FromBody]dynamic infor)
        {
            try
            {
                var res = _vendorRepository.UpdateCustomTable(infor.data.ToString());
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
    }
}
