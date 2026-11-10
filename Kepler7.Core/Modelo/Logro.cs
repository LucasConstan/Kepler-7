namespace Kepler7.Core.Modelo
{
    // Un logro del catálogo. En el progreso solo se guarda el nombre;
    // el patrón y la pista sirven para mostrarlo en el panel del Hub.
    public class Logro
    {
        public string Patron { get; }
        public string Nombre { get; }
        public string ComoSeConsigue { get; }

        public Logro(string patron, string nombre, string comoSeConsigue)
        {
            Patron = patron;
            Nombre = nombre;
            ComoSeConsigue = comoSeConsigue;
        }
    }
}
