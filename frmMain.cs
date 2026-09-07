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
                botoesPorPosicao[posicao] = button;
            }
        }

        private async void frmMain_Load(object sender, EventArgs e)
        {
            jogo.IniciarNovoJogo();
            IniciarTimerDeUI();
            AtualizarMenuDica();

            await MostrarPreviaInicial();
        }

        private async void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            jogo.IniciarNovoJogo();
            IniciarTimerDeUI();
            AtualizarMenuDica();

            tableLayoutPanel1.Enabled = true;
            for (var i = 0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                var button = (Button)tableLayoutPanel1.Controls[i];
                button.Visible = true;
                button.Enabled = true;
            }

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

            int buttonNo = int.Parse(button.Name.Substring(6));
            int posicao = buttonNo - 1;

            ResultadoJogada resultado = jogo.SelecionarCarta(posicao);

            button.BackgroundImage = jogo.Baralho.Cartas[posicao].Imagem;
            button.Refresh();

            if (resultado == ResultadoJogada.NenhumaCartaSelecionada)
            {
                return;
            }

            Button primeiraCarta = botoesPorPosicao[jogo.UltimaPosicaoPrimeiraCarta]!;

            tableLayoutPanel1.Enabled = false;
            newGameToolStripMenuItem.Enabled = false;
            usarDicaToolStripMenuItem.Enabled = false;

            await Task.Delay(1000);

            if (resultado == ResultadoJogada.ParNaoEncontrado)
            {
                jogo.EsconderUltimaJogadaSemPar();
            }

            primeiraCarta.BackgroundImage = null;
            button.BackgroundImage = null;

            if (resultado == ResultadoJogada.ParEncontrado || resultado == ResultadoJogada.JogoFinalizado)
            {
                primeiraCarta.Visible = false;
                button.Visible = false;

                if (resultado == ResultadoJogada.JogoFinalizado)
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
