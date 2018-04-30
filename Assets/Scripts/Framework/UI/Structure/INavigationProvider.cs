using Framework.UI.Structure.Base;

namespace Framework.UI.Structure
{
    public interface INavigationProvider
    {
        Screen CurrentScreen { get; }
        void OpenScreen<T>() where T : Screen;
    }
}