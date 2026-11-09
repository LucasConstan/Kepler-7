using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Command
{
    // Disparo orbital de la estación contra el enemigo elegido, durante la oleada.
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

        public string Descripcion
        {
            get { return "Disparo orbital a " + _objetivo.NombreCompleto + " (-" + Danio + " PV, " + Costo + " cr)"; }
        }

        public bool Ejecutar()
        {
            if (_partida.Fase != FasePartida.Oleada || !_objetivo.EstaViva || _objetivo.Carril < 0)
            {
                return false;
            }

            if (!_partida.Estacion.Gastar(Costo))
            {
                return false;
            }

            _danioCausado = _objetivo.RecibirDanio(Danio);
            if (!_objetivo.EstaViva)
            {
                _partida.Matar(_objetivo);
            }

            return true;
        }

        public bool Deshacer()
        {
            // A un enemigo que ya fue destruido no se le puede devolver la vida.
            if (!_objetivo.EstaViva || _objetivo.Carril < 0)
            {
                return false;
            }

            _objetivo.Vida = _objetivo.Vida + _danioCausado;
            _partida.Estacion.Ganar(Costo);
            return true;
        }
    }
}
