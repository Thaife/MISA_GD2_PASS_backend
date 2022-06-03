using MISA.WEB02.GD2.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.GD2.Core.Interfaces.Infrastructure
{
    public interface IVendorGroupAssistantRepository:IBaseRepository<VendorGroupAssistant>
    {
        public int InsertMultiVendorGroupsAssistant(List<Guid> listIds, Guid vendorId);
        public int DeleteMultiVendorGroupsAssistantByVendorId(Guid vendorId);
    }
}
