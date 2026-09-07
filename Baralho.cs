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

        public Baralho()
        {
            Reiniciar();
        }

        public void Reiniciar()
        {
            Cartas = new List<Carta>(42);
            for (int id = 0; id < pokemon.Length; id++)
            {
                Cartas.Add(new Carta(id, pokemon[id]));
                Cartas.Add(new Carta(id, pokemon[id]));
            }

            Embaralhar();
        }

        public void Embaralhar()
        {
            Random rnd = new Random();

            for (int i = 0; i < 42; i++)
            {
                int number = rnd.Next(0, 21);
                Carta temp = Cartas[i];
                Cartas[i] = Cartas[number];
                Cartas[number] = temp;
            }
        }
    }
}
