using Kepler7.Core.Patrones.ChainOfResponsibility;
using Kepler7.Core.Patrones.Visitor;

namespace Kepler7.Core.Modelo
{
    // Las mejoras que se pueden comprar en la tienda del Hub y lo que hace cada una al empezar la partida.
    public static class CatalogoMejoras
    {
        // Estos nombres se guardan en el JSON del progreso. Si se cambian, las mejoras
        // que alguien ya tenía compradas dejan de reconocerse.
        public const string CreditosExtra = "Créditos extra";
        public const string EscudoDeReserva = "Escudo de reserva";
        public const string RefuerzoTripulacion = "Refuerzo de tripulación";

        public const int CantidadCreditosExtra = 100;
        public const int TripulantesDeRefuerzo = 250;

        // En este orden se muestran en la tienda.
        public static readonly Mejora[] Todas =
        {
            new Mejora(CreditosExtra, "Arrancás la próxima partida con " + CantidadCreditosExtra + " créditos más.", 30),
            new Mejora(EscudoDeReserva, "Un módulo de escudo más en el inventario, para sumarlo a la cadena de defensas del héroe.", 50),
            new Mejora(RefuerzoTripulacion, "Cada subsistema arranca con " + TripulantesDeRefuerzo + " tripulantes más protegiendo su núcleo.", 40)
        };

        // Aplica una mejora comprada a la partida que está empezando y devuelve qué pasó, para mostrarlo.
        // Solo usa cosas que la Partida ya tiene públicas, así la Partida no necesita saber que existe una tienda.
        public static string Aplicar(string nombre, Partida partida)
        {
            switch (nombre)
            {
                case CreditosExtra:
                    partida.Estacion.Ganar(CantidadCreditosExtra);
                    return "+" + CantidadCreditosExtra + " créditos para arrancar.";

                case EscudoDeReserva:
                    partida.ObtenerInventario().Agregar(new ItemModuloDefensa(EscudoDeReserva, new DefensaEscudo()));
                    return "hay un " + EscudoDeReserva + " en el inventario del héroe.";

                case RefuerzoTripulacion:
                    for (int carril = 0; carril < Grilla.Carriles; carril++)
                    {
                        partida.Tripulacion.Agregar(carril, TripulantesDeRefuerzo);
                    }
                    return "+" + TripulantesDeRefuerzo + " tripulantes en cada subsistema.";

                default:
                    // Por si en el JSON quedó una mejora que ya no existe en el catálogo.
                    return "la mejora \"" + nombre + "\" ya no existe y se ignoró.";
            }
        }
    }
}
