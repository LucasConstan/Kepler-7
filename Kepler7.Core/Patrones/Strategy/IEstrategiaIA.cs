using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Strategy
{
    /// <summary>
    /// STRATEGY - Interfaz de la estrategia. Cada IA decide a su manera qué hace un enemigo en su turno;
    /// el enemigo puede cambiar de estrategia en cualquier momento.
    /// </summary>
    public interface IEstrategiaIA
    {
        string Nombre { get; }
        string Sigla { get; }
        string Descripcion { get; }

        Accion DecidirAccion(Enemigo enemigo, Contexto contexto);
    }
}
