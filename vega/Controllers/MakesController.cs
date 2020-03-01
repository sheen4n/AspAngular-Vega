using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Core;

namespace vega.Controllers
{
    public class MakesController : Controller
    {
        // Create a field to store the mapper object
        private readonly IMapper _mapper;
        private readonly IUnitOfWork unitOfWork;
        public MakesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet("/api/makes")]
        public async Task<IEnumerable<MakeResource>> GetMakes()
        {
            var makes = await unitOfWork.Makes.GetMakes();
            return _mapper.Map<IEnumerable<MakeResource>>(makes);
        }
    }
}