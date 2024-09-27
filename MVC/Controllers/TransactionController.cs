using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Base.Urls;
using MVC.Models;
using MVC.Models.ViewModels;
using MVC.Repository.Data;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace MVC.Controllers
{
	[Authorize]
	public class TransactionController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly AccountRepository _account;
		private readonly TrBPKBRepository _trBPKB;
        private readonly Address _address;

        public TransactionController(ILogger<HomeController> logger, AccountRepository account, TrBPKBRepository trBPKB,Address address)
		{
			_logger = logger;
			_account = account;
			_trBPKB = trBPKB;
            _address = address;

		}
		public IActionResult Index()
		{
			return View();
		}

        [HttpPost]
        [Route("/add-bpkb")]
        public async Task<JsonResult> AddBpkb(AddBpkbVM data)
        {
            TrBpkb trBpkb = new TrBpkb();
            trBpkb.FakturDate = data.FakturDate;
            trBpkb.BpkbDate = data.BpkbDate;
            trBpkb.BpkbDateIn = data.BpkbDateIn;
            trBpkb.BpkbNo = data.BpkbNo;
            trBpkb.LastUpdatedBy = HttpContext.Session.GetString("UserName");
            trBpkb.CreatedBy = HttpContext.Session.GetString("UserName");
            trBpkb.CreatedOn = DateTime.Now;
            trBpkb.LocationId = data.LocationId;
            trBpkb.AgreementNumber = data.AgreementNumber;
            trBpkb.BranchId = data.BranchId;
            trBpkb.FakturNo = data.FakturNo;
            trBpkb.LastUpdatedOn = DateTime.Now;
            trBpkb.PoliceNo = data.PoliceNo;
            
            StringContent content = new StringContent(JsonConvert.SerializeObject(trBpkb), Encoding.UTF8, "application/json");
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(_address.Link),

            };
            http.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("JWToken"));

            var result = await http.PostAsync(_address.Link + "transaction/" + "register-bpkb", content);
   
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return Json(new { Status = HttpStatusCode.BadRequest, Message = "BPKB Sudah Terdaftar Sebelumnya." });
            }
            return Json(new { Status = HttpStatusCode.OK, Message = "BPKB Berhasil Terdaftar." });
        }

        [HttpGet]
        [Route("/get-all-location")]
        public async Task<List<MsStorageLocation>> GetAllLocation()
        {
            List<MsStorageLocation>? msStorageLocations = new List<MsStorageLocation>();
            HttpClient http = new HttpClient
            {
                BaseAddress = new Uri(_address.Link),

            };
            http.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("JWToken"));

            using (var response = await http.GetAsync(_address.Link + "transaction/" + "get-all-storage-location"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                msStorageLocations = JsonConvert.DeserializeObject<List<MsStorageLocation>>(apiResponse);
            }
            return msStorageLocations ?? new List<MsStorageLocation>();
        }
    }
}
