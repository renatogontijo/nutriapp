using Model;
using View;
using System;

namespace Presenter
{
    public class MainPresenter : IPresenter
    {
        private const string CO_NAME = "MENU PRINCIPAL";

        public string Name { get { return CO_NAME; } }

        public ConsoleView ConsoleView { get; set; }
        public IPresenter ParentPresenter { get; set; }
        public IModel Model { get; set; }
        public IView View { get; set; }

        public bool Save(out string msgErr)
        {
            msgErr = null;
            return false;
        }
        public void SetConsoleView(ConsoleView consoleView)
        {
            ConsoleView = consoleView;
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

        private void CreateActions(ConsoleView consoleView)
        {
            consoleView.StartAction();

            // Alimento
            AlimentoPresenter alimentoPresenter = new AlimentoPresenter();
            alimentoPresenter.SetParentPresenter(this);
            alimentoPresenter.SetView(new AlimentoView(), new Alimento());
            consoleView.CreateAction(ConsoleKey.F1, alimentoPresenter);

            // Cliente
            ClientePresenter clientePresenter = new ClientePresenter();
            clientePresenter.SetParentPresenter(this);
            clientePresenter.SetView(new ClienteView(), new Cliente());
            consoleView.CreateAction(ConsoleKey.F2, clientePresenter);

            // Consulta
            ConsultaPresenter consultaPresenter = new ConsultaPresenter();
            consultaPresenter.SetParentPresenter(this);
            consultaPresenter.SetView(new ConsultaView(), new Consulta());
            consoleView.CreateAction(ConsoleKey.F3, consultaPresenter);
        }

        public void SelectedItem(ConsoleView consoleView, int position)
        {
            
        }

        public void Search(ConsoleView consoleView)
        {

        }

        public void Show(ConsoleView consoleView)
        {
            CreateActions(consoleView);

            View.Show(consoleView, Model);
        }
    }
}