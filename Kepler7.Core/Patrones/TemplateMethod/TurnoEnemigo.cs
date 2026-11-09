using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.TemplateMethod
{
    // Acá se juntan los dos patrones: el turno sigue la plantilla de TurnoUnidad,
    // pero el paso de decidir se lo pregunta a la estrategia de IA del enemigo.
    // Los casteos a Enemigo son seguros porque este turno solo lo usa esa clase.
    public class TurnoEnemigo : TurnoUnidad
    {
        protected override string Inicio(Unidad unidad, Contexto contexto)
        {
            Enemigo enemigo = (Enemigo)unidad;
            return "empieza su turno (IA " + enemigo.Estrategia.Nombre + ", " + enemigo.Vida + "/" + enemigo.VidaMaxima + " PV)";
        }

        protected override Accion Decidir(Unidad unidad, Contexto contexto)
        {
            Enemigo enemigo = (Enemigo)unidad;
            return enemigo.Estrategia.DecidirAccion(enemigo, contexto);
        }

        protected override string Actuar(Unidad unidad, Accion accion, Contexto contexto)
        {
            Enemigo enemigo = (Enemigo)unidad;

            // Los enemigos lentos (la Criatura) solo avanzan cada cierta cantidad de ticks.
            if (accion.Tipo == TipoAccion.Avanzar && !enemigo.PuedeAvanzarEnTick(contexto.Tick))
            {
                return "es lento, no se mueve en este tick";
            }

            return base.Actuar(unidad, accion, contexto);
        }
    }
}
