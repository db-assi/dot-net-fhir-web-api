using dotnet_azure_fhir_web_api.IServices;
using dotnet_azure_fhir_web_api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_azure_fhir_web_api.Services
{
    public class ResourceFetchService : IResourceFetchService
    {
        private readonly IProtectedWebApiCallerService _caller;
        private readonly ILoggerManager _logger;
        private readonly AuthenticationConfig config = AuthenticationConfig.ReadFromJsonFile("appsettings.json");

        public ResourceFetchService(IProtectedWebApiCallerService caller, ILoggerManager logger)
        {
            _caller = caller;
            _logger = logger;
        }

        public async Task<JObject> GetSingleResource(string requestOptions)
        {
            try
            {
                _logger.LogInfo($"{Environment.NewLine}Class: ResourceFetchService, Method: GetSingleResource");
                return await _caller.ProtectedWebApiCaller($"{config.BaseAddress}{requestOptions}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Environment.NewLine}Class: ResourceFetchService, Method: GetSingleResource, {Environment.NewLine}Exception: {ex}, {Environment.NewLine}Message: {ex.Message}, {Environment.NewLine}StackTrace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<List<JObject>> GetMultipleResources(string requestOptions, List<string> ids)
        {
            try
            {
                _logger.LogInfo($"{Environment.NewLine}Class: ResourceFetchService, Method: GetMultipleResources");
                List<JObject> results = new List<JObject>();
                for (int i = 0; i < ids.Count; i++)
                {
                    results.Add(await GetSingleResource($"{requestOptions}{ids[i]}"));
                }
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Environment.NewLine}Class: ResourceFetchService, Method: GetMultipleResources, {Environment.NewLine}Exception: {ex}, {Environment.NewLine}Message: {ex.Message}, {Environment.NewLine}StackTrace: {ex.StackTrace}");
                return null;
            }
        }


        public async Task<List<JObject>> GetAllPages(string requestOptions)
        {
            List<JObject> list = new List<JObject>();

            try
            {
                _logger.LogInfo($"{Environment.NewLine}Class: ResourceFetchService, Method: GetAllPages");
                var json = await _caller.ProtectedWebApiCaller($"{config.BaseAddress}{requestOptions}");
                return await RetrieveAllPages(json, list);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Environment.NewLine}Class: ResourceFetchService, Method: GetAllPages, {Environment.NewLine}Exception: {ex}, {Environment.NewLine}Message: {ex.Message}, {Environment.NewLine}StackTrace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<List<JObject>> GetPages(string requestOptions, int pages)
        {
            List<JObject> list = new List<JObject>();

            try
            {
                _logger.LogInfo("Class: ResourceFetchService, Method: GetPages");
                var json = await _caller.ProtectedWebApiCaller($"{config.BaseAddress}{requestOptions}");
                return await RetrievePages(json, list, pages);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Environment.NewLine}Class: ResourceFetchService, Method: GetPages, {Environment.NewLine}Exception: {ex}, {Environment.NewLine}Message: {ex.Message}, {Environment.NewLine}StackTrace: {ex.StackTrace}");
                return null;
            }
        }

        private async Task<List<JObject>> RetrieveAllPages(JObject json, List<JObject> list)
        {
            if(json == null)
            {
                return null;
            }


            string relation = (string)json["link"][0]["relation"];
            list.Add(json);

            if (relation.Equals("next"))
            {
                string url = (string)json["link"][0]["url"];
                var response = await _caller.ProtectedWebApiCaller(url);
                return await RetrieveAllPages(response, list);
            }
            else
            {
                return list;
            }
        }

        private async Task<List<JObject>> RetrievePages(JObject json, List<JObject> list, int pages)
        {
            if(json == null)
            {
                return null;
            }


            string relation = (string)json["link"][0]["relation"];
            list.Add(json);

            while (list.Count < pages)
            {
                if (relation.Equals("next"))
                {
                    string url = (string)json["link"][0]["url"];
                    var response = await _caller.ProtectedWebApiCaller(url);
                    return await RetrievePages(response, list, pages);
                }
                else
                {
                    return list;
                }
            }
            return list;
        }
    }
}



