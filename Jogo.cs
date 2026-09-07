namespace MatchingGame
{
    public enum ResultadoJogada
    {
        NenhumaCartaSelecionada,
        ParEncontrado,
        ParNaoEncontrado,
        NivelConcluido,
        JogoFinalizado
    }

    public class Jogo
    {
        private int primeiraPosicao = -1;

        private int ultimaPosicaoSemParA = -1;
        private int ultimaPosicaoSemParB = -1;

        public int UltimaPosicaoPrimeiraCarta { get; private set; } = -1;

        public Baralho Baralho { get; } = new Baralho();

        public Nivel Nivel { get; } = new Nivel();

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

            Nivel.Reiniciar();
            IniciarNovoNivel();
        }

        public void IniciarNovoNivel()
        {
            Baralho.Reiniciar(Nivel.ParesDoNivelAtual);

            ParesEncontrados = 0;
            primeiraPosicao = -1;
            ultimaPosicaoSemParA = -1;
            ultimaPosicaoSemParB = -1;
            UltimaPosicaoPrimeiraCarta = -1;

            CronometroNivel.Reiniciar();
            CronometroNivel.Iniciar();

            DicasRestantesNoNivel = 1;
        }

        public void AvancarNivel()
        {
            Nivel.AvancarNivel();
            IniciarNovoNivel();
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

                if (ParesEncontrados == Baralho.ParesEmJogo)
                {
                    CronometroNivel.Pausar();
                    if (Nivel.EhUltimoNivel)
                    {
                        CronometroTotal.Pausar();
                        return ResultadoJogada.JogoFinalizado;
                    }

                    return ResultadoJogada.NivelConcluido;
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
