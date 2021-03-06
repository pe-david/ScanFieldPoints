﻿using System.Runtime.CompilerServices;
using ReactiveUI;
using System.Windows;
using Point = System.Windows.Point;
using System.Windows.Media;

namespace ScanFieldPoints
{
    /// <summary>
    /// Interaction logic for ScanField.xaml
    /// </summary>
    public partial class ScanField : IViewFor<ScanFieldVM>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ScanFieldVM),
                typeof(ScanField),
                new PropertyMetadata(defaultValue:default(ScanFieldVM)));

        public static readonly DependencyProperty TopLeftProperty =
            DependencyProperty.Register(
                "TopLeft",
                typeof(Point),
                typeof(ScanField),
                new PropertyMetadata(default(Point)));

        public static readonly DependencyProperty BottomRightProperty =
            DependencyProperty.Register(
                "BottomRight",
                typeof(Point),
                typeof(ScanField),
                new PropertyMetadata(default(Point)));

        public static readonly DependencyProperty PitchProperty =
            DependencyProperty.Register(
                "Pitch",
                typeof(int),
                typeof(ScanField),
                new FrameworkPropertyMetadata(
                    defaultValue: 25));

        public static readonly DependencyProperty DotDiameterProperty =
            DependencyProperty.Register(
                "DotDiameter",
                typeof(int),
                typeof(ScanField),
                new FrameworkPropertyMetadata(
                    defaultValue: 16));

        public new static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register(
                "BorderThickness",
                typeof(Thickness),
                typeof(ScanField),
                new FrameworkPropertyMetadata(
                    defaultValue: new Thickness(2)));

        public new static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register(
                "BorderBrush",
                typeof(Brush),
                typeof(ScanField),
                new FrameworkPropertyMetadata(
                    defaultValue: Brushes.Black));

        public ScanField()
        {
            InitializeComponent();

            ViewModel = new ScanFieldVM(Global.Bus);

            this.WhenActivated(
                d =>
                {
                    d(this.OneWayBind(ViewModel, vm => vm.MaskedPoints, v => v.Dots.ItemsSource));
                    d(this.Bind(ViewModel, vm => vm.TopLeft, v => v.TopLeft));
                    d(this.Bind(ViewModel, vm => vm.BottomRight, v => v.BottomRight));
                    //d(this.Bind(ViewModel, vm => vm.Pitch, v => v.Pitch));

                    ViewModel.Pitch = Pitch;
                    ViewModel.DotSize = DotDiameter;
                    DotsBorder.BorderThickness = BorderThickness;
                    DotsBorder.BorderBrush = BorderBrush;

                    TopLeft = new Point(0, 0);
                    BottomRight = new Point(this.Dots.ActualWidth, this.Dots.ActualHeight);
                });
        }

        public ScanFieldVM ViewModel
        {
            get => (ScanFieldVM)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public Point TopLeft
        {
            get => (Point)GetValue(TopLeftProperty);
            set => SetValue(TopLeftProperty, value);
        }

        public Point BottomRight
        {
            get => (Point)GetValue(BottomRightProperty);
            set => SetValue(BottomRightProperty, value);
        }

        public int Pitch
        {
            get => (int)GetValue(PitchProperty);
            set => SetValue(PitchProperty, value);
        }

        public int DotDiameter
        {
            get => (int)GetValue(DotDiameterProperty);
            set => SetValue(DotDiameterProperty, value);
        }

        public new Thickness BorderThickness
        {
            get => (Thickness)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        public new Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ScanFieldVM)value;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            TopLeft = new Point(0, 0);
            BottomRight = new Point(e.NewSize.Width, e.NewSize.Height);
        }
    }
}
