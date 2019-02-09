using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ReactiveUI;
using Xunit;

namespace ScanFieldPoints.Test
{
    public sealed class when_scanning_subject : with_scan_field_points
    {
        protected override void When()
        {

        }

        [Fact]
        public void bottom_right_change_all_points()
        {
            var vm = new ScanFieldVM(Bus);
            var originalPointCount = vm.AllPoints.Count;
            vm.BottomRight = new Point(500, 500);
            var newPointCount = vm.AllPoints.Count;
            Assert.IsOrBecomesTrue(() => newPointCount > originalPointCount);
        }

        [Fact]
        public void top_left_changes_all_points()
        {
            var vm = new ScanFieldVM(Bus);
            var originalPointCount = vm.AllPoints.Count;
            vm.TopLeft = new Point(25, 25);
            var newPointCount = vm.AllPoints.Count;
            Assert.IsOrBecomesTrue(() => newPointCount < originalPointCount);
        }

        [Fact]
        public void bottom_right_changes_masked_points()
        {
            var vm = new ScanFieldVM(Bus);
            var originalPointCount = vm.MaskedPoints.Count;
            vm.BottomRight = new Point(500, 500);
            var newPointCount = vm.MaskedPoints.Count;
            Assert.IsOrBecomesTrue(() => newPointCount > originalPointCount);
        }

        [Fact]
        public void top_left_changes_masked_points()
        {
            var vm = new ScanFieldVM(Bus);
            var originalPointCount = vm.MaskedPoints.Count;
            vm.TopLeft = new Point(25, 25);
            var newPointCount = vm.MaskedPoints.Count;
            Assert.IsOrBecomesTrue(() => newPointCount < originalPointCount);
        }
    }
}
