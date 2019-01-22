using Model;
using Presenter;
using View;
using System;
using Xunit;

namespace nutriapp.tests
{
    public class AlimentoTest
    {
        [Fact]
        public void SaveTest()
        {
            Alimento alimento1 = new Alimento()
            {
               NomeAlimento = "Maça",
               Calorias = "2",
               GrupoAlimentar = "1"
            };
            Assert.True(SaveData(alimento1), "Dados salvos com sucesso! :)");
        }

        private bool SaveData(Alimento alimento)
        {
            string msgErr = null;

            AlimentoPresenter presenter = new AlimentoPresenter();
            presenter.SetView(new AlimentoView(), alimento);
            return presenter.Save(out msgErr);
        }

    }
}
