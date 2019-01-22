using Model;
using Presenter;
using View;
using System;
using Xunit;

namespace nutriapp.tests
{
    public class ClienteTest
    {
        [Fact]
        public void SaveTest()
        {
            Cliente cliente1 = new Cliente()
            {
                Nome = "Jose da Silva",
                Telefone = "(31) 9999-9999",
                Email = "jose.silva@gmail.com",
                Endereco = "Rua das Flores, 35, Parque São Jorge",
                DataNascimento = "20/01/1970"
            };
            Assert.True(SaveData(cliente1), "Dados salvos com sucesso! :)");

            Cliente cliente2 = new Cliente()
            {
                Nome = "Margarete Souza",
                Telefone = "",
                Email = "margarete.souza@gmail.com",
                Endereco = "Av Getulio Vargas, 1621, Funcionarios",
                DataNascimento = "08/02/1983"
            };
            Assert.False(SaveData(cliente2), "Dados inconsistentes. NãO foram salvos! :(");
        }

        private bool SaveData(Cliente cli)
        {
            string msgErr = null;

            ClientePresenter presenter = new ClientePresenter();
            presenter.SetView(new ClienteView(), cli);
            return presenter.Save(out msgErr);
        }

    }
}
