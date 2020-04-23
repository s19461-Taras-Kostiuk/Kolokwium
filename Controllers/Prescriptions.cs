using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/prescriptions")]
    public class Prescriptions : ControllerBase
    {

        private readonly IPrescriptionsDbService _dbService;

        public Prescriptions(IPrescriptionsDbService dbService)
        {
            this._dbService = dbService;
        }


        [HttpGet]
        public IActionResult GetAnimals(string lekarz)
        {
            var result = _dbService.GetPrescriptions(lekarz);
            if (result == null)
            {
                ObjectResult res = new ObjectResult(result);
                res.StatusCode = 400;
                return res;
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreatePresc(PrescriptionsRequest presc)
        {

            var res = _dbService.CreatePrescription(presc);
            if (res == null)
                return BadRequest();
            return Created("", res);


        }
    }
}
