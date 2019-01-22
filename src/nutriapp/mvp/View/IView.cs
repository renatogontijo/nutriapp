using Model;
using Presenter;

namespace View
{
    public interface IView
    {
        void SetPresenter(IPresenter presenter);
        void SetModel(IModel model);
        void Show(ConsoleView consoleView, IModel model);
    }
}