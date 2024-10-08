using ReactiveUI;

namespace Catser.UI;

public abstract class ViewModelBase : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator => new ViewModelActivator();
}
