using Model;
using View;
using System;

namespace Presenter
{
    public interface IPresenter
    {
        string Name { get; }
        IPresenter ParentPresenter { get; set; }
        IModel Model { get; set; }
        IView View { get; set; }

        void SetParentPresenter(IPresenter presenter);
        void SetView(IView view, IModel model);
        bool Save(out string msgErr);
        void Search(ConsoleView consoleView);

        void SelectedItem(ConsoleView consoleView, int position);
        void Show(ConsoleView consoleView);
    }
}