using System;
using System.IO;

namespace Kepler7.Core.Patrones.Bridge
{
    // Guarda cada clave en un archivo .json dentro de %AppData%\Kepler7\.
    // Lo que se guarda sobrevive aunque se cierre el juego. No se usa la carpeta del ejecutable porque
    // ahí puede no haber permiso para escribir, y se pierde al recompilar o al mover el juego.
    public class AlmacenamientoArchivo : IAlmacenamiento
    {
        private readonly string _carpeta;

        public AlmacenamientoArchivo()
        {
            // ApplicationData es la carpeta %AppData% (Roaming) del usuario que está jugando.
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _carpeta = Path.Combine(appData, "Kepler7");
        }

        public string Nombre
        {
            get { return "Archivo JSON"; }
        }

        public string Descripcion
        {
            get { return "Archivos .json en " + _carpeta + " (sobreviven al cerrar el juego)."; }
        }

        public void Guardar(string clave, string contenido)
        {
            // Si la carpeta ya existe, CreateDirectory no hace nada.
            Directory.CreateDirectory(_carpeta);
            File.WriteAllText(RutaDe(clave), contenido);
        }

        public string? Cargar(string clave)
        {
            string ruta = RutaDe(clave);
            if (!File.Exists(ruta))
            {
                return null;
            }

            return File.ReadAllText(ruta);
        }

        public void Borrar(string clave)
        {
            string ruta = RutaDe(clave);
            if (File.Exists(ruta))
            {
                File.Delete(ruta);
            }
        }

        private string RutaDe(string clave)
        {
            return Path.Combine(_carpeta, clave + ".json");
        }
    }
}
