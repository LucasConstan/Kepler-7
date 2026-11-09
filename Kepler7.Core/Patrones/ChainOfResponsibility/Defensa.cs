namespace Kepler7.Core.Patrones.ChainOfResponsibility
{
    // Patrón Chain of Responsibility: cada defensa conoce solo a la siguiente.
    // Aplica su regla y, si todavía queda daño, le pasa el golpe.
    public abstract class Defensa
    {
        private Defensa? _siguiente;

        public void EstablecerSiguiente(Defensa? siguiente)
        {
            _siguiente = siguiente;
        }

        public void Procesar(Golpe golpe)
        {
            Aplicar(golpe);

            // Si no queda daño, la cadena se corta acá.
            if (golpe.ObtenerDanioRestante() > 0 && _siguiente != null)
            {
                _siguiente.Procesar(golpe);
            }
        }

        // Por defecto no hace nada: solo el escudo tiene carga para recuperar.
        public virtual void Recargar()
        {
        }

        public abstract string ObtenerNombre();

        protected abstract void Aplicar(Golpe golpe);
    }
}
