using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hakoniwa.Core;

namespace Hakoniwa.Core
{


    public class IoWriter : MonoBehaviour
    {
#if VDEV_IO_MMAP
    public string filepath = null;
    private MmapFileWriter io;
#else
        /*************************************
         * UDP SETTINGS
         *************************************/
        public string host = null;
        public int port = 0;
        private UdpCommClient io;
        /*************************************/
#endif

        private IoBufferParameter bufferParams = null;

        public void Initialize()
        {
            GameObject hakoniwa = GameObject.Find("Hakoniwa");

            bufferParams = hakoniwa.GetComponentInChildren<IoBufferParameter>();

#if VDEV_IO_MMAP
            io = new MmapFileWriter();
            io.DoStart(this.filepath);
#else
            /*************************************
             * UDP SETTINGS
             *************************************/
            io = new UdpCommClient();
            io.host = host;
            io.port = port;
            /*************************************/
            io.DoStart();
#endif
            Debug.Log("params=" + bufferParams);
        }


        public void Set(string name, int value1)
        {
            int off = bufferParams.GetSensorOffset(name);
            if (off > 0)
            {
                //Debug.Log("off=" + off + ": value=" + value1);
                io.SetData(off, value1);
            }
        }
        public void SetSimTime(ulong time)
        {
            io.SetData64(0, time);
        }


        public void Publish()
        {
            io.Send();
        }
    }

}

