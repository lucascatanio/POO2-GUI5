using System.Drawing;

namespace MatchingGame
{
    public class Carta
    {
        public Image Imagem { get; }

        public int PokemonId { get; }

        public bool EstaVirada { get; private set; }

        public bool EstaCombinada { get; private set; }

        public Carta(int pokemonId, Image imagem)
        {
            PokemonId = pokemonId;
            Imagem = imagem;
        }

        public void Mostrar()
        {
            EstaVirada = true;
        }

        public void Esconder()
        {
            EstaVirada = false;
        }

        public void Combinar()
        {
            EstaCombinada = true;
            EstaVirada = true;
        }
    }
}
