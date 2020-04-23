using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IPrescriptionsDbService
    {

        public IEnumerable<Prescription> GetPrescriptions(string lekarz);
        public PrescriptionsRequest CreatePrescription(PrescriptionsRequest request);
    }
}
