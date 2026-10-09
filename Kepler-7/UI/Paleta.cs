using System.Drawing;

namespace Kepler_7.UI
{
    /// <summary>Colores de la interfaz (tema oscuro "espacial").</summary>
    public static class Paleta
    {
        public static readonly Color Fondo = Color.FromArgb(12, 16, 28);
        public static readonly Color Panel = Color.FromArgb(22, 28, 44);
        public static readonly Color Control = Color.FromArgb(32, 40, 60);
        public static readonly Color Borde = Color.FromArgb(55, 66, 92);
        public static readonly Color Texto = Color.FromArgb(225, 230, 240);
        public static readonly Color TextoSuave = Color.FromArgb(140, 152, 175);
        public static readonly Color Acento = Color.FromArgb(70, 140, 255);

        public static readonly Color CeldaDefensa = Color.FromArgb(24, 36, 62);
        public static readonly Color CeldaAproximacion = Color.FromArgb(48, 24, 34);
        public static readonly Color CeldaHoverValida = Color.FromArgb(50, 90, 150);
        public static readonly Color CeldaHoverInvalida = Color.FromArgb(110, 40, 50);
    }
}
