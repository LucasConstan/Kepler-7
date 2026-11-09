using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Command
{
    // Despliega una unidad de la reserva en el tablero, o mueve un defensor a otra celda.
    public class MoverComando : IComando
    {
        private readonly Partida _partida;
        private readonly Unidad _unidad;
        private readonly int _carril;
        private readonly int _columna;

        // De dónde venía, para poder devolverla al deshacer.
        private readonly bool _veniaDeLaReserva;
        private int _carrilAnterior;
        private int _columnaAnterior;

        public MoverComando(Partida partida, Unidad unidad, int carril, int columna)
        {
            _partida = partida;
            _unidad = unidad;
            _carril = carril;
            _columna = columna;
            _veniaDeLaReserva = partida.Estacion.Reserva.Contains(unidad);
        }

        public string Descripcion
        {
            get
            {
                string accion;
                if (_veniaDeLaReserva)
                {
                    accion = "Desplegar";
                }
                else
                {
                    accion = "Mover";
                }

                // Las columnas arrancan en 0 internamente, pero al jugador se le muestran desde 1.
                int columnaParaMostrar = _columna + 1;
                return accion + " " + _unidad.NombreCompleto + " en " + Estacion.Subsistemas[_carril]
                    + " (col. " + columnaParaMostrar + ")";
            }
        }

        public bool Ejecutar()
        {
            if (_partida.Fase != FasePartida.Preparacion || !_partida.Grilla.EsZonaDefensa(_columna))
            {
                return false;
            }

            if (_veniaDeLaReserva)
            {
                return _partida.Desplegar(_unidad, _carril, _columna);
            }

            _carrilAnterior = _unidad.Carril;
            _columnaAnterior = _unidad.Columna;
            return _partida.Grilla.Mover(_unidad, _carril, _columna);
        }

        public bool Deshacer()
        {
            // Si la unidad ya no está donde la dejó este comando, deshacerlo la movería de un lugar equivocado.
            if (_unidad.Carril != _carril || _unidad.Columna != _columna)
            {
                return false;
            }

            if (_veniaDeLaReserva)
            {
                _partida.Grilla.Quitar(_unidad);
                _partida.Estacion.AgregarAReserva(_unidad);
                return true;
            }

            return _partida.Grilla.Mover(_unidad, _carrilAnterior, _columnaAnterior);
        }
    }
}
