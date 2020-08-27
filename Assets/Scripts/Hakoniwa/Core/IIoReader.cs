using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hakoniwa.Assets;

namespace Hakoniwa.Core
{
    public delegate void IoReaderCallback();
    public interface IIoReader
    {
        void RefData(int off_byte, out int data);
        void RefData64(int off, out ulong data);
        void SetCallback(IoReaderCallback func);
        void DoRun();
    }
}
