namespace Kepler7.Core.Patrones.State
{
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
