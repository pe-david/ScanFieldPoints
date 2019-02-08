using Greylock.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ReactiveDomain.Bus;
using ReactiveUI;
using ReactiveUI.Legacy;

namespace ScanFieldPoints
{
    public class ScanFieldVM : ViewModel
    {
        private ReactiveList<Point> _allPoints;

        public ScanFieldVM(IGeneralBus bus) : base(bus)
        {

        }

        public ReactiveList<Point> AllPoints
        {
            get => _allPoints;
            set => this.RaiseAndSetIfChanged(ref _allPoints, value);
        }

        public IReactiveDerivedList<PointVM> MaskedPoints =>
                                             AllPoints.CreateDerivedCollection(
                                                 x => new PointVM(x),
                                                 filter: x => false);
    }
}
