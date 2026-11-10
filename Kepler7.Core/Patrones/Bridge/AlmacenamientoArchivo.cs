using System;
using System.IO;

namespace Kepler7.Core.Patrones.Bridge
{
    // Guarda cada clave en un archivo .json dentro de una carpeta "Guardado" junto al ejecutable.
    // Lo que se guarda sobrevive aunque se cierre el juego.
    public class AlmacenamientoArchivo : IAlmacenamiento
    {
        private readonly string _carpeta;

        public AlmacenamientoArchivo()
        {
            _carpeta = Path.Combine(AppContext.BaseDirectory, "Guardado");
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

        private string RutaDe(string clave)
        {
            return Path.Combine(_carpeta, clave + ".json");
        }
    }
}
