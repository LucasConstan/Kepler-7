namespace Kepler7.Core.Patrones.ChainOfResponsibility
{
    /// <summary>
    /// CHAIN OF RESPONSIBILITY - Pedido. Es lo que viaja por la cadena: el daño que queda
    /// y los pasos del recorrido, que después se muestran en pantalla.
    /// </summary>
    public class Golpe
    {
        private int _danioRestante;
        private List<string> _pasos;

        public Golpe(int danio)
        {
            _danioRestante = danio;
            _pasos = new List<string>();
            _pasos.Add("Golpe de " + danio);
        }

        public int ObtenerDanioRestante()
        {
            return _danioRestante;
        }

        public void ReducirDanio(int cantidad)
        {
            _danioRestante = _danioRestante - cantidad;
        }

        public void AgregarPaso(string paso)
        {
            _pasos.Add(paso);
        }

        public List<string> ObtenerPasos()
        {
            return _pasos;
        }
    }
}
