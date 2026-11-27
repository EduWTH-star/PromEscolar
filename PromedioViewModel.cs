using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PromedioAcademicoApp.Models;

namespace PromedioAcademicoApp.ViewModels;

public class PromedioViewModel : INotifyPropertyChanged
{
    private readonly PromedioModel _model = new();

    private string _calificacion1Texto = string.Empty;
    private string _calificacion2Texto = string.Empty;
    private string _calificacion3Texto = string.Empty;
    private string _promedioTexto = "--";
    private string _nivelTexto = "--";
    private string _mensajeError = string.Empty;

    public string Calificacion1Texto
    {
        get => _calificacion1Texto;
        set { _calificacion1Texto = value; OnPropertyChanged(); }
    }

    public string Calificacion2Texto
    {
        get => _calificacion2Texto;
        set { _calificacion2Texto = value; OnPropertyChanged(); }
    }

    public string Calificacion3Texto
    {
        get => _calificacion3Texto;
        set { _calificacion3Texto = value; OnPropertyChanged(); }
    }

    public string PromedioTexto
    {
        get => _promedioTexto;
        set { _promedioTexto = value; OnPropertyChanged(); }
    }

    public string NivelTexto
    {
        get => _nivelTexto;
        set { _nivelTexto = value; OnPropertyChanged(); }
    }

    public string MensajeError
    {
        get => _mensajeError;
        set
        {
            _mensajeError = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TieneError));
        }
    }

    public bool TieneError => !string.IsNullOrWhiteSpace(MensajeError);

    public ICommand CalcularPromedioCommand { get; }

    public PromedioViewModel()
    {
        CalcularPromedioCommand = new Command(EjecutarCalculo);
    }

    private void EjecutarCalculo()
    {
        if (!TryParseCalificacion(Calificacion1Texto, "Calificación 1", out double c1, out string error1))
        {
            MostrarError(error1);
            return;
        }

        if (!TryParseCalificacion(Calificacion2Texto, "Calificación 2", out double c2, out string error2))
        {
            MostrarError(error2);
            return;
        }

        if (!TryParseCalificacion(Calificacion3Texto, "Calificación 3", out double c3, out string error3))
        {
            MostrarError(error3);
            return;
        }

        _model.Calificacion1 = c1;
        _model.Calificacion2 = c2;
        _model.Calificacion3 = c3;

        double promedioExacto = _model.CalcularPromedio();
        string nivel = _model.DeterminarNivel(promedioExacto);

        MensajeError = string.Empty;
        PromedioTexto = promedioExacto.ToString("0.00", CultureInfo.InvariantCulture);
        NivelTexto = nivel;
    }

    private static bool TryParseCalificacion(string texto, string nombreCampo, out double valor, out string mensajeError)
    {
        valor = 0;
        mensajeError = string.Empty;

        if (string.IsNullOrWhiteSpace(texto))
        {
            mensajeError = $"{nombreCampo} no puede estar vacía.";
            return false;
        }

        string normalizado = texto.Trim().Replace(',', '.');
        if (!double.TryParse(normalizado, NumberStyles.Float, CultureInfo.InvariantCulture, out double numero) ||
            double.IsNaN(numero) || double.IsInfinity(numero))
        {
            mensajeError = $"{nombreCampo} debe ser un número válido.";
            return false;
        }

        if (numero < 0.0 || numero > 10.0)
        {
            mensajeError = $"{nombreCampo} debe estar entre 0 y 10.";
            return false;
        }

        valor = numero;
        return true;
    }

    private void MostrarError(string mensaje)
    {
        MensajeError = mensaje;
        PromedioTexto = "--";
        NivelTexto = "--";
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}