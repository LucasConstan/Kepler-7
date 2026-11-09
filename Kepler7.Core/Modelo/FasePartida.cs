namespace Kepler7.Core.Modelo
{
    public enum FasePartida
    {
        Preparacion,   // el jugador arma y despliega sus defensas
        Oleada,        // los ticks avanzan y los enemigos atacan
        Victoria,
        Derrota
    }
}
