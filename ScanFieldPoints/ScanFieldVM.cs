using Greylock.Common.ViewModels;
using ReactiveDomain.Bus;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Windows;
using ReactiveUI.Legacy;

namespace ScanFieldPoints
{
    public class ScanFieldVM : ViewModel
    {
        private static readonly int spacing = 25;

        private ReactiveList<Point> _allPoints;
        private Point _topLeft = new Point(0, 0);
        private Point _bottomRight = new Point(100, 100);

        public ScanFieldVM(IGeneralBus bus) : base(bus)
        {
            this.WhenAnyValue(
                    x => x.TopLeft,
                    x => x.BottomRight)
                .Subscribe(RebuildPointList);
        }

        private void RebuildPointList(Tuple<Point, Point> boundaryPoints)
        {
            var topLeft = boundaryPoints.Item1;
            var bottomRight = boundaryPoints.Item2;

            BuildPointList(topLeft, bottomRight);
        }

        private void BuildPointList(Point topLeft, Point bottomRight)
        {
            var pointList = new List<Point>();
            var height = bottomRight.Y - topLeft.Y;
            var width = bottomRight.X - topLeft.X;

            var xPoints = (int)(width / spacing);
            var yPoints = (int)(height / spacing);

            for (int i = 0; i < xPoints; i++)
            {
                for (int j = 0; j < yPoints; j++)
                {
                    var p = new Point(i * spacing, j * spacing);
                    pointList.Add(p);
                }
            }

            AllPoints = new ReactiveList<Point>(pointList);
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

        public IReactiveDerivedList<PointVM> MaskedPoints =>
                                             AllPoints.CreateDerivedCollection(
                                                 x => new PointVM(x),
                                                 filter: x => false);
    }
}
