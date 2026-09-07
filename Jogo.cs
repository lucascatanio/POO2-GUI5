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
        private int primeiraPosicao = -1;

        private int ultimaPosicaoSemParA = -1;
        private int ultimaPosicaoSemParB = -1;

        public int UltimaPosicaoPrimeiraCarta { get; private set; } = -1;

        public Baralho Baralho { get; } = new Baralho();

        public Cronometro CronometroTotal { get; } = new Cronometro();

        public Cronometro CronometroNivel { get; } = new Cronometro();

        public int Tentativas { get; private set; }

        public int ParesEncontrados { get; private set; }

        public int DicasRestantesNoNivel { get; private set; }

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

            DicasRestantesNoNivel = 1;
        }

        public bool TentarUsarDica()
        {
            if (DicasRestantesNoNivel <= 0 || primeiraPosicao >= 0)
            {
                return false;
            }

            DicasRestantesNoNivel--;
            return true;
        }

        public void RevelarTodas()
        {
            foreach (Carta carta in Baralho.Cartas)
            {
                if (!carta.EstaCombinada)
                {
                    carta.Mostrar();
                }
            }
        }

        public void EsconderTodas()
        {
            foreach (Carta carta in Baralho.Cartas)
            {
                if (!carta.EstaCombinada)
                {
                    carta.Esconder();
                }
            }
        }

        public ResultadoJogada SelecionarCarta(int posicao)
        {
            Carta carta = Baralho.Cartas[posicao];
            carta.Mostrar();

            if (primeiraPosicao < 0)
            {
                primeiraPosicao = posicao;
                Tentativas++;
                return ResultadoJogada.NenhumaCartaSelecionada;
            }

            Carta primeira = Baralho.Cartas[primeiraPosicao];
            int posicaoPrimeira = primeiraPosicao;
            UltimaPosicaoPrimeiraCarta = posicaoPrimeira;
            primeiraPosicao = -1;

            if (primeira.PokemonId == carta.PokemonId)
            {
                primeira.Combinar();
                carta.Combinar();
                ParesEncontrados++;

                if (ParesEncontrados == Baralho.TotalDePares)
                {
                    CronometroNivel.Pausar();
                    CronometroTotal.Pausar();
                    return ResultadoJogada.JogoFinalizado;
                }

                return ResultadoJogada.ParEncontrado;
            }

            ultimaPosicaoSemParA = posicaoPrimeira;
            ultimaPosicaoSemParB = posicao;
            return ResultadoJogada.ParNaoEncontrado;
        }

        public void EsconderUltimaJogadaSemPar()
        {
            if (ultimaPosicaoSemParA >= 0)
            {
                Baralho.Cartas[ultimaPosicaoSemParA].Esconder();
            }

            if (ultimaPosicaoSemParB >= 0)
            {
                Baralho.Cartas[ultimaPosicaoSemParB].Esconder();
            }

            ultimaPosicaoSemParA = -1;
            ultimaPosicaoSemParB = -1;
        }
    }
}
