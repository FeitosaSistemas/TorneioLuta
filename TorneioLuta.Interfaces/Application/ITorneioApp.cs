using System.Collections.Generic;
using TorneioLuta.Domain.DTO;
using TorneioLuta.Domain.Entities;

namespace TorneioLuta.Interfaces.Application
{
    public interface ITorneioApp
    {
        List<GrupoDTO> IniciarTorneio(List<LutadoresSelectedDTO> lutadores);
        List<List<GrupoDTO>> CriarGrupos(List<Lutador> lutadores);
        List<GrupoDTO> Lutas(List<List<GrupoDTO>> grupos);
    }
}
