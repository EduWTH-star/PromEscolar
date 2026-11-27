using PromedioAcademicoApp.ViewModels;

namespace PromedioAcademicoApp.Views;

public partial class PromedioView : ContentPage
{
    public PromedioView()
    {
        InitializeComponent();
        BindingContext = new PromedioViewModel();
    }
}