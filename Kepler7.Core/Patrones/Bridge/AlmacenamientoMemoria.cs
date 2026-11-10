using System.Collections.Generic;

namespace Kepler7.Core.Patrones.Bridge
{
    // Guarda el mismo texto JSON, pero en un diccionario en memoria: se pierde al cerrar el juego.
    // Sirve para probar sin tocar los archivos y para mostrar que la Persistencia no cambia
    // aunque cambie dónde se guarda.
    public class AlmacenamientoMemoria : IAlmacenamiento
    {
        private readonly Dictionary<string, string> _datos = new Dictionary<string, string>();

        public string Nombre
        {
            get { return "Memoria"; }
        }

        public string Descripcion
        {
            get { return "Solo en memoria (se pierde al cerrar o reiniciar el juego)."; }
        }

        public void Guardar(string clave, string contenido)
        {
            _datos[clave] = contenido;
        }

        public string? Cargar(string clave)
        {
            if (!_datos.ContainsKey(clave))
            {
                return null;
            }

            return _datos[clave];
        }
    }
}
