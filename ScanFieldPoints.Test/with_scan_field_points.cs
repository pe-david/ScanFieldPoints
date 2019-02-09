using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveDomain.Tests.Specifications;

namespace ScanFieldPoints.Test
{
    public class with_scan_field_points : MockRepositorySpecification, IDisposable
    {
        // ReSharper disable once InconsistentNaming
        private bool _disposed;

        static with_scan_field_points()
        {
            //Bootstrap.Load();
        }

        protected override void Given()
        {
            base.Given();
        }

        protected override void When()
        {
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
            }

            _disposed = true;
        }
    }
}
