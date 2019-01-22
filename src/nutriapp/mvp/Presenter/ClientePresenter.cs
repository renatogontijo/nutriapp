using Model;
using View;
using System;
using System.Collections.Generic;

namespace Presenter
{
    public class ClientePresenter : IPresenter
    {
        // Dados em memória
        private static IDictionary<string, IModel> m_dataSet = new Dictionary<string, IModel>();
        public static int SelectedItemValue = -1;
        private const string CO_NAME = "Cadastro de cliente";

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

            Cliente model = (Cliente)Model;

            if (string.IsNullOrEmpty(model.Nome))
                msgErr += "Campo Nome deve ser preenchido.";
            else if (model.Nome.Length < 4)
                msgErr += "Campo Nome deve ter no mínimo 3 caracteres.";
            else if (string.IsNullOrEmpty(model.Endereco))
                msgErr += "Campo Endereço deve ser preenchido.";
            else if (model.Endereco.Length < 4)
                msgErr += "Campo Endereco deve ter no mínimo 3 caracteres.";
            else if (string.IsNullOrEmpty(model.Telefone))
                msgErr += "Campo Telefone deve ser preenchido.";
            else if (model.Telefone.Length < 8)
                msgErr += "Campo Endereco deve ter no mínimo 8 caracteres.";
            else if (string.IsNullOrEmpty(model.Email))
                msgErr += "Campo Email deve ser preenchido.";
            else if (model.Email.IndexOf('@') < 0)
                msgErr += "Campo Email inválido.";

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

            CreateActions(consoleView);

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
        }

        public void Show(ConsoleView consoleView)
        {
            CreateActions(consoleView);

            View.Show(consoleView, Model);
        }
    }
}