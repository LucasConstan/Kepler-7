using Kepler7.Core.Patrones.TemplateMethod;

namespace Kepler7.Core.Modelo
{
    /// <summary>
    /// Lo que reciben los turnos y las estrategias de IA para ver el estado del juego
    /// y ejecutar acciones, sin conocer a la Partida completa.
    /// </summary>
    public class Contexto
    {
        private readonly Partida _partida;

        internal Contexto(Partida partida)
        {
            _partida = partida;
        }

        public int Tick => _partida.Tick;
        public Grilla Grilla => _partida.Grilla;
        public Random Azar => _partida.Azar;

        public void Registrar(Unidad unidad, EtapaTurno etapa, string texto)
        {
            _partida.Historial.Registrar(Tick, unidad.NombreCompleto, etapa, texto);
        }

        public int AplicarDanio(Unidad objetivo, int cantidad)
        {
            int real = objetivo.RecibirDanio(cantidad);
            if (!objetivo.EstaViva) _partida.Matar(objetivo);
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
                    if (actor is not Enemigo enemigo) return "no puede avanzar";
                    if (enemigo.Columna == 0)
                    {
                        _partida.GolpearNucleo(enemigo);
                        return "llega al núcleo y lo golpea";
                    }
                    return Mover(actor, actor.Carril, actor.Columna - 1) ? "avanza una casilla" : "no puede avanzar";

                case TipoAccion.CambiarCarril:
                    return Mover(actor, accion.Carril, actor.Columna)
                        ? "cambia al carril de " + Estacion.Subsistemas[accion.Carril]
                        : "no puede cambiar de carril";

                case TipoAccion.Esperar:
                    return "espera";

                default:
                    return "no sabe cómo hacer eso";
            }
        }

        private string Atacar(Unidad atacante, Unidad? objetivo)
        {
            if (objetivo == null || !objetivo.EstaViva) return "no hay a quién atacar";

            int real = AplicarDanio(objetivo, atacante.DanioTotal);
            string texto = $"ataca a {objetivo.NombreCompleto}: -{real} PV";
            return objetivo.EstaViva ? texto + $" (le quedan {objetivo.Vida})" : texto + " (derrotado)";
        }

        private bool Mover(Unidad unidad, int carril, int columna)
        {
            return Grilla.Mover(unidad, carril, columna);
        }
    }
}
