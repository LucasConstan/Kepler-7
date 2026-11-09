using System;
using Kepler7.Core.Patrones.TemplateMethod;

namespace Kepler7.Core.Modelo
{
    // Es lo que reciben los turnos y las estrategias de IA para ver el estado del juego
    // y ejecutar acciones, sin tener acceso a la Partida completa.
    public class Contexto
    {
        private readonly Partida _partida;

        internal Contexto(Partida partida)
        {
            _partida = partida;
        }

        public int Tick
        {
            get { return _partida.Tick; }
        }

        public Grilla Grilla
        {
            get { return _partida.Grilla; }
        }

        public Random Azar
        {
            get { return _partida.Azar; }
        }

        public void Registrar(Unidad unidad, EtapaTurno etapa, string texto)
        {
            _partida.Historial.Registrar(Tick, unidad.NombreCompleto, etapa, texto);
        }

        public int AplicarDanio(Unidad objetivo, int cantidad)
        {
            int real = objetivo.RecibirDanio(cantidad);
            if (!objetivo.EstaViva)
            {
                _partida.Matar(objetivo);
            }

            return real;
        }

        public int Curar(Unidad objetivo, int cantidad)
        {
            return objetivo.Curar(cantidad);
        }

        // Ejecuta las acciones comunes a todos (atacar, avanzar, cambiar de carril, esperar).
        // Las acciones propias de un rol (curar, potenciar) las resuelve el turno de ese rol.
        public string Ejecutar(Unidad actor, Accion accion)
        {
            switch (accion.Tipo)
            {
                case TipoAccion.Atacar:
                    return Atacar(actor, accion.Objetivo);

                case TipoAccion.Avanzar:
                    return Avanzar(actor);

                case TipoAccion.CambiarCarril:
                    return CambiarCarril(actor, accion.Carril);

                case TipoAccion.Esperar:
                    return "espera";

                default:
                    return "no sabe cómo hacer eso";
            }
        }

        private string Avanzar(Unidad actor)
        {
            if (!(actor is Enemigo))
            {
                return "no puede avanzar";
            }

            // Desde la columna 0 ya no hay a dónde moverse: lo que sigue es el núcleo.
            Enemigo enemigo = (Enemigo)actor;
            if (enemigo.Columna == 0)
            {
                _partida.GolpearNucleo(enemigo);
                return "llega al núcleo y lo golpea";
            }

            if (Mover(actor, actor.Carril, actor.Columna - 1))
            {
                return "avanza una casilla";
            }

            return "no puede avanzar";
        }

        private string CambiarCarril(Unidad actor, int carril)
        {
            if (Mover(actor, carril, actor.Columna))
            {
                return "cambia al carril de " + Estacion.Subsistemas[carril];
            }

            return "no puede cambiar de carril";
        }

        private string Atacar(Unidad atacante, Unidad? objetivo)
        {
            // El objetivo se eligió al decidir, pero pudo haber muerto antes de que llegue este momento.
            if (objetivo == null || !objetivo.EstaViva)
            {
                return "no hay a quién atacar";
            }

            int real = AplicarDanio(objetivo, atacante.DanioTotal);
            string texto = "ataca a " + objetivo.NombreCompleto + ": -" + real + " PV";

            if (objetivo.EstaViva)
            {
                return texto + " (le quedan " + objetivo.Vida + ")";
            }

            return texto + " (derrotado)";
        }

        private bool Mover(Unidad unidad, int carril, int columna)
        {
            return Grilla.Mover(unidad, carril, columna);
        }
    }
}
