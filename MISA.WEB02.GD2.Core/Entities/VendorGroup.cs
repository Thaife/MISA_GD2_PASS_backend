using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.GD2.Core.Entities
{
    public class VendorGroup : BaseEntity
    {
        /// <summary>
        /// Id nhóm nhà cung cấp
        /// </summary>
        public Guid VendorGroupId { get; set; }
        /// <summary>
        /// Mã nhóm nhà cung cấp
        /// </summary>
        public string VendorGroupCode { get; set; }
        /// <summary>
        /// Tên nhóm nhà cung cấp
        /// </summary>
        public string VendorGroupName { get; set; }


    }
}
