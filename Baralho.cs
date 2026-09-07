using System;
using System.Collections.Generic;
using System.Drawing;

namespace MatchingGame
{
    public class Baralho
    {
        private static readonly Image[] pokemon =
        {
            Properties.Resources.abra,
            Properties.Resources.articuno,
            Properties.Resources.bellsprout,
            Properties.Resources.bulbasaur,
            Properties.Resources.caterpie,
            Properties.Resources.charmander,
            Properties.Resources.charmander__2_,
            Properties.Resources.dratini,
            Properties.Resources.eevee,
            Properties.Resources.jigglypuff,
            Properties.Resources.mankey,
            Properties.Resources.meowth,
            Properties.Resources.mew,
            Properties.Resources.pidgey,
            Properties.Resources.pikachu,
            Properties.Resources.psyduck,
            Properties.Resources.rattata,
            Properties.Resources.snorlax,
            Properties.Resources.squirtle,
            Properties.Resources.venonat,
            Properties.Resources.weedle
        };

        public List<Carta> Cartas { get; private set; } = new List<Carta>();

        public int TotalDePares => pokemon.Length;

        public int ParesEmJogo => Cartas.Count / 2;

        public Baralho()
        {
            Reiniciar();
        }

        public void Reiniciar()
        {
            Reiniciar(pokemon.Length);
        }

        public void Reiniciar(int totalPares)
        {
            totalPares = Math.Clamp(totalPares, 1, pokemon.Length);

            int[] indices = new int[pokemon.Length];
            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] = i;
            }

            Random rnd = new Random();
            for (int i = indices.Length - 1; i > 0; i--)
            {
                int j = rnd.Next(0, i + 1);
                int temp = indices[i];
                indices[i] = indices[j];
                indices[j] = temp;
            }

            Cartas = new List<Carta>(totalPares * 2);
            for (int i = 0; i < totalPares; i++)
            {
                int id = indices[i];
                Cartas.Add(new Carta(id, pokemon[id]));
                Cartas.Add(new Carta(id, pokemon[id]));
            }

            Embaralhar();
        }

        public void Embaralhar()
        {
            Random rnd = new Random();

            int metade = Cartas.Count / 2;
            for (int i = 0; i < Cartas.Count; i++)
            {
                int number = rnd.Next(0, metade);
                Carta temp = Cartas[i];
                Cartas[i] = Cartas[number];
                Cartas[number] = temp;
            }
        }
    }
}
