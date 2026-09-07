namespace MatchingGame
{
    public class Nivel
    {
        private static readonly int[] progressao = { 2, 8, 15, 21 };

        public int NumeroAtual { get; private set; } = 1;

        public int TotalNiveis => progressao.Length;

        public int ParesDoNivelAtual => progressao[NumeroAtual - 1];

        public bool EhUltimoNivel => NumeroAtual >= TotalNiveis;

        public void Reiniciar()
        {
            NumeroAtual = 1;
        }

        public void AvancarNivel()
        {
            if (EhUltimoNivel)
            {
                return;
            }

            NumeroAtual++;
        }
    }
}
