namespace Kepler7.Core.Modelo
{
    // Un enemigo que aparece en un tick determinado de la oleada.
    public class SpawnEnemigo
    {
        public int Tick { get; }
        public int Carril { get; }
        public TipoEnemigo Tipo { get; }

        public SpawnEnemigo(int tick, int carril, TipoEnemigo tipo)
        {
            Tick = tick;
            Carril = carril;
            Tipo = tipo;
        }
    }
}
