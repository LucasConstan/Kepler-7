namespace Kepler7.Core.Patrones.Flyweight
{
    // Guarda solo lo que es propio de cada tripulante (estado extrínseco) y una referencia a su especie, que es compartida.
    // Se llama así para no chocar con el Tripulante del Prototype.
    public class TripulanteEstacion
    {
        public int Identificador { get; }
        public string Nombre { get; }
        public string Rango { get; }
        public int Carril { get; }
        public IEspecie Especie { get; }

        public TripulanteEstacion(int identificador, string nombre, string rango, int carril, IEspecie especie)
        {
            Identificador = identificador;
            Nombre = nombre;
            Rango = rango;
            Carril = carril;
            Especie = especie;
        }

        public override string ToString()
        {
            return Nombre + " (" + Rango + ") · " + Especie.Nombre + " de " + Especie.PlanetaOrigen + ", dieta " + Especie.Dieta;
        }
    }
}
