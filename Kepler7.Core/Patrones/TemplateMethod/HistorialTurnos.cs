using System.Collections.Generic;

namespace Kepler7.Core.Patrones.TemplateMethod
{
    public enum EtapaTurno
    {
        Inicio,
        Decision,
        Accion,
        Fin
    }

    public class EntradaTurno
    {
        public int Tick { get; }
        public string Unidad { get; }
        public EtapaTurno Etapa { get; }
        public string Texto { get; }

        public EntradaTurno(int tick, string unidad, EtapaTurno etapa, string texto)
        {
            Tick = tick;
            Unidad = unidad;
            Etapa = etapa;
            Texto = texto;
        }

        public override string ToString()
        {
            string etapa = Etapa == EtapaTurno.Decision ? "Decisión" : Etapa == EtapaTurno.Accion ? "Acción" : Etapa.ToString();
            return $"[t{Tick}] {Unidad} · {etapa}: {Texto}";
        }
    }

    /// <summary>Registro de cada etapa de cada turno, para mostrarlo en la interfaz.</summary>
    public class HistorialTurnos
    {
        private const int Maximo = 500;
        private readonly List<EntradaTurno> _entradas = new List<EntradaTurno>();

        public IReadOnlyList<EntradaTurno> Entradas => _entradas;

        public event Action<EntradaTurno>? EntradaRegistrada;

        internal void Registrar(int tick, string unidad, EtapaTurno etapa, string texto)
        {
            var entrada = new EntradaTurno(tick, unidad, etapa, texto);
            _entradas.Add(entrada);
            if (_entradas.Count > Maximo) _entradas.RemoveAt(0);
            EntradaRegistrada?.Invoke(entrada);
        }
    }
}
