using MISA.WEB02.GD2.Core.Entities;
using MISA.WEB02.GD2.Core.Interfaces.Infrastructure;
using MISA.WEB02.GD2.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.GD2.Core.Service
{
    public class VendorService: BaseService<Vendor>, IVendorService
    {
        IBaseRepository<Vendor> _baseRepository;
        IVendorRepository _vendorRepository;
        IVendorGroupAssistantRepository _vendorGroupAssistantRepository;
        public VendorService(IBaseRepository<Vendor> _baseRepository, IVendorRepository vendorRepository, IVendorGroupAssistantRepository vendorGroupAssistantRepository) : base(_baseRepository)
        {
            _vendorRepository = vendorRepository;
            _vendorGroupAssistantRepository = vendorGroupAssistantRepository;
        }


        public int InsertVendorService(Vendor vendor)
        {

            ValidateObject(vendor);
            
            var res = _vendorRepository.InsertVendor(vendor);
            return res;
        }

        public int InsertMultiVendorGroupsAssistant(List<Guid>? idVendorGroups, Guid id)
        {
            return _vendorGroupAssistantRepository.InsertMultiVendorGroupsAssistant(idVendorGroups, id);
        }
        
        public int UpdateVendorService(Vendor vendor, Guid vendorId)
        {
            var f = _vendorGroupAssistantRepository.DeleteMultiVendorGroupsAssistantByVendorId(vendorId);
            var s = _vendorRepository.Update(vendorId, vendor);
            var l = 0;
            if (s > 0 && vendor.VendorGroups != null)
            {
                List<Guid> idVendorGroups = new List<Guid>(vendor.VendorGroups);
                l = InsertMultiVendorGroupsAssistant(idVendorGroups, vendorId);
            }
            return f + s + l;
        }

        //#region field
        //IUnitRepository _unitRepository;


        //IBaseRepository<Unit> _baseRepository;
        //List<Object> errLstMsgs = new List<Object>();

        //#endregion
        //#endregion
    }
}
