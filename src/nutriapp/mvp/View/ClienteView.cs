using Model;
using Presenter;
using System;

namespace View
{
    public class ClienteView : IView
    {
        public IPresenter Presenter { get; set; }
        public IModel Model { get; set; }

        public void SetPresenter(IPresenter presenter)
        {
            Presenter = presenter;
        }

        public void SetModel(IModel model)
        {
            Model = model;
        }

        public void Show(ConsoleView consoleView, IModel model)
        {
            Model = model;

            consoleView.StartScreen(Presenter);

            consoleView.EnableSave = true;
            consoleView.EnableSearch = true;

            consoleView.ScreenSection(0, 1, "Nome........:");
            consoleView.ScreenSection(15, 1, model, "Nome", EnScreenType.eString, 65);
            consoleView.ScreenSection(0, 2, "Endereço....:");
            consoleView.ScreenSection(15, 2, model, "Endereco", EnScreenType.eString, 65);
            consoleView.ScreenSection(0, 3, "Telefone....:");
            consoleView.ScreenSection(15, 3, model, "Telefone", EnScreenType.eString, 20);
            consoleView.ScreenSection(0, 4, "Email.......:");
            consoleView.ScreenSection(15, 4, model, "Email", EnScreenType.eString, 20);
            consoleView.ScreenSection(0, 5, "Data Nascto.:");
            consoleView.ScreenSection(15, 5, model, "DataNascimento", EnScreenType.eString, 10);

            consoleView.CreateScreen();
        }
    }
}