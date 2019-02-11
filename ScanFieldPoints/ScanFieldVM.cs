using Greylock.Common.ViewModels;
using ReactiveDomain.Bus;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows;
using ReactiveUI.Legacy;

namespace ScanFieldPoints
{
    public class ScanFieldVM : ViewModel
    {
        private static readonly int spacing = 25;

        private ReactiveList<Point> _allPoints;
        private Point _topLeft;
        private Point _bottomRight;

        public ScanFieldVM(IGeneralBus bus) : base(bus)
        {
            AllPoints = new ReactiveList<Point>();
            this.WhenAnyValue(
                    x => x.TopLeft,
                    x => x.BottomRight)
                .Throttle(TimeSpan.FromSeconds(0.2))
                .ObserveOnDispatcher()
                .Subscribe(RebuildPointList);

            MaskedPoints =
                AllPoints.CreateDerivedCollection(
                    x => new PointVM(Bus, x),
                    filter: x => true);
        }

        private void RebuildPointList(Tuple<Point, Point> boundaryPoints)
        {
            var topLeft = boundaryPoints.Item1;
            var bottomRight = boundaryPoints.Item2;

            BuildPointList();
        }

        private void BuildPointList()
        {
            AllPoints.Clear();
            var height = BottomRight.Y - TopLeft.Y;
            var width = BottomRight.X - TopLeft.X;

            var xPoints = (int)(width / spacing);
            var yPoints = (int)(height / spacing);

            for (int i = 0; i < xPoints; i++)
            {
                for (int j = 0; j < yPoints; j++)
                {
                    var p = new Point(i * spacing, j * spacing);
                    AllPoints.Add(p);
                }
            }
        }

        public Point TopLeft
        {
            get => _topLeft;
            set => this.RaiseAndSetIfChanged(ref _topLeft, value);
        }

        public Point BottomRight
        {
            get => _bottomRight;
            set => this.RaiseAndSetIfChanged(ref _bottomRight, value);
        }

        public ReactiveList<Point> AllPoints
        {
            get => _allPoints;
            set => this.RaiseAndSetIfChanged(ref _allPoints, value);
        }

        public IReactiveDerivedList<PointVM> MaskedPoints;
    }
}
