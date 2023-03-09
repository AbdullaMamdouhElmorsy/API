using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using static Core.Consts.Consts;
using Core.Entities.ICEntities;
using API.Errors;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class ICController : BaseApiController
    {

        private readonly IUnitOfWork _unitsRepo;
        private readonly IMapper _mapper;

        public ICController(IUnitOfWork unitsRepo, IMapper mapper)
        {
            _unitsRepo = unitsRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiException), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<IcPriceFTabDto>>> GetICPrices()
        {

            var IcPrices = new List<IcPriceFTab>();
            var equityPrices = await _unitsRepo.IcPriceFTabs.FindAllAsync(x => x.IcTypeId == 1, null, null, d => d.EntryDate, OrderBy.Descending);
            IcPrices.Add(equityPrices.First());
            var MoneyMarketPrices = await _unitsRepo.IcPriceFTabs.FindAllAsync(x => x.IcTypeId == 2, null, null, d => d.EntryDate, OrderBy.Descending);
            IcPrices.Add(MoneyMarketPrices.First());
            var data = _mapper.Map<IReadOnlyList<IcPriceFTabDto>>(IcPrices);
            return Ok(data);
        }






    }
}