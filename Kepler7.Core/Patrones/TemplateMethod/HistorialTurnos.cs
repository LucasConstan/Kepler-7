using System;
using System.Collections.Generic;

namespace Kepler7.Core.Patrones.TemplateMethod
{
    // Guarda lo que pasó en cada etapa de cada turno.
    public class HistorialTurnos
    {
        // Se queda solo con las últimas entradas para que la lista no crezca sin límite.
        private const int Maximo = 500;
        private readonly List<EntradaTurno> _entradas = new List<EntradaTurno>();

        public IReadOnlyList<EntradaTurno> Entradas
        {
            get { return _entradas; }
        }

        // La pantalla principal se suscribe acá para mostrar cada línea apenas ocurre.
        public event Action<EntradaTurno>? EntradaRegistrada;

        internal void Registrar(int tick, string unidad, EtapaTurno etapa, string texto)
        {
            EntradaTurno entrada = new EntradaTurno(tick, unidad, etapa, texto);
            _entradas.Add(entrada);

            if (_entradas.Count > Maximo)
            {
                _entradas.RemoveAt(0);
            }

            if (EntradaRegistrada != null)
            {
                EntradaRegistrada(entrada);
            }
        }
    }
}
