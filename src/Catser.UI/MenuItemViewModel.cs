using Avalonia.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Catser.UI;

public class MenuItemViewModel
{
    public string Header { get; set; }
    public ICommand Command { get; set; }
    public object CommandParameter { get; set; }
    public ObservableCollection<MenuItemViewModel> Items { get; set; }

    public KeyGesture HotKey { get; set; }
}