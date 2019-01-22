using Model;
using Presenter;
using System;

namespace View
{
    public class MainView : IView
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

            consoleView.ScreenSection(20, 2, "F1. Cadastrar Alimento");
            consoleView.ScreenSection(20, 3, "F2. Cadastrar Cliente");
            consoleView.ScreenSection(20, 4, "F3. Cadastrar Consulta");

            consoleView.ScreenSection(20, 6, "Digite uma opção:");
            consoleView.ScreenSection(38, 6, Model, "MenuItem", EnScreenType.eString, 1);

            consoleView.CreateScreen();
        }
    }
}