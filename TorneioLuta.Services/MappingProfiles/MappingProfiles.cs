using AutoMapper;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using TorneioLuta.Domain.DTO;
using TorneioLuta.Domain.Entities;

namespace TorneioLuta.Services.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<JObject, Lutador>()
                .ForMember(d => d.Id, opt => opt.MapFrom(j => j.SelectToken("id").Value<Int32>()))
                .ForMember(d => d.Nome, opt => opt.MapFrom(j => j.SelectToken("nome").Value<string>()))
                .ForMember(d => d.Idade, opt => opt.MapFrom(j => j.SelectToken("idade").Value<Int32>()))
                .ForMember(d => d.Lutas, opt => opt.MapFrom(j => j.SelectToken("lutas").Value<Int32>()))
                .ForMember(d => d.Derrotas, opt => opt.MapFrom(j => j.SelectToken("derrotas").Value<Int32>()))
                .ForMember(d => d.Vitorias, opt => opt.MapFrom(j => j.SelectToken("vitorias").Value<Int32>()))
                .ForMember(d => d.ArtesMarciais, opt => opt.MapFrom(j => j.SelectToken(".artesMarciais").Values<string>().ToList()))
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ReverseMap();
        }
    }
}
