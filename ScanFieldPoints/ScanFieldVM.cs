using Greylock.Common.ViewModels;
using ReactiveDomain.Bus;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reactive.Linq;
using System.Windows.Documents;
using ReactiveUI.Legacy;
using Point = System.Windows.Point;

namespace ScanFieldPoints
{
    public class ScanFieldVM : ViewModel
    {
        private ReactiveList<Point> _allPoints;
        private Point _topLeft;
        private Point _bottomRight;
        private int _pitch = 25;
        private Bitmap _pointMask = new Bitmap(512, 512, PixelFormat.Format16bppGrayScale);

        public ScanFieldVM(IGeneralBus bus) : base(bus)
        {
            AllPoints = new ReactiveList<Point>();
            this.WhenAnyValue(
                    x => x.TopLeft,
                    x => x.BottomRight)
                .Throttle(TimeSpan.FromMilliseconds(100))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(RebuildPointList);

            MaskedPoints =
                AllPoints.CreateDerivedCollection(
                    x => new PointVM(Bus, x),
                    filter: p => true);
                    //filter: p => _pointMask.GetPixel((int)p.X, (int)p.Y) == Color.White);
        }

        private void RebuildPointList(Tuple<Point, Point> boundaryPoints)
        {
            var topLeft = boundaryPoints.Item1;
            var bottomRight = boundaryPoints.Item2;

            BuildPointList();
        }

        private void BuildPointList()
        {
            var watch = new Stopwatch();
            watch.Start();

            AllPoints.Clear();
            var height = BottomRight.Y - TopLeft.Y;
            var width = BottomRight.X - TopLeft.X;

            var xPoints = (int)(width / Pitch);
            var yPoints = (int)(height / Pitch);

            ComputeXyOffsets(TopLeft, BottomRight, out var xOffset, out var yOffset);
            for (int i = 0; i < xPoints; i++)
            {
                for (int j = 0; j < yPoints; j++)
                {
                    var p = new Point(i * Pitch + xOffset + Global.DotSize, j * Pitch + yOffset + Global.DotSize);
                    AllPoints.Add(p);
                }
            }

            watch.Stop();
            var xxx = watch.ElapsedMilliseconds;
        }

        private void ComputeXyOffsets(Point topLeft, Point bottomRight, out double xOffset, out double yOffset)
        {
            var width = bottomRight.X - topLeft.X;
            var height = bottomRight.Y - topLeft.Y;

            var xPoints = Math.Floor(width / Pitch);
            var yPoints = Math.Floor(height / Pitch);

            xOffset = (width - xPoints * Pitch);
            yOffset = (height - yPoints * Pitch);
        }

        public int Pitch
        {
            get => _pitch;
            set => this.RaiseAndSetIfChanged(ref _pitch, value);
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
