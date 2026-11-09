using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Command
{
    // Paga una unidad nueva y la manda a la reserva. La unidad ya viene armada
    // por el Astillero (Builder) o por el Laboratorio (Prototype).
    public class ConstruirComando : IComando
    {
        private readonly Estacion _estacion;
        private readonly Unidad _unidad;

        public ConstruirComando(Estacion estacion, Unidad unidad)
        {
            _estacion = estacion;
            _unidad = unidad;
        }

        public string Descripcion
        {
            get { return "Construir " + _unidad.NombreCompleto + " (" + _unidad.Costo + " cr)"; }
        }

        public bool Ejecutar()
        {
            if (!_estacion.Gastar(_unidad.Costo))
            {
                return false;
            }

            _estacion.AgregarAReserva(_unidad);
            return true;
        }

        public bool Deshacer()
        {
            // Si ya se desplegó, primero hay que deshacer el despliegue (la pila lo garantiza).
            if (!_estacion.QuitarDeReserva(_unidad))
            {
                return false;
            }

            _estacion.Ganar(_unidad.Costo);
            return true;
        }
    }
}
