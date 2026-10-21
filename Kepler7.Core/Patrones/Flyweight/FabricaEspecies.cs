using System.Collections.Generic;
using System.Drawing;

namespace Kepler7.Core.Patrones.Flyweight
{
    /// <summary>
    /// FLYWEIGHT - Fábrica. Guarda las especies en un diccionario: si la especie ya existe
    /// la devuelve, y si no existe la crea una sola vez. Así nunca hay dos objetos de la misma especie.
    /// </summary>
    public class FabricaEspecies
    {
        public static readonly string[] NombresEspecies = { "Humano", "Zorgon", "Kryll", "Androide", "Lumino" };

        private readonly Dictionary<string, IEspecie> _especies = new Dictionary<string, IEspecie>();

        public int CantidadEspecies => _especies.Count;

        public IEspecie ObtenerEspecie(string nombre)
        {
            if (!_especies.ContainsKey(nombre))
                _especies[nombre] = CrearEspecie(nombre);
            return _especies[nombre];
        }

        // Los datos de cada especie (estado intrínseco).
        private static IEspecie CrearEspecie(string nombre)
        {
            switch (nombre)
            {
                case "Zorgon":
                    return new Especie("Zorgon", Color.FromArgb(120, 200, 90), "Zorgon Prime", "Minerales");
                case "Kryll":
                    return new Especie("Kryll", Color.FromArgb(200, 120, 200), "Nebulosa Kryll", "Insectos");
                case "Androide":
                    return new Especie("Androide", Color.FromArgb(160, 170, 185), "Fábrica orbital", "Energía");
                case "Lumino":
                    return new Especie("Lumino", Color.FromArgb(240, 220, 100), "Estrella Lumen", "Luz solar");
                default:
                    return new Especie("Humano", Color.FromArgb(230, 170, 130), "Tierra", "Omnívora");
            }
        }
    }
}
