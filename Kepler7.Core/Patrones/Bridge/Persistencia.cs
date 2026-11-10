using System.Text.Encodings.Web;
using System.Text.Json;

namespace Kepler7.Core.Patrones.Bridge
{
    // Patrón Bridge: esta es la "abstracción", es decir, QUÉ se guarda y en qué formato (JSON).
    // No escribe archivos ni diccionarios: se lo pide a su almacenamiento, que es el "puente".
    // Así se pueden combinar libremente: progreso o log, en archivo o en memoria.
    public abstract class Persistencia
    {
        // Las dos persistencias escriben el JSON igual: con sangría y sin escapar los acentos.
        protected static readonly JsonSerializerOptions OpcionesJson = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        protected IAlmacenamiento Almacenamiento { get; private set; }

        protected Persistencia(IAlmacenamiento almacenamiento)
        {
            Almacenamiento = almacenamiento;
        }

        // El puente se puede cambiar en plena ejecución sin tocar el código de las persistencias.
        public void CambiarAlmacenamiento(IAlmacenamiento nuevo)
        {
            Almacenamiento = nuevo;
        }

        public IAlmacenamiento ObtenerAlmacenamiento()
        {
            return Almacenamiento;
        }
    }
}
