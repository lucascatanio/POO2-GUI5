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

        public Cronometro CronometroTotal { get; } = new Cronometro();

        public Cronometro CronometroNivel { get; } = new Cronometro();

        public int Tentativas { get; private set; }

        public int ParesEncontrados { get; private set; }

        public void IniciarNovoJogo()
        {
            Baralho.Reiniciar();
            Tentativas = 0;
            ParesEncontrados = 0;
            primeiraPosicao = -1;

            CronometroTotal.Reiniciar();
            CronometroTotal.Iniciar();

            IniciarNovoNivel();
        }

        // Preparado para o sistema de níveis (fase futura): o cronômetro do nível
        // pode ser reiniciado de forma independente do cronômetro total.
        public void IniciarNovoNivel()
        {
            CronometroNivel.Reiniciar();
            CronometroNivel.Iniciar();
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

                if (ParesEncontrados == TotalDePares)
                {
                    CronometroNivel.Pausar();
                    CronometroTotal.Pausar();
                    return ResultadoJogada.JogoFinalizado;
                }

                return ResultadoJogada.ParEncontrado;
            }

            primeira.Virar();
            carta.Virar();
            return ResultadoJogada.ParNaoEncontrado;
        }
    }
}
