using System;

namespace Model
{
    public class Consulta : IModel
    {
        public string Data { get; set; }
        public string Hora { get; set; }
        public string Peso { get; set; }
        public string PercentualGordura { get; set; }
        public string SensacaoFisica { get; set; }
        public string RestricoesAlimentares01 { get; set; }
        public string RestricoesAlimentares02 { get; set; }
        public string RestricoesAlimentares03 { get; set; }
        public string RestricoesAlimentares04 { get; set; }
        public string RestricoesAlimentares05 { get; set; }
        public string RestricoesAlimentares06 { get; set; }
        public string RestricoesAlimentares07 { get; set; }
        public string RestricoesAlimentares08 { get; set; }
        public Cliente ClienteConsulta { get; set; }
    }
}