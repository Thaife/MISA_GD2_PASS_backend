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
    public class AccountingRepository : BaseRepository<Accounting>, IAccountingRepository
    {
        public AccountingRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
