using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveDomain.Bus;

namespace ScanFieldPoints
{
    public static class Global
    {
        static Global()
        {
            Bus = new CommandBus(
                "Main Bus",
                false,
                TimeSpan.FromMinutes(1),
                TimeSpan.FromMinutes(1));
        }

        public static IGeneralBus Bus { get; }
    }
}
