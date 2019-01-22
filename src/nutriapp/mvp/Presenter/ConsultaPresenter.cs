using Model;
using View;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Presenter
{
    public class ConsultaPresenter : IPresenter
    {
        // Dados em memória
        private static IDictionary<string, IModel> m_dataSet = new Dictionary<string, IModel>();
        public static int SelectedItemValue = -1;
        private const string CO_NAME = "Consulta";

        public string Name { get { return CO_NAME; } }
        public IPresenter ParentPresenter { get; set; }
        public IModel Model { get; set; }
        public IView View { get; set; }

        public void SetParentPresenter(IPresenter presenter)
        {
            ParentPresenter = presenter;
        }

        public void SetView(IView view, IModel model)
        {
            view.SetPresenter(this);
            view.SetModel(model);
            View = view;
            Model = model;
        }

        private bool ValidateModel(out string msgErr)
        {
            msgErr = null;

            Consulta model = (Consulta)Model;

            if (string.IsNullOrEmpty(model.Data))
                msgErr += "Campo Data deve ser preenchido.";
            else if (!Regex.IsMatch(model.Data, "##/##/####"))
                msgErr += "Campo Data deve estar no formado dd/MM/yyyy.";
            else if (string.IsNullOrEmpty(model.Hora))
                msgErr += "Campo Hora deve ser preenchido.";
            else if (!Regex.IsMatch(model.Hora, "##:##"))
                msgErr += "Campo Hora deve estar no formato HH:mm.";
            else if (string.IsNullOrEmpty(model.Peso))
                msgErr += "Campo Peso deve ser preenchido.";
            else if (Regex.IsMatch(model.Peso, "\\d{2,7}"))
                msgErr += "Campo Peso deve ter no mínimo 2 caracteres numéricos.";
            else if (string.IsNullOrEmpty(model.PercentualGordura))
                msgErr += "Campo PercentualGordura deve ser preenchido.";
            else if (Regex.IsMatch(model.PercentualGordura, "\\d{2,3}"))
                msgErr += "Campo PercentualGordura não pode ultrapassar 3 dígitos numéricos.";

            return (string.IsNullOrEmpty(msgErr));
        }
        public bool Save(out string msgErr)
        {
            if (!ValidateModel(out msgErr))
                return false;

            Cliente model = (Cliente)Model;
            if (m_dataSet.ContainsKey(model.Nome))
                m_dataSet[model.Nome] = Model;
            else
                m_dataSet.Add(model.Nome, Model);

            return true;
        }

        public void SelectedItem(ConsoleView consoleView, int position)
        {
            
        }

        public void Search(ConsoleView consoleView)
        {
            int pos = 0;

            consoleView.StartScreen(this);

            consoleView.EnableSelectItem = true;

            foreach(KeyValuePair<string, IModel> item in m_dataSet)
            {
                Cliente c = (Cliente)item.Value;
                consoleView.ScreenSection(0, ++pos, string.Format("{0,2}.{1}", pos, c.Nome));

                if (pos >= 24)
                    break;
            }

            consoleView.ScreenSection(0, 25, "Selecione a item e pressione F5: ");
            consoleView.ScreenSection(32, 25, this, "SelectedItemValue", EnScreenType.eNumberInt, 2);
            
            consoleView.CreateScreen();
        }

        private void CreateActions(ConsoleView consoleView)
        {
            consoleView.StartAction();

            // GerarCombinacaoAlimentar
            GerarCombinacaoAlimentarPresenter combAlimPresenter = new GerarCombinacaoAlimentarPresenter();
            combAlimPresenter.SetView(new GerarCombinacaoAlimentarView(), new CombinacaoAlimentar());
            combAlimPresenter.SetParentPresenter(this);
            consoleView.CreateAction(ConsoleKey.F6, combAlimPresenter, "Gerar Combinação Alimentar");
        }

        public void Show(ConsoleView consoleView)
        {
            CreateActions(consoleView);

            View.Show(consoleView, Model);
        }
    }
}