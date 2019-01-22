using System;
using System.Collections.Generic;

namespace Model
{
    public class CombinacaoAlimentar : IModel
    {
        public CombinacaoAlimentar()
        {
            Alimentos = new List<Alimento>();
        }
        
        public string Calorias { get; set; }
        public List<Alimento> Alimentos { get; set; }
        public List<Alimento> CombinacoesAlimentares { get; set; }
    }
}