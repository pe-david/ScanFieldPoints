using Greylock.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveDomain.Bus;

namespace ScanFieldPoints
{
    public class ScanFieldVM : ViewModel
    {
        public ScanFieldVM(IGeneralBus bus) : base(bus)
        {

        }
    }
}
