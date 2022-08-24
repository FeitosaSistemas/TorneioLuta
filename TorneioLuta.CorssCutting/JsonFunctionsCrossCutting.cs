using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using TorneioLuta.Domain.DTO;

namespace TorneioLuta.CrossCutting
{
    public static class JsonFunctionsCrossCutting
    {
        public static List<LutadoresSelectedDTO> ConvertForDTO(string lutadoresId)
        {
            List<LutadoresSelectedDTO> resultado = new List<LutadoresSelectedDTO>();
            foreach (var item in lutadoresId.Split(','))
                resultado.Add(new LutadoresSelectedDTO { Id = Convert.ToInt32(item) });

            return resultado;
        }
    }
}
