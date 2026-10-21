using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.Builder;
using Kepler7.Core.Patrones.Flyweight;

namespace Kepler7.Game.UI
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
        private const int AltoEncabezado = 44;
        private const int Margen = 10;

        private Partida _partida = new Partida(0);
        private Point? _celdaHover;

        public event EventHandler<CeldaEventArgs>? CeldaClick;

        /// <summary>Si es true, el hover marca en azul/rojo si la celda sirve para desplegar.</summary>
        public bool ModoDespliegue { get; set; }

        /// <summary>Unidad marcada con un borde blanco (el enemigo elegido para cambiarle la IA).</summary>
        public Unidad? Seleccionada { get; set; }

        public Partida Partida
        {
            get => _partida;
            set { _partida = value; Invalidate(); }
        }

        public TableroControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = Paleta.Fondo;
            Cursor = Cursors.Hand;
        }

        private Size TamanioCelda => new Size(
            Math.Max(1, (Width - AnchoEncabezado - Margen * 2) / Grilla.Columnas),
            Math.Max(1, (Height - AltoEncabezado - Margen * 2) / Grilla.Carriles));

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
            if (p.X < Margen + AnchoEncabezado || p.Y < Margen + AltoEncabezado) return null;
            int col = (p.X - Margen - AnchoEncabezado) / t.Width;
            int carril = (p.Y - Margen - AltoEncabezado) / t.Height;
            if (col >= Grilla.Columnas || carril >= Grilla.Carriles) return null;
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
            for (int carril = 0; carril < Grilla.Carriles; carril++)
            {
                DibujarEncabezadoCarril(g, carril);
                for (int col = 0; col < Grilla.Columnas; col++)
                    DibujarCelda(g, carril, col);
            }
        }

        private void DibujarEncabezadoColumnas(Graphics g)
        {
            var primera = AreaCelda(0, 0);
            var ultimaDefensa = AreaCelda(0, Grilla.ColumnasDefensa - 1);
            var ultima = AreaCelda(0, Grilla.Columnas - 1);
            using (var fuente = new Font("Segoe UI Semibold", 9f))
            using (var pincelDef = new SolidBrush(Paleta.Acento))
            using (var pincelEnem = new SolidBrush(Color.FromArgb(230, 90, 100)))
            {
                var areaDef = new Rectangle(primera.Left, Margen, ultimaDefensa.Right - primera.Left, AltoEncabezado);
                var areaEnem = new Rectangle(ultimaDefensa.Right, Margen, ultima.Right - ultimaDefensa.Right, AltoEncabezado);
                var formato = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString("ZONA DE DEFENSA", fuente, pincelDef, areaDef, formato);
                g.DrawString("APROXIMACIÓN ENEMIGA ◄", fuente, pincelEnem, areaEnem, formato);

                // Arriba de los núcleos: el título y el total de tripulantes de la estación (Flyweight).
                var tripulacion = _partida.Tripulacion;
                var areaNucleo = new Rectangle(Margen, Margen, AnchoEncabezado - 8, 18);
                g.DrawString("◄ NÚCLEO", fuente, pincelDef, areaNucleo, formato);
                using (var fuenteChica = new Font("Segoe UI", 7.5f))
                using (var pincelSuave = new SolidBrush(Paleta.TextoSuave))
                {
                    // Se dibuja desde un punto (sin recuadro) para que el texto no se corte si es largo.
                    g.DrawString($"Tripulantes totales {tripulacion.TotalVivos:N0}/{tripulacion.TotalGenerados:N0}",
                                 fuenteChica, pincelSuave, Margen + 4, Margen + 19);
                }
                float porcentaje = (float)tripulacion.TotalVivos / tripulacion.TotalGenerados;
                DibujarBarra(g, new Rectangle(Margen + 10, Margen + 36, AnchoEncabezado - 28, 5), porcentaje, ColorTripulantes);
            }
        }

        private static readonly Color ColorTripulantes = Color.FromArgb(80, 170, 255);

        private static void DibujarBarra(Graphics g, Rectangle barra, float porcentaje, Color color)
        {
            porcentaje = Math.Min(1f, porcentaje);
            using (var fondo = new SolidBrush(Color.FromArgb(60, 255, 255, 255)))
            using (var relleno = new SolidBrush(color))
            {
                g.FillRectangle(fondo, barra);
                g.FillRectangle(relleno, barra.Left, barra.Top, (int)(barra.Width * porcentaje), barra.Height);
            }
        }

        private void DibujarEncabezadoCarril(Graphics g, int carril)
        {
            var celda = AreaCelda(carril, 0);
            var area = new Rectangle(Margen, celda.Top + 3, AnchoEncabezado - 8, celda.Height - 6);
            int integridad = _partida.IntegridadDelCarril(carril);
            bool caido = integridad == 0;

            using (var fondo = new SolidBrush(caido ? Color.FromArgb(70, 24, 30) : Paleta.Panel))
            using (var borde = new Pen(caido ? Color.FromArgb(200, 70, 80) : Paleta.Borde))
            using (var camino = Redondeado(area, 6))
            {
                g.FillPath(fondo, camino);
                g.DrawPath(borde, camino);
            }

            using (var fuente = new Font("Segoe UI Semibold", 10f))
            using (var fuenteChica = new Font("Segoe UI", 8f))
            using (var pincel = new SolidBrush(Paleta.Texto))
            using (var pincelSuave = new SolidBrush(caido ? Color.FromArgb(240, 120, 130) : Paleta.TextoSuave))
            {
                int mitad = area.Top + area.Height / 2;
                int tripulantes = _partida.Tripulacion.TripulantesEn(carril);
                g.DrawString(Estacion.Subsistemas[carril], fuente, pincel, area.Left + 10, mitad - 30);
                g.DrawString(caido ? "¡CAÍDO!" : $"Núcleo {integridad}  ·  Trip. {tripulantes:N0}",
                             fuenteChica, pincelSuave, area.Left + 10, mitad - 10);

                // Barra de tripulantes del subsistema (el escudo del núcleo).
                float porcTripulantes = (float)tripulantes / BibliotecaTripulantes.InicialesPorCarril;
                DibujarBarra(g, new Rectangle(area.Left + 10, mitad + 8, area.Width - 20, 5), porcTripulantes, ColorTripulantes);

                // Barra de integridad del núcleo del carril.
                var barra = new Rectangle(area.Left + 10, mitad + 17, area.Width - 20, 6);
                float porcentaje = (float)integridad / Partida.IntegridadMaxima;
                Color colorBarra = porcentaje > 0.5f ? Color.FromArgb(90, 220, 120)
                                 : porcentaje > 0.25f ? Color.FromArgb(230, 190, 60)
                                 : Color.FromArgb(230, 80, 80);
                using (var fondoBarra = new SolidBrush(Color.FromArgb(60, 255, 255, 255)))
                using (var relleno = new SolidBrush(colorBarra))
                {
                    g.FillRectangle(fondoBarra, barra);
                    g.FillRectangle(relleno, barra.Left, barra.Top, (int)(barra.Width * porcentaje), barra.Height);
                }
            }
        }

        private void DibujarCelda(Graphics g, int carril, int col)
        {
            var area = AreaCelda(carril, col);
            var interior = Rectangle.Inflate(area, -2, -2);
            var grilla = _partida.Grilla;

            Color colorFondo = grilla.EsZonaDefensa(col) ? Paleta.CeldaDefensa : Paleta.CeldaAproximacion;
            if (_celdaHover.HasValue && _celdaHover.Value.X == col && _celdaHover.Value.Y == carril)
            {
                bool valida = grilla.EsZonaDefensa(col) && grilla.ObtenerUnidad(carril, col) == null;
                colorFondo = !ModoDespliegue || valida ? Paleta.CeldaHoverValida : Paleta.CeldaHoverInvalida;
            }

            using (var pincel = new SolidBrush(colorFondo))
            using (var camino = Redondeado(interior, 5))
                g.FillPath(pincel, camino);

            var unidad = grilla.ObtenerUnidad(carril, col);
            if (unidad != null) DibujarUnidad(g, unidad, interior);
        }

        private void DibujarUnidad(Graphics g, Unidad unidad, Rectangle celda)
        {
            int lado = Math.Min(celda.Width, celda.Height) - 22;
            var cuerpo = new Rectangle(celda.Left + (celda.Width - lado) / 2, celda.Top + 6, lado, lado);

            using (var relleno = new LinearGradientBrush(cuerpo, ControlPaint.Light(unidad.Color), unidad.Color, 90f))
            using (var borde = new Pen(ControlPaint.Dark(unidad.Color), 2f))
            {
                if (unidad is Enemigo)
                {
                    // Los enemigos se dibujan como rombo, las naves como hexágono y los tripulantes como círculo.
                    var rombo = Rombo(cuerpo);
                    g.FillPolygon(relleno, rombo);
                    g.DrawPolygon(borde, rombo);
                }
                else if (unidad is NaveApoyo)
                {
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

            // Los enemigos muestran la sigla de su IA en una esquina (AG, DF, AL o CA).
            if (unidad is Enemigo enemigo)
            {
                using (var fuenteIA = new Font("Segoe UI", 7.5f, FontStyle.Bold))
                using (var pincelIA = new SolidBrush(ColorDeIA(enemigo.Estrategia.Sigla)))
                    g.DrawString(enemigo.Estrategia.Sigla, fuenteIA, pincelIA, celda.Left + 4, celda.Top + 2);
            }

            if (unidad == Seleccionada)
            {
                using (var marca = new Pen(Color.White, 2f))
                using (var camino = Redondeado(celda, 5))
                    g.DrawPath(marca, camino);
            }

            // Barra de vida
            var barra = new Rectangle(celda.Left + 8, celda.Bottom - 10, celda.Width - 16, 5);
            float vida = unidad.VidaMaxima == 0 ? 0 : (float)unidad.Vida / unidad.VidaMaxima;
            using (var fondo = new SolidBrush(Color.FromArgb(60, 0, 0, 0)))
            using (var relleno = new SolidBrush(Color.FromArgb(90, 220, 120)))
            {
                g.FillRectangle(fondo, barra);
                g.FillRectangle(relleno, barra.Left, barra.Top, (int)(barra.Width * vida), barra.Height);
            }
        }

        public static Color ColorDeIA(string sigla)
        {
            switch (sigla)
            {
                case "AG": return Color.FromArgb(240, 100, 100);
                case "DF": return Color.FromArgb(100, 170, 240);
                case "AL": return Color.FromArgb(240, 210, 90);
                default: return Color.FromArgb(190, 130, 250);
            }
        }

        private static PointF[] Rombo(Rectangle r)
        {
            float cx = r.Left + r.Width / 2f, cy = r.Top + r.Height / 2f;
            return new[]
            {
                new PointF(cx, r.Top), new PointF(r.Right, cy), new PointF(cx, r.Bottom), new PointF(r.Left, cy)
            };
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
