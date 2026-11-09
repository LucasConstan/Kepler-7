using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Strategy
{
    // Patrón Strategy: cada IA decide a su manera qué hace un enemigo en su turno.
    // Como el enemigo guarda esta interfaz y no una clase concreta, se le puede cambiar la IA en plena partida.
    public interface IEstrategiaIA
    {
        string Nombre { get; }
        string Sigla { get; }
        string Descripcion { get; }

        Accion DecidirAccion(Enemigo enemigo, Contexto contexto);
    }
}
