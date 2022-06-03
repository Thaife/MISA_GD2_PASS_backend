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
    public class EmployeeService:BaseService<Employee>, IEmployeeService
    {
        IBaseRepository<Employee> _baseRepository;
        public EmployeeService(IBaseRepository<Employee> baseRepository) : base(baseRepository)
        {
            _baseRepository = baseRepository;
        }
    }
}
