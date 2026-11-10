using System.Collections.Generic;
using Kepler7.Core.Patrones.Bridge;
using Kepler7.Core.Patrones.Proxy;

namespace Kepler7.Core.Modelo
{
    // El Hub junta el progreso del jugador, la Bóveda (siempre a través de su Proxy)
    // y las dos persistencias JSON (Bridge). No depende de la Partida: el progreso
    // es del jugador y se mantiene entre partidas.
    public class HubProgreso
    {
        public const int XpPorMision = 60;
        public const int MonedasPorMision = 40;

        private readonly PersistenciaProgreso _persistenciaProgreso;
        private readonly PersistenciaBoveda _persistenciaBoveda;

        // Se asignan en Cargar(), que se llama desde el constructor.
        private Progreso _progreso = null!;
        private List<string> _logBoveda = null!;
        private IBoveda _boveda = null!;

        public HubProgreso(IAlmacenamiento almacenamiento)
        {
            _persistenciaProgreso = new PersistenciaProgreso(almacenamiento);
            _persistenciaBoveda = new PersistenciaBoveda(almacenamiento);
            Cargar();
        }

        public Progreso ObtenerProgreso()
        {
            return _progreso;
        }

        // PROXY: hacia afuera solo se ve la interfaz IBoveda, y detrás está el ProxyBoveda.
        public IBoveda ObtenerBoveda()
        {
            return _boveda;
        }

        public IReadOnlyList<string> ObtenerLogBoveda()
        {
            return _logBoveda;
        }

        public IAlmacenamiento ObtenerAlmacenamiento()
        {
            return _persistenciaProgreso.ObtenerAlmacenamiento();
        }

        // Da la recompensa de una misión: XP para subir de nivel y monedas que entran por la Bóveda.
        // Devuelve los mensajes para que la interfaz los muestre.
        public List<string> CompletarMision()
        {
            List<string> mensajes = new List<string>();
            mensajes.Add("Misión cumplida: +" + XpPorMision + " XP.");

            if (_progreso.GanarXp(XpPorMision))
            {
                mensajes.Add("¡Subiste al nivel " + _progreso.Nivel + "!");
            }

            if (_boveda.Depositar(MonedasPorMision))
            {
                mensajes.Add("+" + MonedasPorMision + " monedas depositadas en la Bóveda.");
            }

            List<string> logros = _progreso.RevisarLogros();
            for (int i = 0; i < logros.Count; i++)
            {
                mensajes.Add("¡Logro desbloqueado: " + logros[i] + "!");
            }

            return mensajes;
        }

        // BRIDGE: el Hub pide "guardar"; cada persistencia arma su JSON y su almacenamiento decide dónde.
        public void Guardar()
        {
            _persistenciaProgreso.Guardar(_progreso);
            _persistenciaBoveda.Guardar(_logBoveda);
        }

        // Vuelve a leer todo y arma un proxy nuevo, porque el progreso y el log son objetos nuevos.
        public void Cargar()
        {
            _progreso = _persistenciaProgreso.Cargar();
            _logBoveda = _persistenciaBoveda.Cargar();
            _boveda = new ProxyBoveda(new Boveda(_progreso), _progreso, _logBoveda);
        }

        // Las dos persistencias cambian de almacenamiento juntas, así el progreso y el log quedan en el mismo lugar.
        public void CambiarAlmacenamiento(IAlmacenamiento nuevo)
        {
            _persistenciaProgreso.CambiarAlmacenamiento(nuevo);
            _persistenciaBoveda.CambiarAlmacenamiento(nuevo);
        }
    }
}
