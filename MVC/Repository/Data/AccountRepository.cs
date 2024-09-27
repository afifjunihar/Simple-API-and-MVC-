using MVC.Models;
using MVC.Models.ViewModels;
using MVC.Base.Urls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Repository.Data
{
	public class AccountRepository //: GeneralRepository<MsUser, string>
    {
		private readonly Address _address;
		private readonly string _request;
		private readonly HttpClient httpClient;

		public AccountRepository(Address address, string request = "Login/") //: base(address, request)
		{
			this._address = address;
			this._request = request;
			httpClient = new HttpClient
			{
				BaseAddress = new Uri(address.Link)
			};
		}

		public async Task<JWTokenVM> Auth(LoginVM login)
		{
			StringContent content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
			var result = await httpClient.PostAsync(_request + "user", content);

			string apiResponse = await result.Content.ReadAsStringAsync();
            JWTokenVM? token = JsonConvert.DeserializeObject<JWTokenVM>(apiResponse);

			return token??new JWTokenVM();
		}
        public HttpStatusCode Register(LoginVM login)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");        
            var result = httpClient.PostAsync(_request + "register-user", content).Result;
            return result.StatusCode;

        }
    }
}