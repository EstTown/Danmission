using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DanmissionManager.TESTCLASSSES
{
    class Product2
    {
        public Product2()
        {
            
        }
        public Bitmap ProductImage { get; set; }
        public int id { get; set; }
        public DateTime date { get; set; }
        public int category { get; set; }
    }
}
