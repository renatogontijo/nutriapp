using Model;
using Presenter;
using System;

namespace View
{
    public class AlimentoView : IView
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

            consoleView.ScreenSection(0, 1, "Nome do Alimento:");
            consoleView.ScreenSection(19, 1, Model, "NomeAlimento", EnScreenType.eString, 61);
            //
            consoleView.ScreenSection(0, 2, "Calorias........:");
            consoleView.ScreenSection(19, 2, Model, "Calorias", EnScreenType.eString, 4);
            //
            consoleView.ScreenSection(0, 3, "Grupo [1,2,3]...:");
            consoleView.ScreenSection(19, 3, Model, "GrupoAlimentar", EnScreenType.eString, 1);

            consoleView.CreateScreen();
        }

    }
}