using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Models;
using vega.Persistence;

namespace vega.Controllers
{
    public class MakesController : Controller
    {
        private readonly VegaDbContext context;
        // Create a field to store the mapper object
        private readonly IMapper _mapper;

        public MakesController(VegaDbContext context, IMapper mapper)
        {
            this.context = context;
            _mapper = mapper;
        }


        [HttpGet("/api/makes")]
        public async Task<IEnumerable<MakeResource>> GetMakes()
        {
            var makes = await context.Makes.Include(m => m.Models).ToListAsync();
            return _mapper.Map<IEnumerable<MakeResource>>(makes);
        }
    }
}