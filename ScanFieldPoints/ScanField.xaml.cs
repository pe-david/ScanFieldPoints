using ReactiveUI;
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

        public ScanField()
        {
            InitializeComponent();

            ViewModel = new ScanFieldVM(Global.Bus);

            this.WhenActivated(
                d =>
                {
                    d(this.OneWayBind(ViewModel, vm => vm.MaskedPoints, v => v.Dots.ItemsSource));
                });
        }

        public ScanFieldVM ViewModel
        {
            get => (ScanFieldVM)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ScanFieldVM)value;
        }
    }
}
