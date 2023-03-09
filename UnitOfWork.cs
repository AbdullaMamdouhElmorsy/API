using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.FXRatesEntities;
using Core.Entities.ICEntities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FXRATEContext _fxcontext;
        private readonly ICContext _iccontext;

        public IBaseRepository<ForeignExchangeTab> ForeignExchangeTabs { get; private set; }
        public IBaseRepository<IcPriceFTab> IcPriceFTabs { get; private set; }
        public UnitOfWork(FXRATEContext fxcontext , ICContext iccontext)
        {
            _fxcontext = fxcontext;
            _iccontext = iccontext;

            ForeignExchangeTabs = new BaseRepository<ForeignExchangeTab>(_fxcontext);
            IcPriceFTabs = new BaseRepository<IcPriceFTab>(_iccontext);
        }

        public int ICComplete()
        {
            return _iccontext.SaveChanges();
        }

        public void ICDispose()
        {
            _iccontext.Dispose();
        }

        public int FXComplete()
        {
            return _fxcontext.SaveChanges();
        }

        public void FXDispose()
        {
            _fxcontext.Dispose();
        }

        public int Complete()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _fxcontext.Dispose();
            _iccontext.Dispose();
        }
    }
}