using API.Context;
using API.Models;
using API.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        public IConfiguration configuration;
        private readonly MGFContext mGFContext;
        public TransactionController(MGFContext mGFContext, IConfiguration configuration)
        {
            this.mGFContext = mGFContext;
            this.configuration = configuration;
        }

        [Route("register-bpkb")]
        [HttpPost]
        public async Task<IActionResult> RegisterBPKB(TrBpkbVM bpkb)
        {
            TrBpkb? data = await mGFContext.TrBpkbs.Where(obj => obj.AgreementNumber == bpkb.AgreementNumber ).FirstOrDefaultAsync();
            if (data != null)
            {
                return BadRequest(new { Message = "Data sudah terdaftar sebelumnya."});
            
            }
            TrBpkb newData = new TrBpkb();
            newData.AgreementNumber = bpkb.AgreementNumber;
            newData.BranchId = bpkb.BranchId;
            newData.BpkbNo = bpkb.BpkbNo;
            newData.BpkbDateIn = bpkb.BpkbDateIn;
            newData.BpkbDate = bpkb.BpkbDate;
            newData.FakturNo = bpkb.FakturNo;
            newData.FakturDate = bpkb.FakturDate;
            newData.PoliceNo = bpkb.PoliceNo;
            newData.CreatedBy = bpkb.CreatedBy;
            newData.CreatedOn = DateTime.Now;
            MsStorageLocation? location = await mGFContext.MsStorageLocations.Where(obj => obj.LocationId == newData.LocationId).FirstOrDefaultAsync();
            newData.Location = location??new MsStorageLocation();


            await mGFContext.TrBpkbs.AddAsync(newData);
            await mGFContext.SaveChangesAsync();
            
            return Ok(new { Message = "Data berhasil terdaftar." });
        }

        [Route("get-all-bpkb")]
        [HttpGet]
        public async Task<IActionResult> GetBPKB()
        {
            List<TrBpkb>? data = await mGFContext.TrBpkbs.ToListAsync();
            return Ok(data);
        }

        [Route("get-bpkb/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetBPKB(string id)
        {
            TrBpkb? data = await mGFContext.TrBpkbs.Where(obj => obj.AgreementNumber == id).FirstOrDefaultAsync();
            return Ok(data);
        }


    }
}
