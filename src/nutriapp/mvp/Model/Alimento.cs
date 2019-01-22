using System;

namespace Model
{
    public enum EnGrupoAlimentar
    {
        eGrupo1 = 1,
        eGrupo2 = 2,
        eGrupo3 = 3
    }

    public class Alimento : IModel
    {
        public string GrupoAlimentar { get; set; }
        public string NomeAlimento { get; set; }
        public string Calorias { get; set; }
    }
}