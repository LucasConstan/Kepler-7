using System.Collections.Generic;
using System.Text.Json;

namespace Kepler7.Core.Patrones.Bridge
{
    // Convierte el log de la Bóveda (una lista de líneas) a JSON y lo deja en el almacenamiento.
    // Es la misma idea que PersistenciaProgreso, pero con otros datos y otra clave.
    public class PersistenciaBoveda : Persistencia
    {
        private const string Clave = "log_boveda";

        public PersistenciaBoveda(IAlmacenamiento almacenamiento) : base(almacenamiento)
        {
        }

        public void Guardar(List<string> log)
        {
            string json = JsonSerializer.Serialize(log, OpcionesJson);
            Almacenamiento.Guardar(Clave, json);
        }

        // Si no hay nada guardado (o el archivo está roto) se arranca con un log vacío.
        public List<string> Cargar()
        {
            string? json = Almacenamiento.Cargar(Clave);
            if (json == null)
            {
                return new List<string>();
            }

            try
            {
                List<string>? log = JsonSerializer.Deserialize<List<string>>(json, OpcionesJson);
                if (log == null)
                {
                    return new List<string>();
                }
                return log;
            }
            catch (JsonException)
            {
                return new List<string>();
            }
        }
    }
}
