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
using System.Windows.Media.Imaging;
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
        private int _dotSize = 8;
        //private Bitmap _pointMask = new Bitmap(512, 512, PixelFormat.Format16bppGrayScale);
        private Bitmap _pointMask = new Bitmap(@"C:\Users\rosed18169\source\repos\ScanFieldPoints\ScanFieldPoints\Resources\mask.tif");

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
                    x => new PointVM(Bus, x, DotSize),
                    p => ExamineP(p));
            //filter: p => PixelInMap((int)p.X, (int)p.Y));
            //filter: p => true);
        }

        private bool ExamineP(Point p)
        {
            var color1 = Win32_Helper.GetColor((int)p.X, (int)p.Y);
            var color = _pointMask.GetPixel((int)p.X, (int)p.Y);
            var test1 = color == color1;

            var white = Color.Black;
            var test = color == Color.White;
            return true;
        }

        private bool PixelInMap(int x, int y)
        {
            try
            {
                var color = _pointMask.GetPixel(x, y);
                var test = color == Color.White;

                if (test == true)
                {
                    var msg = $"The color is {color.ToString()}";
                    throw new InvalidOperationException(msg);
                }

                return test;
            }
            catch (InvalidOperationException e)
            {
                var msg = e.Message;
                return false;
            }
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

            ComputeXyOffsets(
                            TopLeft,
                            BottomRight,
                            xOffset: out var xOffset,
                            yOffset: out var yOffset,
                            xPoints: out var xPoints,
                            yPoints: out var yPoints);

            if (xOffset < 0) throw new InvalidOperationException("xOffset is negative.");
            if (yOffset < 0) throw new InvalidOperationException("yOffset is negative.");

            for (int i = 0; i < xPoints; i++)
            {
                for (int j = 0; j < yPoints; j++)
                {
                    var p = new Point(i * Pitch + xOffset, j * Pitch + yOffset);
                    AllPoints.Add(p);
                }
            }
        }

        private void ComputeXyOffsets(
                                      Point topLeft,
                                      Point bottomRight,
                                      out double xOffset,
                                      out double yOffset,
                                      out int xPoints,
                                      out int yPoints)
        {
            var width = bottomRight.X - topLeft.X;
            var height = bottomRight.Y - topLeft.Y;

            xPoints = (int)Math.Floor(width / Pitch);
            yPoints = (int)Math.Floor(height / Pitch);

            xOffset = (((width - (xPoints - 1) * Pitch) - DotSize)) / 2;
            yOffset = (((height - (yPoints - 1) * Pitch) - DotSize)) / 2;
        }

        public int Pitch
        {
            get => _pitch;
            set => this.RaiseAndSetIfChanged(ref _pitch, value);
        }

        public int DotSize
        {
            get => _dotSize;
            set => this.RaiseAndSetIfChanged(ref _dotSize, value);
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
