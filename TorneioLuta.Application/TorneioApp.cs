using System;
using System.Collections.Generic;
using System.Linq;
using TorneioLuta.Domain.DTO;
using TorneioLuta.Domain.Entities;
using TorneioLuta.Domain.Enum;
using TorneioLuta.Interfaces.Application;
using TorneioLuta.Interfaces.Services;

namespace TorneioLuta.Application
{
    public class TorneioApp : ITorneioApp
    {
        private ILutadoresServices _lutadoresServices;

        public TorneioApp(ILutadoresServices lutadoresServices)
        {
            _lutadoresServices = lutadoresServices;
        }

        #region Public Methods

        public List<GrupoDTO> IniciarTorneio(List<LutadoresSelectedDTO> lutadoresIds)
        {
            List<Lutador> lutadores = _lutadoresServices.Getlutadores();
            List<Lutador> lutadoresSelecteds = new List<Lutador>();

            foreach (var item in lutadoresIds)
                lutadoresSelecteds.Add(lutadores.Where(w => w.Id.Equals(item.Id)).FirstOrDefault());

            List<List<GrupoDTO>> grupoList = this.CriarGrupos(lutadoresSelecteds);
            return this.Lutas(grupoList);
        }

        public List<List<GrupoDTO>> CriarGrupos(List<Lutador> lutadores)
        {
            List<Lutador> lutadoresAll = lutadores.OrderBy(o => o.Idade).ToList();
            var gruposList = new List<List<GrupoDTO>>();

            var grupo = new List<GrupoDTO>();
            for (int i = 0, k = 1; lutadoresAll.Count > i; i++)
            {
                grupo.Add(new GrupoDTO
                {
                    IdGrupo = k,
                    IdLutador = lutadoresAll[i].Id,
                    Nome = lutadoresAll[i].Nome,

                    Lutas = lutadoresAll[i].Lutas,
                    Vitorias = lutadoresAll[i].Vitorias,

                    QtdArtesMarciais = lutadoresAll[i].ArtesMarciais.Count
                });

                if (grupo.Count == 5)
                {
                    gruposList.Add(grupo);
                    grupo = new List<GrupoDTO>();
                    k++;
                }
            }

            return gruposList;
        }

        public List<GrupoDTO> Lutas(List<List<GrupoDTO>> grupos)
        {
            List<List<GrupoDTO>> VencedoresFaseGrupoList = new List<List<GrupoDTO>>();

            //Primeira fase: lutadores do mesmo grupo
            foreach (var grupo in grupos)
            {
                List<GrupoDTO> VencedoresFaseGrupo = new List<GrupoDTO>();

                List<GrupoDTO> PrimeiraFase = this.PrimeiraFase(grupo);
                List<GrupoDTO> resultado = PrimeiraFase.OrderByDescending(o => o.TotalVitorias).ToList();

                //Identificando os 2 primeiros
                VencedoresFaseGrupo.Add(resultado[0]);
                VencedoresFaseGrupo.Add(resultado[1]);

                VencedoresFaseGrupoList.Add(VencedoresFaseGrupo);
            }

            //Segunda fase - Quartas de Final: Luta entre o primeiro e o segundo dos grupos
            List<GrupoDTO> resultadofase2 = this.QuartasFinal(VencedoresFaseGrupoList);

            //Terceira fase - Semifinal
            List<List<GrupoDTO>> resultadoSemiFinal = this.SemiFinal(resultadofase2);

            //Final
            List<GrupoDTO> resultadoFinal = this.Final(resultadoSemiFinal);

            return resultadoFinal;
        }

        #endregion Public Methods

        #region Private Methods

        private int ValidacaoLuta(GrupoDTO desafiante, GrupoDTO desafiado)
        {
            //Caso de vitória do desafiante
            if (desafiante.Porcentagem > desafiado.Porcentagem)
                return desafiante.IdLutador;

            //Caso de vitória do desafiado
            else if (desafiante.Porcentagem < desafiado.Porcentagem)
                return desafiado.IdLutador;

            //Caso de empate
            else
            {
                //Regra 1 - Desempate: Caso o desafiante tenha mais artes marciais
                if (desafiante.QtdArtesMarciais > desafiado.QtdArtesMarciais)
                    return desafiante.IdLutador;

                //Regra 2 - Desempate: 
                else if (desafiante.Lutas > desafiado.Lutas)
                    return desafiante.IdLutador;

                //Caso o desafiante não tenha os critérios a seu favor o desafiado será o vitorioso
                else
                    return desafiado.IdLutador;

            }
        }

        private List<GrupoDTO> PrimeiraFase(List<GrupoDTO> grupo)
        {
            for (int i = 0; i < grupo.Count; i++)
            {
                var desafiante = grupo[i];

                for (int k = i + 1; grupo.Count > k; k++)
                {
                    var desafiado = grupo[k];

                    int vitoriosoId = ValidacaoLuta(desafiante, desafiado);
                    if (desafiante.IdLutador == vitoriosoId)
                    {
                        desafiante.Vitorias += 1;
                        desafiante.Lutas += 1;

                        desafiante.TotalVitorias += 1;
                    }
                    else
                    {
                        desafiado.Vitorias += 1;
                        desafiado.Lutas += 1;

                        desafiado.TotalVitorias += 1;
                    }
                }
            }

            return grupo;
        }

        private List<GrupoDTO> QuartasFinal(List<List<GrupoDTO>> grupos)
        {
            List<GrupoDTO> VencedoresFase2List = new List<GrupoDTO>();

            for (int i = 0; i < grupos.Count; i += 2)
            {
                List<GrupoDTO> grupo1 = grupos[i].OrderBy(o => o.TotalVitorias).ToList();
                List<GrupoDTO> grupo2 = grupos[(i + 1)].OrderBy(o => o.TotalVitorias).ToList();

                //Luta1: Primeiro colocado do grupo1 luta contra o segundo do grupo subsequente
                GrupoDTO desafianteG1P1 = grupo1[0];
                GrupoDTO desafiadoG2P2 = grupo2[1];

                desafianteG1P1.TotalVitorias = 0;
                desafiadoG2P2.TotalVitorias = 0;

                //Validação Luta
                var vencedorId = this.ValidacaoLuta(desafianteG1P1, desafiadoG2P2);

                if (desafianteG1P1.IdLutador == vencedorId)
                {
                    desafianteG1P1.Lutas += 1;
                    desafianteG1P1.Vitorias += 1;

                    VencedoresFase2List.Add(desafianteG1P1);
                }
                else
                {
                    desafiadoG2P2.Lutas += 1;
                    desafiadoG2P2.Vitorias += 1;

                    VencedoresFase2List.Add(desafiadoG2P2);
                }

                //Luta2: Segundo colocado do grupo1 luta com o primeiro colocado do grupo subsequente
                GrupoDTO desafianteG1P2 = grupo1[1];
                GrupoDTO desafiadoG2P1 = grupo2[0];

                desafianteG1P2.TotalVitorias = 0;
                desafiadoG2P1.TotalVitorias = 0;

                vencedorId = this.ValidacaoLuta(desafianteG1P2, desafiadoG2P1);
                if (desafianteG1P2.IdLutador == vencedorId)
                {
                    desafianteG1P2.Lutas += 1;
                    desafianteG1P2.Vitorias += 1;

                    VencedoresFase2List.Add(desafianteG1P2);
                }
                else
                {
                    desafiadoG2P1.Lutas += 1;
                    desafiadoG2P1.Vitorias += 1;

                    VencedoresFase2List.Add(desafiadoG2P1);
                }
            }

            return VencedoresFase2List;
        }

        private List<List<GrupoDTO>> SemiFinal(List<GrupoDTO> semifinalistas)
        {
            List<List<GrupoDTO>> final = new List<List<GrupoDTO>>();

            List<GrupoDTO> finalistaslist = new List<GrupoDTO>();
            List<GrupoDTO> terceiroLugar = new List<GrupoDTO>();

            for (int i = 0; semifinalistas.Count > i; i = i + 2)
            {
                GrupoDTO desafiante = semifinalistas[i];
                GrupoDTO desafiado = semifinalistas[(i + 1)];

                desafiante.TotalVitorias = 0;
                desafiado.TotalVitorias = 0;

                var vitoriosoId = this.ValidacaoLuta(desafiante, desafiado);

                if (desafiante.IdLutador == vitoriosoId)
                {
                    desafiante.Lutas += 1;
                    desafiante.Vitorias += 1;

                    finalistaslist.Add(desafiante);
                    terceiroLugar.Add(desafiado);
                }
                else
                {
                    desafiado.Lutas += 1;
                    desafiado.Vitorias += 1;

                    finalistaslist.Add(desafiado);
                    terceiroLugar.Add(desafiante);
                }
            }

            final.Add(finalistaslist);
            final.Add(terceiroLugar);

            return final;
        }

        private List<GrupoDTO> Final(List<List<GrupoDTO>> finalistas)
        {
            List<GrupoDTO> resultado = new List<GrupoDTO>();
            foreach (var item in finalistas)
            {
                List<GrupoDTO> final = item;

                for (int i = 0; final.Count > i; i = i + 2)
                {
                    GrupoDTO desafiante = final[i];
                    GrupoDTO desafiado = final[(i + 1)];

                    //Disputa pelo título de campeão e vice-campeão
                    var vencedorId = this.ValidacaoLuta(desafiante, desafiado);
                    if (vencedorId == desafiante.IdLutador)
                    {
                        desafiante.Lutas += 1;
                        desafiante.Vitorias += 1;

                        desafiante.StatusTorneio = (resultado.Count < 1) ? StatusTorneioEnum.Campeao : StatusTorneioEnum.TerceiroColocado;
                        desafiado.StatusTorneio = (resultado.Count < 1) ? StatusTorneioEnum.ViceCampecao : StatusTorneioEnum.QuartoLugar;
                    }
                    else
                    {
                        desafiado.Lutas += 1;
                        desafiado.Vitorias += 1;

                        desafiado.StatusTorneio = (resultado.Count < 1) ? StatusTorneioEnum.Campeao : StatusTorneioEnum.TerceiroColocado;
                        desafiante.StatusTorneio = (resultado.Count < 1) ? StatusTorneioEnum.ViceCampecao : StatusTorneioEnum.QuartoLugar;
                    }

                    resultado.Add(desafiado);
                    resultado.Add(desafiante);
                }
            }

            return resultado;
        }

        #endregion Private Methods
    }
}
