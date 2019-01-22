using Model;
using Presenter;
using System;

namespace View
{
    public class GerarCombinacaoAlimentarView : IView
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

            consoleView.EnableSearch = true;

            consoleView.ScreenSection(0, 1, "Calorias totais (digite o valor e pressiona F3):");
            consoleView.ScreenSection(50, 1, Model, "Calorias", EnScreenType.eString, 4);

            consoleView.CreateScreen();
        }
    }
}