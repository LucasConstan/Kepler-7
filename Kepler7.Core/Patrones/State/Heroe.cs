using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.ChainOfResponsibility;
using Kepler7.Core.Patrones.TemplateMethod;
using Kepler7.Core.Patrones.Visitor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Kepler7.Core.Patrones.State
{
    // El héroe delega en su estado actual si puede jugar y cuánto pega.
    // Acá también se juntan otros patrones: la cadena de defensas y lo que se equipa desde el inventario.
    public class Heroe : Unidad
    {
        private IEstadoHeroe _estado;
        private TurnoHeroe _turno;
        private CadenaDefensas _cadena;
        private Golpe? _ultimoGolpe;
        private ItemArma? _arma;
        private ItemArmadura? _armadura;

        public Heroe()
        {
            Nombre = "Comandante";
            Simbolo = "Hé";
            Color = Color.Goldenrod;
            Vida = 150;
            _estado = new EstadoNormal();
            _turno = new TurnoHeroe();

            // Arranca solo con el blindaje: las demás defensas se equipan desde el Inventario.
            _cadena = new CadenaDefensas();
            _cadena.Agregar(new DefensaBlindaje());
        }

        public CadenaDefensas ObtenerCadena()
        {
            return _cadena;
        }

        public Golpe? ObtenerUltimoGolpe()
        {
            return _ultimoGolpe;
        }

        public IEstadoHeroe ObtenerEstado()
        {
            return _estado;
        }

        public void CambiarEstado(IEstadoHeroe nuevoEstado)
        {
            _estado = nuevoEstado;
        }

        public bool TieneVidaBaja()
        {
            return Vida <= 45;
        }

        // Equipa el arma y devuelve la que tenía antes (null si no tenía).
        public ItemArma? EquiparArma(ItemArma nueva)
        {
            ItemArma? anterior = _arma;
            _arma = nueva;
            return anterior;
        }

        // Equipa la armadura y devuelve la que tenía antes (null si no tenía).
        public ItemArmadura? EquiparArmadura(ItemArmadura nueva)
        {
            ItemArmadura? anterior = _armadura;
            _armadura = nueva;
            Vida = VidaMaxima;
            return anterior;
        }

        public string ObtenerNombreArma()
        {
            if (_arma == null)
            {
                return "Ninguna";
            }
            return _arma.ObtenerNombre();
        }

        public string ObtenerNombreArmadura()
        {
            if (_armadura == null)
            {
                return "Ninguna";
            }
            return _armadura.ObtenerNombre();
        }

        public override int Danio
        {
            get
            {
                int danioBase = 15;
                if (_arma != null)
                {
                    danioBase = danioBase + _arma.ObtenerBonusDanio();
                }
                return _estado.ModificarDanio(danioBase);
            }
        }

        public override int VidaMaxima
        {
            get
            {
                int vidaMaxima = 150;
                if (_armadura != null)
                {
                    vidaMaxima = vidaMaxima + _armadura.ObtenerBonusVida();
                }
                return vidaMaxima;
            }
        }

        public override TurnoUnidad Turno
        {
            get { return _turno; }
        }

        public override bool PuedeActuar
        {
            get { return EstaViva && _estado.PuedeActuar(); }
        }

        public override int RecibirDanio(int cantidad)
        {
            // CHAIN OF RESPONSIBILITY: el golpe pasa por las defensas y a la vida llega lo que queda.
            Golpe golpe = new Golpe(cantidad);
            _cadena.Procesar(golpe);

            int danioRecibido = base.RecibirDanio(golpe.ObtenerDanioRestante());
            golpe.AgregarPaso("A la vida: -" + danioRecibido);
            _ultimoGolpe = golpe;

            // Sin vida cae, sin importar en qué estado estaba: por eso esta regla no va en cada estado.
            if (!EstaViva)
            {
                CambiarEstado(new EstadoCaido());
                return danioRecibido;
            }

            _estado.AlRecibirDanio(this, danioRecibido);
            return danioRecibido;
        }

        // Al terminar una oleada: vuelve curado y en estado Normal.
        public void Restaurar()
        {
            Vida = VidaMaxima;
            CambiarEstado(new EstadoNormal());
            _cadena.Recargar();
        }

        public override string Descripcion()
        {
            return "Comandante | Estado: " + _estado.ObtenerNombre() + " | Vida " + Vida + "/" + VidaMaxima;
        }
    }
}
