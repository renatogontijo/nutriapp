using Model;
using Presenter;
using System;

namespace View
{
    public class MostrarCombinacaoAlimentarView : IView
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

            CombinacaoAlimentar combinacao = (CombinacaoAlimentar)Model;

            consoleView.StartScreen(Presenter);

            consoleView.EnableSearch = true;

            consoleView.ScreenSection(0, 1, "Calorias totais (digite o valor e pressiona F3):");
            consoleView.ScreenSection(50, 1, Model, "Calorias", EnScreenType.eString, 4);

            consoleView.ScreenSection(0, 3, "========== COMBINAÇÕES ==========");

            if (combinacao.CombinacoesAlimentares.Count == 0)
                consoleView.ScreenSection(0, 4, "Vazia!");
            else
            {
                int row = 4;
                foreach(Alimento alimento in combinacao.CombinacoesAlimentares)
                {
                    string str = string.Format("{0}, Grupo {1}, {2} calorias", 
                        alimento.NomeAlimento, 
                        alimento.GrupoAlimentar,
                        alimento.Calorias);
                    consoleView.ScreenSection(0, row++, str);
                    if (row >= 24)
                        break;
                }
            }

            consoleView.CreateScreen();
        }
    }
}