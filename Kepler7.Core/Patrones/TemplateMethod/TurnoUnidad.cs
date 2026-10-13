using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.TemplateMethod
{
    /// <summary>
    /// TEMPLATE METHOD — Clase abstracta. EjecutarTurno() define el orden fijo del turno
    /// (inicio, decisión, acción, fin) y NO se puede redefinir. Las subclases solo cambian
    /// los pasos variables: cada rol decide y actúa a su manera.
    /// </summary>
    public abstract class TurnoUnidad
    {
        // Las unidades con número menor juegan primero dentro de un tick.
        public virtual int Prioridad => 1;

        // El método plantilla: no es virtual, así el orden nunca cambia.
        public void EjecutarTurno(Unidad unidad, Contexto contexto)
        {
            contexto.Registrar(unidad, EtapaTurno.Inicio, Inicio(unidad, contexto));

            if (!unidad.PuedeActuar)
            {
                contexto.Registrar(unidad, EtapaTurno.Fin, "no puede actuar, pierde el turno");
                return;
            }

            Accion accion = Decidir(unidad, contexto);
            contexto.Registrar(unidad, EtapaTurno.Decision, accion.ToString());

            string resultado = Actuar(unidad, accion, contexto);
            contexto.Registrar(unidad, EtapaTurno.Accion, resultado);

            contexto.Registrar(unidad, EtapaTurno.Fin, Fin(unidad, contexto));
        }

        // Pasos con comportamiento por defecto (las subclases pueden cambiarlos).
        protected virtual string Inicio(Unidad unidad, Contexto contexto)
        {
            return $"empieza su turno ({unidad.Vida}/{unidad.VidaMaxima} PV)";
        }

        protected virtual string Actuar(Unidad unidad, Accion accion, Contexto contexto)
        {
            return contexto.Ejecutar(unidad, accion);
        }

        protected virtual string Fin(Unidad unidad, Contexto contexto)
        {
            return "termina su turno";
        }

        // Paso obligatorio: cada subclase decide a su manera.
        protected abstract Accion Decidir(Unidad unidad, Contexto contexto);
    }
}
