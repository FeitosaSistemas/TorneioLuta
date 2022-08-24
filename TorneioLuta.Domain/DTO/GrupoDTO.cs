using System;
using TorneioLuta.Domain.Enum;

namespace TorneioLuta.Domain.DTO
{
    public class GrupoDTO
    {
        public int IdGrupo { get; set; }
        public int IdLutador { get; set; }
        public string Nome { get; set; }

        public decimal Lutas { get; set; }
        public decimal Vitorias { get; set; }

        public int QtdArtesMarciais { get; set; }

        public decimal Porcentagem
        {
            get
            {
                if (this.Lutas < 1 || this.Vitorias < 1)
                    return 0;

                return Math.Round((decimal)(this.Lutas / this.Vitorias) * 100);
            }
        } 

        public int TotalVitorias { get; set; }
        public StatusTorneioEnum StatusTorneio { get; set; }
    }
}
