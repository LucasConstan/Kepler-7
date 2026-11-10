using System.Text.Json;
using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Bridge
{
    // Convierte el Progreso (XP, nivel, monedas y logros) a JSON y lo deja en el almacenamiento.
    public class PersistenciaProgreso : Persistencia
    {
        private const string Clave = "progreso";

        public PersistenciaProgreso(IAlmacenamiento almacenamiento) : base(almacenamiento)
        {
        }

        public void Guardar(Progreso progreso)
        {
            string json = JsonSerializer.Serialize(progreso, OpcionesJson);
            Almacenamiento.Guardar(Clave, json);
        }

        // Si no hay nada guardado (o el archivo está roto) se arranca con un progreso nuevo.
        public Progreso Cargar()
        {
            string? json = Almacenamiento.Cargar(Clave);
            if (json == null)
            {
                return new Progreso();
            }

            try
            {
                Progreso? progreso = JsonSerializer.Deserialize<Progreso>(json, OpcionesJson);
                if (progreso == null)
                {
                    return new Progreso();
                }
                return progreso;
            }
            catch (JsonException)
            {
                return new Progreso();
            }
        }
    }
}
