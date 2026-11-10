using System.Collections.Generic;

namespace Kepler7.Core.Patrones.ChainOfResponsibility
{
    // Arma la cadena de defensas y deja cambiar su orden. El golpe siempre entra por la primera de la lista.
    public class CadenaDefensas
    {
        private List<Defensa> _defensas = new List<Defensa>();

        public List<Defensa> ObtenerDefensas()
        {
            return _defensas;
        }

        // Deja la cadena sin defensas. Se usa al retomar una partida guardada, para rearmarla en su orden.
        public void Vaciar()
        {
            _defensas.Clear();
        }

        public void Agregar(Defensa defensa)
        {
            _defensas.Add(defensa);
            Enlazar();
        }

        public void Subir(int posicion)
        {
            if (posicion > 0)
            {
                Intercambiar(posicion, posicion - 1);
            }
        }

        public void Bajar(int posicion)
        {
            if (posicion >= 0 && posicion < _defensas.Count - 1)
            {
                Intercambiar(posicion, posicion + 1);
            }
        }

        public void Procesar(Golpe golpe)
        {
            if (_defensas.Count > 0)
            {
                _defensas[0].Procesar(golpe);
            }
        }

        public void Recargar()
        {
            for (int i = 0; i < _defensas.Count; i++)
            {
                _defensas[i].Recargar();
            }
        }

        private void Intercambiar(int a, int b)
        {
            Defensa auxiliar = _defensas[a];
            _defensas[a] = _defensas[b];
            _defensas[b] = auxiliar;
            Enlazar();
        }

        // Cada vez que cambia la lista hay que volver a decirle a cada defensa cuál es la siguiente.
        // La última queda sin siguiente, y ahí termina la cadena.
        private void Enlazar()
        {
            for (int i = 0; i < _defensas.Count; i++)
            {
                if (i < _defensas.Count - 1)
                {
                    _defensas[i].EstablecerSiguiente(_defensas[i + 1]);
                }
                else
                {
                    _defensas[i].EstablecerSiguiente(null);
                }
            }
        }
    }
}
