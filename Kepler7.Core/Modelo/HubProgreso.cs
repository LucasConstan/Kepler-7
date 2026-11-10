using System;
using System.Collections.Generic;
using System.IO;
using Kepler7.Core.Patrones.Bridge;
using Kepler7.Core.Patrones.Proxy;

namespace Kepler7.Core.Modelo
{
    // El Hub junta el progreso del jugador, la Bóveda (siempre a través de su Proxy)
    // y las persistencias JSON (Bridge). No depende de la Partida: el progreso es del jugador
    // y se mantiene entre partidas; la partida guardada le llega ya convertida en datos.
    // Los logros de patrones se ganan de nuevo en cada partida. Cada uno vale una recompensa,
    // que se habilita para cobrar cuando la partida supera su primera oleada: en ese momento se
    // acreditan los que había, y a partir de ahí cada logro nuevo se acredita apenas se gana.
    // Lo que ya se pagó de la partida en curso queda en el progreso (PartidaEnCurso), así que
    // retomar un guardado y repetir una oleada o un logro no paga dos veces.
    public class HubProgreso
    {
        // Lo que da cada recompensa de logro que se cobra con el botón del Hub.
        public const int XpPorLogro = 60;
        public const int MonedasPorLogro = 40;

        // Sin esto alcanzaría con reiniciar y hacer clics para juntar logros: hay que jugar de verdad.
        public const int OleadasParaCobrarLogros = 1;

        // Recompensa por jugar: lo de cada oleada se suma al superarla, y lo de la victoria al final.
        public const int XpPorOleada = 20;
        public const int XpPorEnemigo = 2;
        public const int XpPorVictoria = 50;
        public const int MonedasPorOleada = 10;
        public const int MonedasPorVictoria = 30;

        private readonly PersistenciaProgreso _persistenciaProgreso;
        private readonly PersistenciaBoveda _persistenciaBoveda;
        private readonly PersistenciaPartida _persistenciaPartida;

        // Se asignan en Cargar(), que se llama desde el constructor.
        private Progreso _progreso = null!;
        private List<string> _logBoveda = null!;
        private IBoveda _boveda = null!;

        // Se marca cuando la partida termina. Lo que se gane después ya no cuenta para esta partida.
        private bool _partidaTerminada;

        // Avisa cuando se gana un logro de un patrón, para que la interfaz muestre el cartel.
        public event Action<string>? LogroDesbloqueado;

        public HubProgreso(IAlmacenamiento almacenamiento)
        {
            _persistenciaProgreso = new PersistenciaProgreso(almacenamiento);
            _persistenciaBoveda = new PersistenciaBoveda(almacenamiento);
            _persistenciaPartida = new PersistenciaPartida(almacenamiento);
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

        // Atajo para no escribir _progreso.PartidaEnCurso en todos lados.
        private PartidaEnCurso Actual
        {
            get { return _progreso.PartidaEnCurso; }
        }

        // --- Partida en curso ---

        // Lo llama la ventana principal cuando arranca una partida de cero.
        public void EmpezarPartidaNueva()
        {
            PartidaEnCurso nueva = new PartidaEnCurso();
            nueva.Id = Guid.NewGuid().ToString();
            _progreso.PartidaEnCurso = nueva;
            _partidaTerminada = false;
            GuardarSinErrores();
        }

        // Lo llama la ventana principal cuando se retoma una partida guardada. Si el progreso ya la conocía
        // (lo normal), se queda con lo que tenía, que puede incluir oleadas y logros de después de guardar.
        public void ContinuarPartida(PartidaGuardada guardada)
        {
            _partidaTerminada = false;
            if (Actual.Id != guardada.Id)
            {
                // Una partida que este progreso no conoce: se toma como pagado todo lo anterior al guardado.
                PartidaEnCurso retomada = new PartidaEnCurso();
                retomada.Id = guardada.Id;
                retomada.OleadasPagadas = guardada.NumeroOleada - 1;
                retomada.EnemigosPagados = guardada.EnemigosDerrotados;
                _progreso.PartidaEnCurso = retomada;
            }
            GuardarSinErrores();
        }

        public bool GanadoEnEstaPartida(string logro)
        {
            return Actual.Logros.Contains(logro);
        }

        public bool PendienteDeAcreditar(string logro)
        {
            return Actual.LogrosSinAcreditar.Contains(logro);
        }

        // True cuando los logros de esta partida ya se acreditan apenas se ganan.
        public bool CobroHabilitado()
        {
            return !_partidaTerminada && Actual.OleadasPagadas >= OleadasParaCobrarLogros;
        }

        // --- Partida guardada (Bridge: una persistencia más, con los mismos almacenamientos) ---

        // Guarda la foto de la partida que armó la Partida. Hay un solo lugar, así que pisa la anterior.
        // Puede tirar una excepción si no se puede escribir: la ventana la muestra.
        public void GuardarPartida(PartidaGuardada guardada)
        {
            guardada.Id = Actual.Id;
            guardada.Fecha = DateTime.Now.ToString("dd/MM HH:mm");
            _persistenciaPartida.Guardar(guardada);
        }

        // Devuelve null si no hay partida guardada.
        public PartidaGuardada? LeerPartidaGuardada()
        {
            return _persistenciaPartida.Cargar();
        }

        public void BorrarPartidaGuardada()
        {
            try
            {
                _persistenciaPartida.Borrar();
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        // --- Recompensas ---

        // Cobra una recompensa de logro: XP para subir de nivel y monedas que entran por la Bóveda.
        // Devuelve los mensajes para que la interfaz los muestre.
        public List<string> CobrarRecompensaDeLogro()
        {
            List<string> mensajes = new List<string>();
            if (_progreso.CargasDeLogro <= 0)
            {
                mensajes.Add("No tenés recompensas para cobrar: ganá logros en una partida y superá al menos una oleada.");
                return mensajes;
            }

            _progreso.CargasDeLogro = _progreso.CargasDeLogro - 1;
            mensajes.Add("Recompensa de logro cobrada: +" + XpPorLogro + " XP.");

            if (_progreso.GanarXp(XpPorLogro))
            {
                mensajes.Add("¡Subiste al nivel " + _progreso.Nivel + "!");
            }

            if (_boveda.Depositar(MonedasPorLogro))
            {
                mensajes.Add("+" + MonedasPorLogro + " monedas depositadas en la Bóveda.");
            }

            List<string> logros = _progreso.RevisarLogros();
            for (int i = 0; i < logros.Count; i++)
            {
                mensajes.Add("¡Logro desbloqueado: " + logros[i] + "!");
            }

            GuardarSinErrores();
            return mensajes;
        }

        // La ventana principal avisa cada vez que se supera una oleada: cuál fue y cuántos enemigos se
        // derrotaron en total. Se paga la oleada (si no se había pagado antes de retomar la partida) y los
        // enemigos nuevos, y desde la primera oleada los logros que esperaban pasan a recompensas.
        public List<string> OleadaSuperada(int numeroOleada, int enemigosDerrotados)
        {
            List<string> mensajes = new List<string>();

            int xp = 0;
            int monedas = 0;
            if (enemigosDerrotados > Actual.EnemigosPagados)
            {
                xp = (enemigosDerrotados - Actual.EnemigosPagados) * XpPorEnemigo;
                Actual.EnemigosPagados = enemigosDerrotados;
            }

            string motivo = "Oleada " + numeroOleada + " superada";
            if (numeroOleada > Actual.OleadasPagadas)
            {
                Actual.OleadasPagadas = numeroOleada;
                xp = xp + XpPorOleada;
                monedas = MonedasPorOleada;
            }
            else
            {
                motivo = motivo + " (ya se había cobrado antes de retomar la partida)";
            }

            DarRecompensa(xp, monedas, motivo, mensajes);
            if (mensajes.Count == 0)
            {
                mensajes.Add(motivo + ".");
            }

            string acreditado = AcreditarPendientes();
            if (acreditado != "")
            {
                mensajes.Add(acreditado);
            }

            GuardarSinErrores();
            return mensajes;
        }

        // Al terminar la partida se paga lo que todavía no se pagó: los enemigos de la última oleada
        // (la que no se superó) y el premio por ganar. Lo de cada oleada superada ya se sumó en su momento.
        public List<string> RecompensarPartida(ResumenPartida resumen)
        {
            List<string> mensajes = new List<string>();

            int xp = 0;
            int monedas = 0;
            if (resumen.OleadasSuperadas > Actual.OleadasPagadas)
            {
                int oleadasSinPagar = resumen.OleadasSuperadas - Actual.OleadasPagadas;
                xp = xp + oleadasSinPagar * XpPorOleada;
                monedas = monedas + oleadasSinPagar * MonedasPorOleada;
                Actual.OleadasPagadas = resumen.OleadasSuperadas;
            }
            if (resumen.EnemigosDerrotados > Actual.EnemigosPagados)
            {
                xp = xp + (resumen.EnemigosDerrotados - Actual.EnemigosPagados) * XpPorEnemigo;
                Actual.EnemigosPagados = resumen.EnemigosDerrotados;
            }
            if (resumen.Victoria)
            {
                xp = xp + XpPorVictoria;
                monedas = monedas + MonedasPorVictoria;
            }
            DarRecompensa(xp, monedas, "Final de la partida", mensajes);

            if (Actual.Xp == 0)
            {
                mensajes.Add("Esta vez no hubo recompensa: hace falta superar una oleada o derrotar enemigos.");
            }
            else
            {
                mensajes.Add("En total, esta partida te dio +" + Actual.Xp + " XP y +" + Actual.Monedas + " monedas.");
            }

            AcreditarLogrosDePartida(mensajes);

            // La partida terminó: su guardado ya no sirve. Así no se puede volver atrás para reintentarla.
            BorrarPartidaGuardada();
            GuardarSinErrores();
            return mensajes;
        }

        // Suma XP y monedas y deja los mensajes. Las monedas entran por la Bóveda (Proxy),
        // así quedan anotadas en su log como cualquier depósito.
        private void DarRecompensa(int xp, int monedas, string motivo, List<string> mensajes)
        {
            if (xp <= 0 && monedas <= 0)
            {
                return;
            }

            string texto = motivo + ": +" + xp + " XP";
            if (monedas > 0 && _boveda.Depositar(monedas))
            {
                texto = texto + " y +" + monedas + " monedas";
            }
            mensajes.Add(texto + ".");

            Actual.Xp = Actual.Xp + xp;
            Actual.Monedas = Actual.Monedas + monedas;

            if (_progreso.GanarXp(xp))
            {
                mensajes.Add("¡Subiste al nivel " + _progreso.Nivel + "!");
            }

            List<string> logros = _progreso.RevisarLogros();
            for (int i = 0; i < logros.Count; i++)
            {
                mensajes.Add("¡Logro desbloqueado: " + logros[i] + "!");
            }
        }

        // Pasa a recompensas los logros que estaban esperando, si la partida ya superó las oleadas necesarias.
        private string AcreditarPendientes()
        {
            int cantidad = Actual.LogrosSinAcreditar.Count;
            if (cantidad == 0 || Actual.OleadasPagadas < OleadasParaCobrarLogros)
            {
                return "";
            }

            _progreso.CargasDeLogro = _progreso.CargasDeLogro + cantidad;
            for (int i = 0; i < cantidad; i++)
            {
                _progreso.SumarVez(Actual.LogrosSinAcreditar[i]);
            }
            Actual.LogrosSinAcreditar.Clear();
            GuardarSinErrores();

            if (cantidad == 1)
            {
                return "1 recompensa de logro lista para cobrar en el Hub (" + _progreso.CargasDeLogro + " en total).";
            }
            return cantidad + " recompensas de logros listas para cobrar en el Hub (" + _progreso.CargasDeLogro + " en total).";
        }

        // Al terminar la partida se acredita lo que haya quedado esperando. Si nunca se superó una oleada,
        // esos logros se pierden. Después, lo que se gane ya no cuenta para esta partida.
        private void AcreditarLogrosDePartida(List<string> mensajes)
        {
            if (_partidaTerminada)
            {
                return;
            }

            int cantidad = Actual.LogrosSinAcreditar.Count;
            string acreditado = AcreditarPendientes();
            if (acreditado != "")
            {
                mensajes.Add(acreditado);
            }
            else if (cantidad > 0)
            {
                mensajes.Add("Los " + cantidad + " logros de esta partida no se cobran: hace falta superar al menos una oleada.");
                Actual.LogrosSinAcreditar.Clear();
            }

            _partidaTerminada = true;
        }

        // Compra una mejora de la tienda. El pago es un retiro de la Bóveda, así que pasa por el Proxy:
        // si no tiene el nivel o no le alcanza, no se compra nada y el intento queda en el log.
        public string ComprarMejora(Mejora mejora)
        {
            if (!_boveda.Retirar(mejora.Costo))
            {
                // El que decide es el Proxy; acá solo se arma un mensaje que explique por qué no pasó.
                if (_progreso.Nivel < ProxyBoveda.NivelParaRetirar)
                {
                    return "No se pudo comprar " + mejora.Nombre + ": la Bóveda pide nivel " + ProxyBoveda.NivelParaRetirar + " para sacar monedas.";
                }
                return "No se pudo comprar " + mejora.Nombre + ": hacen falta " + mejora.Costo + " monedas.";
            }

            _progreso.MejorasPendientes.Add(mejora.Nombre);
            GuardarSinErrores();
            DesbloquearLogro(CatalogoLogros.Proxy);
            return "Compraste " + mejora.Nombre + " por " + mejora.Costo + " monedas. Se aplica cuando empieces una partida nueva.";
        }

        // Entrega las mejoras compradas y las borra del progreso, porque cada una sirve para una sola partida.
        public List<string> TomarMejorasPendientes()
        {
            List<string> mejoras = new List<string>(_progreso.MejorasPendientes);
            if (mejoras.Count > 0)
            {
                _progreso.MejorasPendientes.Clear();
                GuardarSinErrores();
            }

            return mejoras;
        }

        // Desbloquea un logro de patrón en la partida actual. Cada uno se puede ganar una vez por partida.
        // Si la partida ya superó una oleada se acredita al toque; si no, espera a que la supere.
        // Se guarda enseguida, así retomar un guardado viejo no permite ganarlo de nuevo.
        public bool DesbloquearLogro(string logro)
        {
            if (_partidaTerminada || Actual.Logros.Contains(logro))
            {
                return false;
            }

            Actual.Logros.Add(logro);
            Actual.LogrosSinAcreditar.Add(logro);

            // También queda en el historial de logros conseguidos alguna vez.
            _progreso.DesbloquearLogro(logro);

            AcreditarPendientes();
            GuardarSinErrores();

            if (LogroDesbloqueado != null)
            {
                LogroDesbloqueado(logro);
            }

            return true;
        }

        // --- Guardado del progreso ---

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

        // Las persistencias cambian de almacenamiento juntas, así todo queda en el mismo lugar.
        public void CambiarAlmacenamiento(IAlmacenamiento nuevo)
        {
            _persistenciaProgreso.CambiarAlmacenamiento(nuevo);
            _persistenciaBoveda.CambiarAlmacenamiento(nuevo);
            _persistenciaPartida.CambiarAlmacenamiento(nuevo);
        }

        // El guardado automático no puede cortar el juego: si falla, lo ganado sigue en memoria
        // y el jugador todavía puede guardarlo a mano desde el Hub.
        private void GuardarSinErrores()
        {
            try
            {
                Guardar();
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
        }
    }
}
