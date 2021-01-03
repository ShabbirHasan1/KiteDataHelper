using KiteDataHelper.Common.KiteStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace KiteDataHelper.Models
{
    public class KiteInstruments
    {
        private List<Instrument> _instruments;

        public KiteInstruments()
        {
            _instruments = new List<Instrument>();
        }

        public bool IsSet { get; set; }

        public List<Instrument> Instruments 
        {
            get
            {
                return _instruments;
            }

            set
            {
                _instruments = value;
            }
        }
    }
}
