namespace PromedioAcademicoApp.Models;

public class PromModel
{
    public double Calificacion1 { get; set; }
    public double Calificacion2 { get; set; }
    public double Calificacion3 { get; set; }

    public double CalcularPromedio()
    {
        return (Calificacion1 + Calificacion2 + Calificacion3) / 3.0;
    }

    public string DeterminarNivel(double promedio)
    {
        if (promedio < 7.0)
            return "No acreditado";
        if (promedio < 8.0)
            return "Suficiente";
        if (promedio < 9.0)
            return "Bueno";

        return "Excelente";
    }
}
