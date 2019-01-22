using Model;
using Presenter;
using View;
using System;

namespace nutriapp
{
    class Cliente
    {
        public string Nome = null;
        public string Endereco = null;
        public string Telefone = null;
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            // AlimentoPresenter presenter = new AlimentoPresenter();
            // Alimento alimento = new Alimento();
            // AlimentoView view = new AlimentoView();
            // ConsoleView cvw = new ConsoleView(presenter);
            
            // view.SetModel(alimento);
            // view.CreateScreenDefinition(cvw);
            // view.Show(cvw);
            
            // Cliente cliente = new Cliente();

            // ConsoleView2.ScreenSection(0, 0, "Nome....:");
            // ConsoleView2.ScreenSection(11, 0, cliente, "Nome", EnScreenType.eString, 69);
            // ConsoleView2.ScreenSection(0, 1, "Endereço:");
            // ConsoleView2.ScreenSection(11, 1, cliente, "Endereco", EnScreenType.eString, 69);
            // ConsoleView2.ScreenSection(0, 2, "Telefone:");
            // ConsoleView2.ScreenSection(11, 2, cliente, "Telefone", EnScreenType.eString, 69);
            // //
            // ConsoleView2.CreateScreen();

            // Console.WriteLine("Nome....: " + cliente.Nome);
            // Console.WriteLine("Endereco: " + cliente.Endereco);
            // Console.WriteLine("Telefone: " + cliente.Telefone);

            ConsoleView consoleView = ConsoleView.Instance();
            
            MainPresenter presenter = new MainPresenter();
            //
            presenter.SetView(new MainView(), new Main());
            presenter.Show(consoleView);
        }
    }
}

