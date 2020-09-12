using KooshDaroo.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KooshDaroo.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace KooshDaroo.Services
{
    class PharmacyService
    {
        public async Task<List<KooshDaroo.Models.Pharmacy>> GetPharmacyAsync()
        {
            //RestClient<Pharmacy> restClient = new RestClient<Pharmacy>();
            //var PharmacyList = await restClient.GetAsync("Pharmacy");
            //return PharmacyList;

            var httpClient = new HttpClient();
            try
            {
                var json = await httpClient.GetStringAsync(App.apiAddress + "Pharmacy/");
                var taskModels = JsonConvert.DeserializeObject<List<KooshDaroo.Models.Pharmacy>>(json);
                return taskModels;
            }
            catch (Exception e)
            {
                var x = e.Message;
                return null;
            }

        }
        public async Task<KooshDaroo.Models.Pharmacy> GetPharmacyByIdAsync(int Id)
        {
            //RestClient<Pharmacy> restClient = new RestClient<Pharmacy>();
            //var json = await restClient.GetAsyncByFieldNameReturnString("Pharmacy", "Id", Id.ToString());


            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync(App.apiAddress + "Pharmacy/Id/" + Id);
            var taskModel = JsonConvert.DeserializeObject<List<KooshDaroo.Models.Pharmacy>>(json);

            return taskModel[0];
        }

        public async Task<List<KooshDaroo.Models.Pharmacy>> GetPharmacyByPhoneNoAsync(string phoneNo)
        {
            //RestClient<Pharmacy> restClient = new RestClient<Pharmacy>();
            //var json = await restClient.GetAsyncByFieldNameReturnString("Pharmacy", "Id", Id.ToString());

            try
            {
                var httpClient = new HttpClient();
                var json = await httpClient.GetStringAsync(App.apiAddress + "Pharmacy/PhoneNo/" + phoneNo);
                var taskModel = JsonConvert.DeserializeObject<List<KooshDaroo.Models.Pharmacy>>(json);

                return taskModel;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<KooshDaroo.Models.Pharmacy> PostPharmacyAsync(KooshDaroo.Models.Pharmacy pharmacy)
        {
            //RestClient<Pharmacy> restClient = new RestClient<Pharmacy>();
            //Pharmacy r = await restClient.PostAsync("Pharmacy",pharmacy);

            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(pharmacy);
            HttpContent httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            try
            {
                var result = await httpClient.PostAsync(App.apiAddress + "Pharmacy/", httpContent);
                var jsonString = await result.Content.ReadAsStringAsync();
                var r = JsonConvert.DeserializeObject<KooshDaroo.Models.Pharmacy>(jsonString);

                return r;
            }
            catch (Exception ex)
            {
                return pharmacy;
            }
        }


    }
}
