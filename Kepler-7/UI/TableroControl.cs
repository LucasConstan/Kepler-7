using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Kepler_7.Modelo;
using Kepler_7.Patrones.Builder;

namespace Kepler_7.UI
{
    public class CeldaEventArgs : EventArgs
    {
        public int Carril { get; }
        public int Columna { get; }

        public CeldaEventArgs(int carril, int columna)
        {
            Carril = carril;
            Columna = columna;
        }
    }

    /// <summary>
    /// Dibuja los 6 carriles de la estación con GDI+ (System.Drawing) y avisa en qué celda se hizo clic.
    /// </summary>
    public class TableroControl : Control
    {
        private const int AnchoEncabezado = 150;
        private const int AltoEncabezado = 30;
        private const int Margen = 10;

        private Estacion _estacion = new Estacion(0);
        private Point? _celdaHover;

        public event EventHandler<CeldaEventArgs>? CeldaClick;

        /// <summary>Si es true, el hover marca en azul/rojo si la celda sirve para desplegar.</summary>
        public bool ModoDespliegue { get; set; }

        public Estacion Estacion
        {
            get => _estacion;
            set { _estacion = value; Invalidate(); }
        }

        public TableroControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = Paleta.Fondo;
            Cursor = Cursors.Hand;
        }

        private Size TamanioCelda => new Size(
            Math.Max(1, (Width - AnchoEncabezado - Margen * 2) / Estacion.Columnas),
            Math.Max(1, (Height - AltoEncabezado - Margen * 2) / Estacion.Carriles));

        private Rectangle AreaCelda(int carril, int columna)
        {
            var t = TamanioCelda;
            return new Rectangle(Margen + AnchoEncabezado + columna * t.Width,
                                 Margen + AltoEncabezado + carril * t.Height,
                                 t.Width, t.Height);
        }

        private Point? CeldaEn(Point p)
        {
            var t = TamanioCelda;
            int col = (p.X - Margen - AnchoEncabezado) / t.Width;
            int carril = (p.Y - Margen - AltoEncabezado) / t.Height;
            if (p.X < Margen + AnchoEncabezado || p.Y < Margen + AltoEncabezado) return null;
            if (col >= Estacion.Columnas || carril >= Estacion.Carriles) return null;
            return new Point(col, carril);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var nueva = CeldaEn(e.Location);
            if (nueva != _celdaHover)
            {
                _celdaHover = nueva;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _celdaHover = null;
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            var celda = CeldaEn(e.Location);
            if (celda.HasValue)
                CeldaClick?.Invoke(this, new CeldaEventArgs(celda.Value.Y, celda.Value.X));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            DibujarEncabezadoColumnas(g);
            for (int carril = 0; carril < Estacion.Carriles; carril++)
            {
                DibujarEncabezadoCarril(g, carril);
                for (int col = 0; col < Estacion.Columnas; col++)
                    DibujarCelda(g, carril, col);
            }
        }

        private void DibujarEncabezadoColumnas(Graphics g)
        {
            var primera = AreaCelda(0, 0);
            var ultimaDefensa = AreaCelda(0, Estacion.ColumnasDefensa - 1);
            var ultima = AreaCelda(0, Estacion.Columnas - 1);
            using (var fuente = new Font("Segoe UI Semibold", 9f))
            using (var pincelDef = new SolidBrush(Paleta.Acento))
            using (var pincelEnem = new SolidBrush(Color.FromArgb(230, 90, 100)))
            {
                var areaDef = new Rectangle(primera.Left, Margen, ultimaDefensa.Right - primera.Left, AltoEncabezado);
                var areaEnem = new Rectangle(ultimaDefensa.Right, Margen, ultima.Right - ultimaDefensa.Right, AltoEncabezado);
                var formato = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString("◄ NÚCLEO   ·   ZONA DE DEFENSA", fuente, pincelDef, areaDef, formato);
                g.DrawString("APROXIMACIÓN ENEMIGA ◄", fuente, pincelEnem, areaEnem, formato);
            }
        }

        private void DibujarEncabezadoCarril(Graphics g, int carril)
        {
            var celda = AreaCelda(carril, 0);
            var area = new Rectangle(Margen, celda.Top + 3, AnchoEncabezado - 8, celda.Height - 6);
            using (var fondo = new SolidBrush(Paleta.Panel))
            using (var borde = new Pen(Paleta.Borde))
            using (var camino = Redondeado(area, 6))
            {
                g.FillPath(fondo, camino);
                g.DrawPath(borde, camino);
            }
            using (var fuente = new Font("Segoe UI Semibold", 10f))
            using (var fuenteChica = new Font("Segoe UI", 8f))
            using (var pincel = new SolidBrush(Paleta.Texto))
            using (var pincelSuave = new SolidBrush(Paleta.TextoSuave))
            {
                g.DrawString(Estacion.Subsistemas[carril], fuente, pincel, area.Left + 10, area.Top + area.Height / 2 - 18);
                g.DrawString("Subsistema " + (carril + 1), fuenteChica, pincelSuave, area.Left + 10, area.Top + area.Height / 2 + 2);
            }
        }

        private void DibujarCelda(Graphics g, int carril, int col)
        {
            var area = AreaCelda(carril, col);
            var interior = Rectangle.Inflate(area, -2, -2);

            Color colorFondo = _estacion.EsZonaDefensa(col) ? Paleta.CeldaDefensa : Paleta.CeldaAproximacion;
            if (_celdaHover.HasValue && _celdaHover.Value.X == col && _celdaHover.Value.Y == carril)
            {
                bool valida = _estacion.EsZonaDefensa(col) && _estacion.ObtenerUnidad(carril, col) == null;
                colorFondo = !ModoDespliegue || valida ? Paleta.CeldaHoverValida : Paleta.CeldaHoverInvalida;
            }

            using (var pincel = new SolidBrush(colorFondo))
            using (var camino = Redondeado(interior, 5))
                g.FillPath(pincel, camino);

            var unidad = _estacion.ObtenerUnidad(carril, col);
            if (unidad != null) DibujarUnidad(g, unidad, interior);
        }

        private static void DibujarUnidad(Graphics g, Unidad unidad, Rectangle celda)
        {
            int lado = Math.Min(celda.Width, celda.Height) - 22;
            var cuerpo = new Rectangle(celda.Left + (celda.Width - lado) / 2, celda.Top + 6, lado, lado);

            using (var relleno = new LinearGradientBrush(cuerpo, ControlPaint.Light(unidad.Color), unidad.Color, 90f))
            using (var borde = new Pen(ControlPaint.Dark(unidad.Color), 2f))
            {
                if (unidad is NaveApoyo)
                {
                    // Las naves se dibujan como hexágono, los tripulantes como círculo.
                    var hex = Hexagono(cuerpo);
                    g.FillPolygon(relleno, hex);
                    g.DrawPolygon(borde, hex);
                }
                else
                {
                    g.FillEllipse(relleno, cuerpo);
                    g.DrawEllipse(borde, cuerpo);
                }
            }

            using (var fuente = new Font("Segoe UI", Math.Max(7f, lado / 4f), FontStyle.Bold))
            using (var pincel = new SolidBrush(Color.White))
            {
                var formato = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(unidad.Simbolo, fuente, pincel, cuerpo, formato);
            }

            // Barra de vida
            var barra = new Rectangle(celda.Left + 8, celda.Bottom - 10, celda.Width - 16, 5);
            float porcentaje = unidad.VidaMaxima == 0 ? 0 : (float)unidad.Vida / unidad.VidaMaxima;
            using (var fondo = new SolidBrush(Color.FromArgb(60, 0, 0, 0)))
            using (var vida = new SolidBrush(Color.FromArgb(90, 220, 120)))
            {
                g.FillRectangle(fondo, barra);
                g.FillRectangle(vida, barra.Left, barra.Top, (int)(barra.Width * porcentaje), barra.Height);
            }
        }

        private static PointF[] Hexagono(Rectangle r)
        {
            float cx = r.Left + r.Width / 2f, cy = r.Top + r.Height / 2f, radio = r.Width / 2f;
            var puntos = new PointF[6];
            for (int i = 0; i < 6; i++)
            {
                double angulo = Math.PI / 3 * i;
                puntos[i] = new PointF(cx + radio * (float)Math.Cos(angulo), cy + radio * (float)Math.Sin(angulo));
            }
            return puntos;
        }

        private static GraphicsPath Redondeado(Rectangle r, int radio)
        {
            int d = radio * 2;
            var camino = new GraphicsPath();
            camino.AddArc(r.Left, r.Top, d, d, 180, 90);
            camino.AddArc(r.Right - d, r.Top, d, d, 270, 90);
            camino.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            camino.AddArc(r.Left, r.Bottom - d, d, d, 90, 90);
            camino.CloseFigure();
            return camino;
        }
    }
}
