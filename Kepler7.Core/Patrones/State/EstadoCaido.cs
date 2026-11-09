namespace Kepler7.Core.Patrones.State
{
    // No juega ni pega. Desde este estado no se pasa a otro solo: lo saca Restaurar al terminar la oleada.
    public class EstadoCaido : IEstadoHeroe
    {
        public string ObtenerNombre()
        {
            return "Caído";
        }

        public bool PuedeActuar()
        {
            return false;
        }

        public int ModificarDanio(int danioBase)
        {
            return 0;
        }

        public void AlIniciarTurno(Heroe heroe)
        {
        }

        public void AlRecibirDanio(Heroe heroe, int danioRecibido)
        {
        }
    }
}
