using System.Collections.Generic;
using TorneioLuta.Domain.DTO;
using TorneioLuta.Domain.Entities;

namespace TorneioLuta.Interfaces.Application
{
    public interface ILutadoresApp
    {
        List<Lutador> GetAll();
    }
}
