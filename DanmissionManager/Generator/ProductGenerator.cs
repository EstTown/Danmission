using System;
using System.Collections.Generic;
using Foundation.ObjectHydrator.Interfaces;
using Foundation.ObjectHydrator;

namespace DanmissionManager
{
    public class ProductGenerator : IGenerator<string>
    {
        Random random;
        IList<string> names = new List<string>();
        public ProductGenerator()
        {
            random = RandomSingleton.Instance.Random;
            LoadNames();
        }

        public string Generate()
        {
            return names[random.Next(0, names.Count)];
        }

        private void LoadNames()
        {
            names = new List<String> {
                "Gaffel",
                "Maleri",
                "Ske",
                "Blomstervase",
                "Rød top",
                "Blå løbetrøje",
                "Grøn T-shirt",
                "Kjole",
                "Legetøjskøkken",
                "Dukke",
                "Brødrister",
                "Te kop",
                "Kaffekop",
                "Te stel 12 stk",
                "Gyngestol",
                "Børnegyngestol",
                "Ur",
                "Bornholmerur",
                "Blå børne sneakers",
                "Løbesko",
                "Lædertaske",
                "Halsterklæde",
                "Sort top",
                "Grå hættetrøje",
                "Stråtaske",
                "Termokop",
                "Skærebræt af træ",
                "24stk bestik sæt",
                "4stk ølglas",
                "Bøg",
                "Børnebog",
                "Pocelænset",
                "Havestol",
                "Kontorstol"
            };

        }
    }
}