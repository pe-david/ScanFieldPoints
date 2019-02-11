﻿using ReactiveUI;
using System.Windows;

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
