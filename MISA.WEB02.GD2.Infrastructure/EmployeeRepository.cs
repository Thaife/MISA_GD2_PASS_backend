using Microsoft.Extensions.Configuration;
using MISA.WEB02.GD2.Core.Entities;
using MISA.WEB02.GD2.Core.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.GD2.Infrastructure
{
    public class EmployeeRepository:BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {

        }
    }
}
