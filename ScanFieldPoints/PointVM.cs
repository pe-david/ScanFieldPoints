using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ControlzEx.Standard;
using Greylock.Common.ViewModels;
using ReactiveDomain.Bus;
using ReactiveUI;

namespace ScanFieldPoints
{
    public class PointVM : ViewModel
    {
        private Point _location;

        public PointVM(IGeneralBus bus, Point location) : base(bus)
        {
            _location = new Point(location.X, location.Y);
        }

        public int PointSize => Global.DotSize;

        public Point Location
        {
            get => _location;
            set => this.RaiseAndSetIfChanged(ref _location, value);
        }
    }
}
