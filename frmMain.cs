namespace MatchingGame
{
    public partial class frmMain : Form
    {
        private readonly Jogo jogo = new Jogo();

        private readonly Button?[] botoesPorPosicao = new Button?[42];

        public frmMain()
        {
            InitializeComponent();
            MapearBotoes();
        }

        private void MapearBotoes()
        {
            for (var i = 0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                var button = (Button)tableLayoutPanel1.Controls[i];
                int posicao = int.Parse(button.Name.Substring(6)) - 1;
                button.TabStop = false;
                botoesPorPosicao[posicao] = button;
            }
        }

        private async Task AnimarVirada(Button button, Image? novaImagem)
        {
            Padding margemOriginal = button.Margin;
            int larguraBase = button.Width;
            int passos = 6;
            int atrasoPorPasso = 15;

            for (int i = 1; i <= passos; i++)
            {
                int extra = (larguraBase / 2) * i / passos;
                button.Margin = new Padding(margemOriginal.Left + extra, margemOriginal.Top, margemOriginal.Right + extra, margemOriginal.Bottom);
                await Task.Delay(atrasoPorPasso);
            }

            button.BackgroundImage = novaImagem;

            for (int i = passos - 1; i >= 0; i--)
            {
                int extra = (larguraBase / 2) * i / passos;
                button.Margin = new Padding(margemOriginal.Left + extra, margemOriginal.Top, margemOriginal.Right + extra, margemOriginal.Bottom);
                await Task.Delay(atrasoPorPasso);
            }

            button.Margin = margemOriginal;
        }

        private async void frmMain_Load(object sender, EventArgs e)
        {
            jogo.IniciarNovoJogo();
            IniciarTimerDeUI();
            AtualizarMenuDica();

            ConfigurarTabuleiroParaNivel();

            await MostrarPreviaInicial();
        }

        private async void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            jogo.IniciarNovoJogo();
            IniciarTimerDeUI();
            AtualizarMenuDica();

            tableLayoutPanel1.Enabled = true;
            ConfigurarTabuleiroParaNivel();

            await MostrarPreviaInicial();
        }

        private async void usarDicaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!jogo.TentarUsarDica())
            {
                return;
            }

            await RevelarCartasTemporariamente();
            AtualizarMenuDica();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.Enabled = false;
            this.ActiveControl = null;
            tableLayoutPanel1.Enabled = false;
            newGameToolStripMenuItem.Enabled = false;
            usarDicaToolStripMenuItem.Enabled = false;

            int buttonNo = int.Parse(button.Name.Substring(6));
            int posicao = buttonNo - 1;

            ResultadoJogada resultado = jogo.SelecionarCarta(posicao);

            await AnimarVirada(button, jogo.Baralho.Cartas[posicao].Imagem);

            if (resultado == ResultadoJogada.NenhumaCartaSelecionada)
            {
                tableLayoutPanel1.Enabled = true;
                newGameToolStripMenuItem.Enabled = true;
                AtualizarMenuDica();
                return;
            }

            Button primeiraCarta = botoesPorPosicao[jogo.UltimaPosicaoPrimeiraCarta]!;

            await Task.Delay(1000);

            if (resultado == ResultadoJogada.ParNaoEncontrado)
            {
                jogo.EsconderUltimaJogadaSemPar();
            }

            await Task.WhenAll(AnimarVirada(primeiraCarta, null), AnimarVirada(button, null));

            if (resultado == ResultadoJogada.ParEncontrado || resultado == ResultadoJogada.NivelConcluido || resultado == ResultadoJogada.JogoFinalizado)
            {
                primeiraCarta.Visible = false;
                button.Visible = false;

                if (resultado == ResultadoJogada.NivelConcluido)
                {
                    jogo.AvancarNivel();
                    ConfigurarTabuleiroParaNivel();
                    AtualizarStatus();
                    await MostrarPreviaInicial();
                }
                else if (resultado == ResultadoJogada.JogoFinalizado)
                {
                    uiTimer.Stop();
                    AtualizarStatus();
                    MessageBox.Show("Parabéns! " + jogo.Tentativas + " Você terminou a parada...");
                    tableLayoutPanel1.Enabled = false;
                }
            }

            button.Enabled = true;
            primeiraCarta.Enabled = true;

            newGameToolStripMenuItem.Enabled = true;
            AtualizarMenuDica();

            if (resultado != ResultadoJogada.JogoFinalizado)
            {
                tableLayoutPanel1.Enabled = true;
            }
        }

        private void IniciarTimerDeUI()
        {
            uiTimer.Stop();
            AtualizarStatus();
            uiTimer.Start();
        }

        private void uiTimer_Tick(object sender, EventArgs e)
        {
            AtualizarStatus();
        }

        private void AtualizarStatus()
        {
            lblTempoTotal.Text = "Tempo total: " + FormatarTempo(jogo.CronometroTotal.TempoDecorrido);
            lblTempoNivel.Text = "Tempo do nível: " + FormatarTempo(jogo.CronometroNivel.TempoDecorrido);
            lblTentativas.Text = "Tentativas: " + jogo.Tentativas;
            lblNivel.Text = "Nível: " + jogo.Nivel.NumeroAtual + "/" + jogo.Nivel.TotalNiveis;
        }

        private void ConfigurarTabuleiroParaNivel()
        {
            for (int posicao = 0; posicao < botoesPorPosicao.Length; posicao++)
            {
                Button? button = botoesPorPosicao[posicao];
                if (button == null)
                {
                    continue;
                }

                if (posicao < jogo.Baralho.Cartas.Count)
                {
                    button.Visible = true;
                    button.Enabled = true;
                    button.BackgroundImage = null;
                }
                else
                {
                    button.Visible = false;
                }
            }
        }

        private void AtualizarMenuDica()
        {
            usarDicaToolStripMenuItem.Text = "Usar Dica (" + jogo.DicasRestantesNoNivel + ")";
            usarDicaToolStripMenuItem.Enabled = jogo.DicasRestantesNoNivel > 0;
        }

        private void RedesenharTabuleiro()
        {
            for (var i = 0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                var button = (Button)tableLayoutPanel1.Controls[i];
                int posicao = int.Parse(button.Name.Substring(6)) - 1;
                if (posicao >= jogo.Baralho.Cartas.Count)
                {
                    continue;
                }

                Carta carta = jogo.Baralho.Cartas[posicao];
                button.BackgroundImage = carta.EstaVirada ? carta.Imagem : null;
            }
        }

        private async Task MostrarPreviaInicial()
        {
            await RevelarCartasTemporariamente();
        }

        private async Task RevelarCartasTemporariamente()
        {
            tableLayoutPanel1.Enabled = false;
            newGameToolStripMenuItem.Enabled = false;
            usarDicaToolStripMenuItem.Enabled = false;

            jogo.RevelarTodas();
            RedesenharTabuleiro();
            Refresh();

            await Task.Delay(3000);

            jogo.EsconderTodas();
            RedesenharTabuleiro();

            tableLayoutPanel1.Enabled = true;
            newGameToolStripMenuItem.Enabled = true;
            AtualizarMenuDica();
        }

        private static string FormatarTempo(TimeSpan tempo)
        {
            return ((int)tempo.TotalMinutes).ToString("00") + ":" + tempo.Seconds.ToString("00");
        }
    }
}
