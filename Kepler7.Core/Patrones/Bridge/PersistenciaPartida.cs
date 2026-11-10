using System.Text.Json;
using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Bridge
{
    // Convierte la partida guardada a JSON y la deja en el almacenamiento.
    // Es la tercera persistencia: se sumó del lado de "qué se guarda" y funciona con los dos
    // almacenamientos sin tocarlos. Esa es justamente la gracia del Bridge.
    public class PersistenciaPartida : Persistencia
    {
        private const string Clave = "partida";

        public PersistenciaPartida(IAlmacenamiento almacenamiento) : base(almacenamiento)
        {
        }

        public void Guardar(PartidaGuardada partida)
        {
            string json = JsonSerializer.Serialize(partida, OpcionesJson);
            Almacenamiento.Guardar(Clave, json);
        }

        // Devuelve null si no hay ninguna partida guardada, o si el archivo está roto.
        public PartidaGuardada? Cargar()
        {
            string? json = Almacenamiento.Cargar(Clave);
            if (json == null)
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<PartidaGuardada>(json, OpcionesJson);
            }
            catch (JsonException)
            {
                return null;
            }
        }

        public void Borrar()
        {
            Almacenamiento.Borrar(Clave);
        }
    }
}
