using Model;
using Presenter;
using System;

namespace View
{
    public class ConsultaView : IView
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

            consoleView.ScreenSection(0, 1, "Data............:");
            consoleView.ScreenSection(19, 1, model, "Data", EnScreenType.eString, 10);
            consoleView.ScreenSection(0, 2, "Hora............:");
            consoleView.ScreenSection(19, 2, model, "Hora", EnScreenType.eString, 5);
            consoleView.ScreenSection(0, 3, "Peso............:");
            consoleView.ScreenSection(19, 3, model, "Peso", EnScreenType.eString, 7);
            consoleView.ScreenSection(0, 4, "% Gordura.......:");
            consoleView.ScreenSection(19, 4, model, "PercentualGordura", EnScreenType.eString, 5);
            consoleView.ScreenSection(0, 5, "Sensação Física.:");
            consoleView.ScreenSection(19, 5, model, "SensacaoFisica", EnScreenType.eString, 67);
            consoleView.ScreenSection(0, 6, "Restrições Alimentares:");
            consoleView.ScreenSection(0, 7, "1.");
            consoleView.ScreenSection(3, 7, model, "RestricoesAlimentares01", EnScreenType.eString, 77);
            consoleView.ScreenSection(0, 8, "2.");
            consoleView.ScreenSection(3, 8, model, "RestricoesAlimentares02", EnScreenType.eString, 77);
            consoleView.ScreenSection(0, 9, "3.");
            consoleView.ScreenSection(3, 9, model, "RestricoesAlimentares03", EnScreenType.eString, 77);
            consoleView.ScreenSection(0, 10, "4.");
            consoleView.ScreenSection(3, 10, model, "RestricoesAlimentares04", EnScreenType.eString, 77);
            consoleView.ScreenSection(0, 11, "5.");
            consoleView.ScreenSection(3, 11, model, "RestricoesAlimentares05", EnScreenType.eString, 77);
            consoleView.ScreenSection(0, 12, "6.");
            consoleView.ScreenSection(3, 12, model, "RestricoesAlimentares06", EnScreenType.eString, 77);
            consoleView.ScreenSection(0, 13, "7.");
            consoleView.ScreenSection(3, 13, model, "RestricoesAlimentares07", EnScreenType.eString, 77);
            consoleView.ScreenSection(0, 14, "8.");
            consoleView.ScreenSection(3, 14, model, "RestricoesAlimentares08", EnScreenType.eString, 77);

            consoleView.CreateScreen();
        }
    }
}