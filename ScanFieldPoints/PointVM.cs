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
        private int _dotSize;

        public PointVM(IGeneralBus bus, Point location, int dotSize) : base(bus)
        {
            _dotSize = dotSize;
            _location = new Point(location.X, location.Y);
        }

        public int DotSize
        {
            get => _dotSize;
            set => this.RaiseAndSetIfChanged(ref _dotSize, value);
        }

        public Point Location
        {
            get => _location;
            set => this.RaiseAndSetIfChanged(ref _location, value);
        }
    }
}
