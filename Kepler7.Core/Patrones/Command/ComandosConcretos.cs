using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Command
{
    /// <summary>
    /// COMMAND concreto - Construir: paga una unidad nueva y la manda a la reserva.
    /// La unidad ya viene armada por el Astillero (Builder) o el Laboratorio (Prototype).
    /// Deshacer la saca de la reserva y devuelve los créditos.
    /// </summary>
    public class ConstruirComando : IComando
    {
        private readonly Estacion _estacion;
        private readonly Unidad _unidad;

        public ConstruirComando(Estacion estacion, Unidad unidad)
        {
            _estacion = estacion;
            _unidad = unidad;
        }

        public string Descripcion => $"Construir {_unidad.NombreCompleto} ({_unidad.Costo} cr)";

        public bool Ejecutar()
        {
            if (!_estacion.Gastar(_unidad.Costo)) return false;
            _estacion.AgregarAReserva(_unidad);
            return true;
        }

        public bool Deshacer()
        {
            // Si ya se desplegó, primero hay que deshacer el despliegue (la pila lo garantiza).
            if (!_estacion.QuitarDeReserva(_unidad)) return false;
            _estacion.Ganar(_unidad.Costo);
            return true;
        }
    }

    /// <summary>
    /// COMMAND concreto - Mover: despliega una unidad de la reserva en el tablero, o mueve
    /// un defensor a otra celda. Guarda de dónde venía para poder devolverlo.
    /// </summary>
    public class MoverComando : IComando
    {
        private readonly Partida _partida;
        private readonly Unidad _unidad;
        private readonly int _carril;
        private readonly int _columna;

        // De dónde venía (para deshacer).
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

        public string Descripcion =>
            $"{(_veniaDeLaReserva ? "Desplegar" : "Mover")} {_unidad.NombreCompleto} en {Estacion.Subsistemas[_carril]} (col. {_columna + 1})";

        public bool Ejecutar()
        {
            if (_partida.Fase != FasePartida.Preparacion || !_partida.Grilla.EsZonaDefensa(_columna)) return false;

            if (_veniaDeLaReserva)
                return _partida.Desplegar(_unidad, _carril, _columna);

            _carrilAnterior = _unidad.Carril;
            _columnaAnterior = _unidad.Columna;
            return _partida.Grilla.Mover(_unidad, _carril, _columna);
        }

        public bool Deshacer()
        {
            if (_unidad.Carril != _carril || _unidad.Columna != _columna) return false;

            if (_veniaDeLaReserva)
            {
                _partida.Grilla.Quitar(_unidad);
                _partida.Estacion.AgregarAReserva(_unidad);
                return true;
            }
            return _partida.Grilla.Mover(_unidad, _carrilAnterior, _columnaAnterior);
        }
    }

    /// <summary>
    /// COMMAND concreto - Curar: repara el núcleo de un subsistema pagando créditos.
    /// Se usa entre oleadas (los defensores ya se curan solos al terminar cada oleada,
    /// pero el daño del núcleo queda). Guarda la integridad anterior para deshacer.
    /// </summary>
    public class CurarComando : IComando
    {
        public const int Costo = 60;
        public const int Reparacion = 25;

        private readonly Partida _partida;
        private readonly int _carril;
        private int _integridadAnterior;

        public CurarComando(Partida partida, int carril)
        {
            _partida = partida;
            _carril = carril;
        }

        public string Descripcion => $"Reparar núcleo de {Estacion.Subsistemas[_carril]} (+{Reparacion}, {Costo} cr)";

        public bool Ejecutar()
        {
            int integridad = _partida.IntegridadDelCarril(_carril);
            if (_partida.Fase != FasePartida.Preparacion) return false;
            if (integridad == 0 || integridad == Partida.IntegridadMaxima) return false; // caído o sano
            if (!_partida.Estacion.Gastar(Costo)) return false;

            _integridadAnterior = integridad;
            _partida.RepararNucleo(_carril, Reparacion);
            return true;
        }

        public bool Deshacer()
        {
            _partida.RestaurarIntegridad(_carril, _integridadAnterior);
            _partida.Estacion.Ganar(Costo);
            return true;
        }
    }

    /// <summary>
    /// COMMAND concreto - Atacar: disparo orbital de la estación contra el enemigo elegido,
    /// durante la oleada. Deshacer le devuelve la vida y reintegra los créditos,
    /// salvo que el enemigo ya haya sido destruido.
    /// </summary>
    public class AtacarComando : IComando
    {
        public const int Costo = 40;
        public const int Danio = 30;

        private readonly Partida _partida;
        private readonly Enemigo _objetivo;
        private int _danioCausado;

        public AtacarComando(Partida partida, Enemigo objetivo)
        {
            _partida = partida;
            _objetivo = objetivo;
        }

        public string Descripcion => $"Disparo orbital a {_objetivo.NombreCompleto} (-{Danio} PV, {Costo} cr)";

        public bool Ejecutar()
        {
            if (_partida.Fase != FasePartida.Oleada || !_objetivo.EstaViva || _objetivo.Carril < 0) return false;
            if (!_partida.Estacion.Gastar(Costo)) return false;

            _danioCausado = _objetivo.RecibirDanio(Danio);
            if (!_objetivo.EstaViva) _partida.Matar(_objetivo);
            return true;
        }

        public bool Deshacer()
        {
            if (!_objetivo.EstaViva || _objetivo.Carril < 0) return false; // ya fue destruido

            _objetivo.Vida += _danioCausado;
            _partida.Estacion.Ganar(Costo);
            return true;
        }
    }
}
