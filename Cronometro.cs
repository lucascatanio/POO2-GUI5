using System;

namespace MatchingGame
{
    public class Cronometro
    {
        private DateTime? inicio;
        private TimeSpan acumulado = TimeSpan.Zero;

        public bool EmExecucao => inicio.HasValue;

        public TimeSpan TempoDecorrido
        {
            get
            {
                if (inicio.HasValue)
                {
                    return acumulado + (DateTime.Now - inicio.Value);
                }

                return acumulado;
            }
        }

        public void Iniciar()
        {
            if (!inicio.HasValue)
            {
                inicio = DateTime.Now;
            }
        }

        public void Pausar()
        {
            if (inicio.HasValue)
            {
                acumulado += DateTime.Now - inicio.Value;
                inicio = null;
            }
        }

        public void Reiniciar()
        {
            inicio = null;
            acumulado = TimeSpan.Zero;
        }
    }
}
