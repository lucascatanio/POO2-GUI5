namespace MatchingGame
{
    public partial class frmMain : Form
    {
        private readonly Jogo jogo = new Jogo();

        private Button? primeiraCarta;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            jogo.IniciarNovoJogo();
            IniciarTimerDeUI();
            AtualizarMenuDica();

            MostrarPreviaInicial();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            jogo.IniciarNovoJogo();
            IniciarTimerDeUI();
            AtualizarMenuDica();
            primeiraCarta = null;

            tableLayoutPanel1.Enabled = true;
            for (var i = 0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                var button = (Button)tableLayoutPanel1.Controls[i];
                button.Visible = true;
            }

            MostrarPreviaInicial();
        }

        private void usarDicaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!jogo.TentarUsarDica())
            {
                return;
            }

            RevelarCartasTemporariamente();
            AtualizarMenuDica();
        }

        private void button1_Click(object sender, EventArgs e)
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
                primeiraCarta = button;
                return;
            }

            System.Threading.Thread.Sleep(1000);

            if (resultado == ResultadoJogada.ParNaoEncontrado)
            {
                jogo.EsconderUltimaJogadaSemPar();
            }

            primeiraCarta!.BackgroundImage = null;
            button.BackgroundImage = null;

            if (resultado == ResultadoJogada.ParEncontrado || resultado == ResultadoJogada.JogoFinalizado)
            {
                primeiraCarta!.Visible = false;
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
            primeiraCarta!.Enabled = true;
            primeiraCarta = null;
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

        private void MostrarPreviaInicial()
        {
            RevelarCartasTemporariamente();
        }

        private void RevelarCartasTemporariamente()
        {
            tableLayoutPanel1.Enabled = false;

            jogo.RevelarTodas();
            RedesenharTabuleiro();
            Refresh();

            System.Threading.Thread.Sleep(3000);

            jogo.EsconderTodas();
            RedesenharTabuleiro();

            tableLayoutPanel1.Enabled = true;
        }

        private static string FormatarTempo(TimeSpan tempo)
        {
            return ((int)tempo.TotalMinutes).ToString("00") + ":" + tempo.Seconds.ToString("00");
        }
    }
}
