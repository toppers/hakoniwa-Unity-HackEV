using Hakoniwa.PluggableAsset.Communication.Pdu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hakoniwa.PluggableAsset.Communication.Pdu.Accessor
{
    class TimeAccessor
    {
        private Pdu pdu;
        
        public TimeAccessor(Pdu pdu)
        {
        	this.pdu = pdu;
        }
        public UInt32 sec
        {
            set
            {
                pdu.SetData("sec", value);
            }
            get
            {
                return pdu.GetDataUInt32("sec");
            }
        }
        public UInt32 nanosec
        {
            set
            {
                pdu.SetData("nanosec", value);
            }
            get
            {
                return pdu.GetDataUInt32("nanosec");
            }
        }
    }
}
