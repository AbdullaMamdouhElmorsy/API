
using Core.Entities.FXRatesEntities;
using Core.Entities.ICEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<ForeignExchangeTab> ForeignExchangeTabs { get; }
        IBaseRepository<IcPriceFTab> IcPriceFTabs { get; }

        int Complete();

        int ICComplete();

        void ICDispose();

        int FXComplete();

        void FXDispose();
    }
}