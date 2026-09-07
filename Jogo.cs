namespace MatchingGame
{
    public enum ResultadoJogada
    {
        NenhumaCartaSelecionada,
        ParEncontrado,
        ParNaoEncontrado,
        JogoFinalizado
    }

    public class Jogo
    {
        private const int TotalDePares = 21;

        private int primeiraPosicao = -1;

        public Baralho Baralho { get; } = new Baralho();

        public int Tentativas { get; private set; }

        public int ParesEncontrados { get; private set; }

        public void IniciarNovoJogo()
        {
            Baralho.Reiniciar();
            Tentativas = 0;
            ParesEncontrados = 0;
            primeiraPosicao = -1;
        }

        public ResultadoJogada SelecionarCarta(int posicao)
        {
            Carta carta = Baralho.Cartas[posicao];
            carta.Virar();

            if (primeiraPosicao < 0)
            {
                primeiraPosicao = posicao;
                Tentativas++;
                return ResultadoJogada.NenhumaCartaSelecionada;
            }

            Carta primeira = Baralho.Cartas[primeiraPosicao];
            primeiraPosicao = -1;

            if (primeira.PokemonId == carta.PokemonId)
            {
                primeira.Combinar();
                carta.Combinar();
                ParesEncontrados++;

                return ParesEncontrados == TotalDePares
                    ? ResultadoJogada.JogoFinalizado
                    : ResultadoJogada.ParEncontrado;
            }

            primeira.Virar();
            carta.Virar();
            return ResultadoJogada.ParNaoEncontrado;
        }
    }
}
