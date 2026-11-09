namespace Kepler7.Core.Modelo
{
    // Lo que una unidad decide hacer en su turno. La devuelve el paso "Decidir" del turno
    // (y, en los enemigos, la estrategia de IA).
    public class Accion
    {
        public TipoAccion Tipo { get; }
        public Unidad? Objetivo { get; }
        public int Carril { get; }
        public string Motivo { get; }

        // El constructor es privado para que cada acción se cree con su método,
        // y así solo se carguen los datos que le corresponden a cada tipo.
        private Accion(TipoAccion tipo, Unidad? objetivo, int carril, string motivo)
        {
            Tipo = tipo;
            Objetivo = objetivo;
            Carril = carril;
            Motivo = motivo;
        }

        public static Accion Esperar(string motivo)
        {
            return new Accion(TipoAccion.Esperar, null, -1, motivo);
        }

        public static Accion Avanzar(string motivo)
        {
            return new Accion(TipoAccion.Avanzar, null, -1, motivo);
        }

        public static Accion Potenciar(string motivo)
        {
            return new Accion(TipoAccion.Potenciar, null, -1, motivo);
        }

        public static Accion Atacar(Unidad objetivo, string motivo)
        {
            return new Accion(TipoAccion.Atacar, objetivo, -1, motivo);
        }

        public static Accion Curar(Unidad objetivo, string motivo)
        {
            return new Accion(TipoAccion.Curar, objetivo, -1, motivo);
        }

        public static Accion CambiarCarril(int carril, string motivo)
        {
            return new Accion(TipoAccion.CambiarCarril, null, carril, motivo);
        }

        public override string ToString()
        {
            string detalle = Tipo.ToString();
            if (Objetivo != null)
            {
                detalle = detalle + " a " + Objetivo.NombreCompleto;
            }

            if (Tipo == TipoAccion.CambiarCarril)
            {
                detalle = detalle + " hacia " + Estacion.Subsistemas[Carril];
            }

            return detalle + " (" + Motivo + ")";
        }
    }
}
