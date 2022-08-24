using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using TorneioLuta.Domain.DTO;
using TorneioLuta.Domain.Entities;
using TorneioLuta.Interfaces.Application;
using TorneioLuta.Interfaces.Services;

namespace TorneioLuta.Application
{
    public class LutadoresApp : ILutadoresApp
    {
        private ILutadoresServices _lutadoresServices;

        public LutadoresApp(ILutadoresServices lutadoresServices)
        {
            _lutadoresServices = lutadoresServices;
        }

        public List<Lutador> GetAll()
        {
            return _lutadoresServices.Getlutadores();
        }
    }
}
