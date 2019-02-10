using ReactiveUI;
using System.Windows;

namespace ScanFieldPoints
{
    /// <summary>
    /// Interaction logic for ScanPoint.xaml
    /// </summary>
    public partial class ScanPoint : IViewFor<PointVM>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                                        "ViewModel",
                                        typeof(PointVM),
                                        typeof(ScanPoint),
                                        new PropertyMetadata(default(PointVM)));

        public ScanPoint()
        {
            InitializeComponent();

            this.WhenActivated(
                d =>
                {
                    d(this.OneWayBind(ViewModel, vm => vm.PointSize, v => v.TheDot.Width));
                    d(this.OneWayBind(ViewModel, vm => vm.PointSize, v => v.TheDot.Height));
                });
        }

        public PointVM ViewModel
        {
            get => (PointVM)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (PointVM)value;
        }
    }
}
