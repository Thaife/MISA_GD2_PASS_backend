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
    public class AccountingService:BaseService<Accounting>, IAccountingService
    {
        #region field
        IAccountingRepository _accountingRepository;


        //IBaseRepository<Accounting> _baseRepository;
        List<Object> errLstMsgs = new List<Object>();

        #endregion

        #region constructor
        public AccountingService(
            IAccountingRepository _accountingRepository
            //IBaseRepository<Accounting> _baseRepository
        ) : base(_accountingRepository)
        {
            this._accountingRepository = _accountingRepository;
        }
        #endregion
    }
}
