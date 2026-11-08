namespace Kepler7.Core.Patrones.ChainOfResponsibility
{
    /// <summary>
    /// CHAIN OF RESPONSIBILITY - Arma la cadena y permite cambiar su orden.
    /// El golpe siempre entra por la primera defensa de la lista.
    /// </summary>
    public class CadenaDefensas
    {
        private List<Defensa> _defensas = new List<Defensa>();

        public List<Defensa> ObtenerDefensas()
        {
            return _defensas;
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
            foreach (Defensa defensa in _defensas)
            {
                defensa.Recargar();
            }
        }

        private void Intercambiar(int a, int b)
        {
            Defensa auxiliar = _defensas[a];
            _defensas[a] = _defensas[b];
            _defensas[b] = auxiliar;
            Enlazar();
        }

        
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
