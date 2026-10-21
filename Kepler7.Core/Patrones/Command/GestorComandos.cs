using System.Collections.Generic;

namespace Kepler7.Core.Patrones.Command
{
    /// <summary>
    /// COMMAND - Invocador. Ejecuta los comandos y los guarda en dos pilas:
    /// una para deshacer y otra para rehacer. También lleva el historial que se ve en pantalla.
    /// </summary>
    public class GestorComandos
    {
        private readonly Stack<IComando> _pilaDeshacer = new Stack<IComando>();
        private readonly Stack<IComando> _pilaRehacer = new Stack<IComando>();
        private readonly List<string> _historial = new List<string>();

        public IReadOnlyList<string> Historial => _historial;
        public bool PuedeDeshacer => _pilaDeshacer.Count > 0;
        public bool PuedeRehacer => _pilaRehacer.Count > 0;

        public bool Ejecutar(IComando comando)
        {
            if (!comando.Ejecutar()) return false;

            _pilaDeshacer.Push(comando);
            _pilaRehacer.Clear(); // una acción nueva invalida lo que se podía rehacer
            _historial.Add("✔ " + comando.Descripcion);
            return true;
        }

        public string Deshacer()
        {
            if (!PuedeDeshacer) return "No hay acciones para deshacer.";

            var comando = _pilaDeshacer.Pop();
            if (comando.Deshacer())
            {
                _pilaRehacer.Push(comando);
                return Anotar("↶ Deshecho: " + comando.Descripcion);
            }
            return Anotar("✖ No se pudo deshacer: " + comando.Descripcion);
        }

        public string Rehacer()
        {
            if (!PuedeRehacer) return "No hay acciones para rehacer.";

            var comando = _pilaRehacer.Pop();
            if (comando.Ejecutar())
            {
                _pilaDeshacer.Push(comando);
                return Anotar("↷ Rehecho: " + comando.Descripcion);
            }
            return Anotar("✖ No se pudo rehacer: " + comando.Descripcion);
        }

        // Al cambiar de fase (empieza o termina una oleada) lo anterior ya no se puede deshacer.
        public void Limpiar(string motivo)
        {
            _pilaDeshacer.Clear();
            _pilaRehacer.Clear();
            Anotar("- " + motivo + " -");
        }

        private string Anotar(string texto)
        {
            _historial.Add(texto);
            return texto;
        }
    }
}
