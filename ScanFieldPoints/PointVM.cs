using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ControlzEx.Standard;
using Greylock.Common.ViewModels;
using ReactiveDomain.Bus;

namespace ScanFieldPoints
{
    public class PointVM : ViewModel
    {
        private Point location;

        public PointVM(IGeneralBus bus) : base(bus)
        {
        }
    }
}
