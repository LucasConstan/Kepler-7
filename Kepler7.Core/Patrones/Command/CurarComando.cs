using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Command
{
    // Repara el núcleo de un subsistema pagando créditos. Se usa entre oleadas: los defensores
    // se curan solos al terminar cada una, pero el daño del núcleo queda.
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

        public string Descripcion
        {
            get { return "Reparar núcleo de " + Estacion.Subsistemas[_carril] + " (+" + Reparacion + ", " + Costo + " cr)"; }
        }

        public bool Ejecutar()
        {
            int integridad = _partida.IntegridadDelCarril(_carril);
            if (_partida.Fase != FasePartida.Preparacion)
            {
                return false;
            }

            // Un núcleo caído ya no se repara, y uno sano no lo necesita.
            if (integridad == 0 || integridad == Partida.IntegridadMaxima)
            {
                return false;
            }

            if (!_partida.Estacion.Gastar(Costo))
            {
                return false;
            }

            _integridadAnterior = integridad;
            _partida.RepararNucleo(_carril, Reparacion);
            return true;
        }

        public bool Deshacer()
        {
            // Vuelve al valor guardado en vez de restar 25, porque la reparación pudo haber sido menor si llegó al máximo.
            _partida.RestaurarIntegridad(_carril, _integridadAnterior);
            _partida.Estacion.Ganar(Costo);
            return true;
        }
    }
}
