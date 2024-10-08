using Microsoft.Extensions.Configuration;

namespace Catser.UI.ViewModels;

public partial class MainWindowViewModel
{
    private readonly IConfiguration businessService;

    public MainWindowViewModel()
    {
    }

    public MainWindowViewModel(IConfiguration businessService)
    {
        this.businessService = businessService;
    }
}