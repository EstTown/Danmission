using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanmissionManager.Simulator.Interfaces;

namespace DanmissionManager.Simulator
{
    class Simulator : ISimulator
    {
        public Simulator(int days, int products, int transactions, IProductGenerator productgenerator, ITransactionGenerator transactiongenerator, IVoucherGenerator vouchergenerator)
        {
            this.SimulateNumberOfDays = days;
            this.ProductGenerator = productgenerator;
            this.TransactionGenerator = transactiongenerator;
            this.VoucherGenerator = vouchergenerator;
        }

        private int SimulateNumberOfDays;
        private int Products;
        private int Transactions;
        private IProductGenerator ProductGenerator { get; set; }
        private ITransactionGenerator TransactionGenerator { get; set; }
        private IVoucherGenerator VoucherGenerator { get; set; }
        public void RunSimulator()
        {
            throw new NotImplementedException();
        }
    }
}
