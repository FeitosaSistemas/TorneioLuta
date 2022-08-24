using System.Collections.Generic;

namespace TorneioLuta.Domain.Entities
{
    public class Lutador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public List<string> ArtesMarciais { get; set; }
        public int Lutas { get; set; }
        public int Derrotas { get; set; }
        public int Vitorias { get; set; }
    }
}
