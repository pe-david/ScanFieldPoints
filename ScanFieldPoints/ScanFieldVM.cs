using Greylock.Common.ViewModels;
using ReactiveDomain.Bus;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Documents;
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
                .Throttle(TimeSpan.FromMilliseconds(100))
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

        //private static int timesCalled = 0;

        private void BuildPointList()
        {
            //timesCalled++;
            var watch = new Stopwatch();
            watch.Start();

            AllPoints.Clear();
            var height = BottomRight.Y - TopLeft.Y;
            var width = BottomRight.X - TopLeft.X;

            var xPoints = (int)(width / spacing);
            var yPoints = (int)(height / spacing);

            //var watch2 = new Stopwatch();
            //watch2.Start();

            ComputeXyOffsets(TopLeft, BottomRight, out var xOffset, out var yOffset);
            //var pointList = new List<Point>();
            for (int i = 0; i < xPoints; i++)
            {
                for (int j = 0; j < yPoints; j++)
                {
                    var p = new Point(i * spacing + xOffset + Global.DotSize, j * spacing + yOffset + Global.DotSize);
                    AllPoints.Add(p);
                }
            }
            //AllPoints.AddRange(pointList);
            //watch2.Stop();
            //var yyy = watch2.ElapsedMilliseconds;

            watch.Stop();
            var xxx = watch.ElapsedMilliseconds;
        }

        private void ComputeXyOffsets(Point topLeft, Point bottomRight, out double xOffset, out double yOffset)
        {
            var width = bottomRight.X - topLeft.X;
            var height = bottomRight.Y - topLeft.Y;

            var xPoints = Math.Floor(width / spacing);
            var yPoints = Math.Floor(height / spacing);

            xOffset = (width - xPoints * spacing) / 2;
            yOffset = (height - yPoints * spacing) / 2;
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
