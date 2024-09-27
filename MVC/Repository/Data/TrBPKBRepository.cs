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
    public class TrBPKBRepository 
    {

        private readonly Address address;
        private readonly string request;
        private readonly HttpClient httpClient;
        public TrBPKBRepository(Address address, string request = "transaction/")
        {
            this.address = address;
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.Link),
            };
        }

        public async Task<List<MsStorageLocation>> GetStorageLocations()
        {
            List<MsStorageLocation>? msStorageLocations = new List<MsStorageLocation>();
            using (var response = await httpClient.GetAsync(address.Link + request + "get-all-storage-location"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                msStorageLocations = JsonConvert.DeserializeObject<List<MsStorageLocation>>(apiResponse);
            }
            return msStorageLocations ?? new List<MsStorageLocation>();
        }
        public async Task<List<TrBpkb>> GetListBPKB()
        {

            List<TrBpkb>? entities = new List<TrBpkb>();
            using (var response = await httpClient.GetAsync(address.Link + request + "get-all-bpkb"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entities = JsonConvert.DeserializeObject<List<TrBpkb>>(apiResponse);
            }
            return entities?? new List<TrBpkb>();
        }

        public async Task<TrBpkb> GetBPKB(string Id)
        {
            TrBpkb? entity = new TrBpkb();
            using (var response = await httpClient.GetAsync(address.Link + request + "get-bpkb/" + Id))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<TrBpkb>(apiResponse);
            }
            return entity?? new TrBpkb();
        }

    }



}
