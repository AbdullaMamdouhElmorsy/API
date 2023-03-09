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
using Infrastructure.Data;
using API.Errors;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class FXRatesController : BaseApiController
    {

        private readonly IUnitOfWork _unitsRepo;
        private readonly IMapper _mapper;

        public FXRatesController(IUnitOfWork unitsRepo , IMapper mapper)
        {
            _unitsRepo = unitsRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IReadOnlyList<ForeignExchangeTabDto>) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiException), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ForeignExchangeTabDto>> GetFXRates()
        {

            var currencies = new List<String>{
                "usd","eur","gbp","chf","jpy","kwd","sar","aed"
            };

            var rates = await _unitsRepo.ForeignExchangeTabs.FindAllAsync(x => x.InitialCurrencyCode.ToLower() != "egp"
            && x.AginstCurrencyCode.ToLower() == "egp" && currencies.Contains(x.InitialCurrencyCode.ToLower()) , null ,null , d => d.InitialCurrencyCode , OrderBy.Ascending);
            var data = _mapper.Map<IReadOnlyList<ForeignExchangeTabDto>>(rates);
            return Ok(data);
        }

    }
}