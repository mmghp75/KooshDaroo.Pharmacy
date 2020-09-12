using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using KooshDaroo.Models;
using KooshDaroo;

namespace Plugin.RestClient
{
    /// <summary>
    /// RestClient implements methods for calling CRUD operations
    /// using HTTP.
    /// </summary>
    public class RestClient<T>
    {
        public string WebServiceUrl = App.apiAddress;//"http://10.0.2.2:55011/api/";
        //private string WebServiceUrl = App.apiAddress;//"http://192.168.43.55/api/";

        public async Task<List<T>> GetAsync(string tablename)
        {
            var httpClient = new HttpClient();

            try
            {
                var json = await httpClient.GetStringAsync(WebServiceUrl + tablename + "/");


                var taskModels = JsonConvert.DeserializeObject<List<T>>(json);

                return taskModels;
            }
            catch (Exception e)
            {
                var x = e.Message;
                return null;
            }
        }
        public async Task<List<T>> GetAsyncByFieldName(string tablename, string fieldname, string valueoffield)
        {
            var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync(WebServiceUrl + tablename + "/" + fieldname + "/" + valueoffield);

            var taskModel = JsonConvert.DeserializeObject<List<T>>(json);

            return taskModel;
        }
        public async Task<T> GetAsyncByFieldNameFirst(string tablename, string fieldname, string valueoffield)
        {
            var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync(WebServiceUrl + tablename + "/" + fieldname + "First/" + valueoffield);

            var taskModels = JsonConvert.DeserializeObject<T>(json);

            return taskModels;
        }
        public async Task<T> PostAsync(string tablename, T t)
        {
            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(t);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            try
            {
                var result = await httpClient.PostAsync(WebServiceUrl + tablename + "/", httpContent);
                var jsonString = await result.Content.ReadAsStringAsync();
                T r = JsonConvert.DeserializeObject<T>(jsonString);
                return r;
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                return t;
            }
        }
        public async Task<string> PostAsyncReturnString(string tablename, T t)
        {
            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(t);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            try
            {
                var result = await httpClient.PostAsync(WebServiceUrl + tablename + "/", httpContent);
                var jsonString = await result.Content.ReadAsStringAsync();
                return jsonString;
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                return "";
            }
        }
        public async Task<bool> PutAsync(string tablename, int id, T t)
        {
            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(t);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PutAsync(WebServiceUrl + tablename + "/" + id, httpContent);

            return result.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteAsync(string tablename, int id, T t)
        {
            var httpClient = new HttpClient();

            var response = await httpClient.DeleteAsync(WebServiceUrl + tablename + "/" + id);

            return response.IsSuccessStatusCode;
        }
    }
}