using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLuta.Domain.Entities;

namespace TorneioLuta.Interfaces.Services
{
    public interface ILutadoresServices
    {
        List<Lutador> Getlutadores();
    }
}
