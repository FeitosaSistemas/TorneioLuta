using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using TorneioLuta.Domain.DTO;
using TorneioLuta.Domain.Entities;
using TorneioLuta.Interfaces.Services;

namespace TorneioLuta.Services
{
    public class LutadoresServices : ILutadoresServices
    {
        private IConfiguration _configuration;
        private IMapper _mapper;
        private ApiDTO api = new ApiDTO();

        public LutadoresServices(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;

            api.Uri = _configuration.GetSection("APILutador:uri").Value;
            api.HeaderKeyName = _configuration.GetSection("APILutador:headerKeyName").Value;
            api.Key = _configuration.GetSection("APILutador:key").Value;
        }

        public List<Lutador> Getlutadores()
        {
            List<Lutador> lutadoresList = new List<Lutador>();

            try
            {
                using (var http = new HttpClient())
                {
                    http.BaseAddress = new Uri(api.Uri);
                    http.DefaultRequestHeaders.Add(api.HeaderKeyName, api.Key);

                    var response = http.GetAsync(http.BaseAddress).Result;

                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(response.RequestMessage.ToString());

                    var json = JArray.Parse(response.Content.ReadAsStringAsync().Result);
                    foreach (var item in json)
                    {
                        Lutador lutador = _mapper.Map<JToken, Lutador>(item);
                        lutadoresList.Add(lutador);
                    }
                }
            }
            catch (Exception)
            {
                lutadoresList = new List<Lutador>();
            }

            return lutadoresList;
        }
    }
}
