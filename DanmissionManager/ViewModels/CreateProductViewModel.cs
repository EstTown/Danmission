using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanmissionManager.ViewModels
{
    class CreateProductViewModel
    {
        public CreateProductViewModel()
        {
            product product = new product()
            {
                date = DateTime.Now,
                desc = "Dett er et smart ur some kan..",
                id = 05521,
                name = "Rolex Ur",
                price = 99.95
            };
            this.Product = product;
        }

        public product Product { get; set; }
    }
}