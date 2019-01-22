using Model;
using Presenter;
using View;
using System;
using System.Collections.Generic;

namespace Presenter
{
    public class AlimentoPresenter : IPresenter
    {
        // Dados em memória
        private static IDictionary<string, IModel> m_dataSet = new Dictionary<string, IModel>();

        private const string CO_NAME = "Cadastro de alimento";

        public string Name { get { return CO_NAME; } }

        public ConsoleView ConsoleView { get; set; }
        public IPresenter ParentPresenter { get; set; }
        public IModel Model { get; set; }
        public IView View { get; set; }

        public AlimentoPresenter()
        {
            LoadDataSet();
        }

        public void SetParentPresenter(IPresenter presenter)
        {
            ParentPresenter = presenter;
        }

        public void SetView(IView view, IModel model)
        {
            Model = model;
            View = view;
            view.SetModel(model);
            view.SetPresenter(this);
        }
        
        private bool ValidateModel(out string msgErr)
        {
            msgErr = null;
            int resInt;

            Alimento model = (Alimento)Model;

            if (string.IsNullOrEmpty(model.NomeAlimento))
                msgErr += "Campo Nome do alimento deve ser preenchido.";
            else if(model.NomeAlimento.Length < 4)
                msgErr += "Campo Nome do alimento deve ter no mínimo 3 caracteres.";
            else if(string.IsNullOrEmpty(model.Calorias))
                msgErr += "Campo Calorias deve ser preenchido.";
            else if(!int.TryParse(model.Calorias, out resInt))
                msgErr += "Campo Calorias deve ser um valor numérico inteiro.";
            else if(string.IsNullOrEmpty(model.GrupoAlimentar))
                msgErr += "Campo Grupo Alimentar deve ser preenchido.";
            else if(!int.TryParse(model.GrupoAlimentar, out resInt) && (resInt < 1 || resInt > 3))
                msgErr += "Campo Grupo Alimentar deve ser classificado em 1, 2 ou 3.";
            
            return (string.IsNullOrEmpty(msgErr));
        }

        public bool Save(out string msgErr)
        {
            if (!ValidateModel(out msgErr))
                return false;
            
            Alimento model = (Alimento)Model;
            if (m_dataSet.ContainsKey(model.NomeAlimento))
                m_dataSet[model.NomeAlimento] = Model;
            else
                m_dataSet.Add(model.NomeAlimento, Model);

            return true;
        }

        private void LoadDataSet()
        {
            if (m_dataSet.Count > 0)
                return;

            Alimento a1 = new Alimento() { NomeAlimento = "Maça", Calorias = "2", GrupoAlimentar = "1"};
            Alimento a2 = new Alimento() { NomeAlimento = "Jaca", Calorias = "200", GrupoAlimentar = "3"};
            Alimento a3 = new Alimento() { NomeAlimento = "Kiwi", Calorias = "80", GrupoAlimentar = "2"};
            m_dataSet.Add(a1.NomeAlimento, a1);
            m_dataSet.Add(a2.NomeAlimento, a2);
            m_dataSet.Add(a3.NomeAlimento, a3);
        }

        private void CreateActions(ConsoleView consoleView)
        {
            consoleView.StartAction();
        }

        public void SelectedItem(ConsoleView consoleView, int position)
        {
            int pos = 0;
            bool selected = false;

            foreach(KeyValuePair<string, IModel> item in m_dataSet)
            {
                Alimento c = (Alimento)item.Value;
                if (++pos == position)
                {
                    selected = true;
                    Model = c;
                    break;
                }
            }

            if (selected)
                Show(consoleView);
        }

        public void Search(ConsoleView consoleView)
        {
            int pos = 0;

            CreateActions(consoleView);

            consoleView.StartScreen(this);

            consoleView.EnableSelectItem = true;

            foreach(KeyValuePair<string, IModel> item in m_dataSet)
            {
                Alimento c = (Alimento)item.Value;
                consoleView.ScreenSection(0, ++pos, string.Format("{0,2}.{1}", pos, c.NomeAlimento));

                if (pos >= 23)
                    break;
            }

            consoleView.ScreenSection(0, 24, "Selecione a item e pressione F5: ");
            consoleView.ScreenSection(32, 24, this, "SelectedItemValue", EnScreenType.eNumberInt, 2);
            
            consoleView.CreateScreen();
        }

        public void Show(ConsoleView consoleView)
        {
            CreateActions(consoleView);

            View.Show(consoleView, Model);
        }
    }
}