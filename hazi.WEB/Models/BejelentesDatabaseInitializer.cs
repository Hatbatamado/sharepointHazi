﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace hazi.Models
{
    public class BejelentesDatabaseInitializer : DropCreateDatabaseAlways<BejelentesContext>
    {
        protected override void Seed(BejelentesContext context)
        {
            GetJogcimek().ForEach(j => context.Jogcimek.Add(j));
            //GetProducts().ForEach(i => context.IdoBejelentesek.Add(i));
        }

        private static List<Jogcim> GetJogcimek()
        {
            var jogcimek = new List<Jogcim>
            {
                new Jogcim
                {
                    ID = 1,
                    Cim = "Jelenlét"
                },
                new Jogcim
                {
                    ID = 2,
                    Cim = "Rendes szabadság"
                },
                new Jogcim
                {
                    ID = 3,
                    Cim = "Nem ismert jogcím"
                },
                new Jogcim
                {
                    ID = 4,
                    Cim = "Munkdaidő keretre jelentett távollét"
                }
            };

            return jogcimek;
        }
    }
}