using Foundation.ObjectHydrator;
using Foundation.ObjectHydrator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanmissionManager
{
    class Generator
    {
        public Generator()
        {
            Hydrator<Product> _customerHydrator = new Hydrator<Product>().WithInteger(x => x.id, 1, 100)
                .With(x => x.name, new ProductGenerator())
                .WithInteger(x => x.category, 1, 50);
            IList<Product> customerlist = _customerHydrator.GetList(20);

            foreach (Product x in customerlist)
            {
                Console.WriteLine(x.id + "        " + x.name);
            }
        }



    }
}
