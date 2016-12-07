using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanmissionManager.Simulator.Interfaces
{
    interface IVoucherGenerator
    {
        List<Voucher> GenerateVoucher();
    }
}
