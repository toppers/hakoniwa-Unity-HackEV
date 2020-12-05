using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hakoniwa.Assets;

namespace Hakoniwa.Core
{
    public interface IIoWriter
    {
        void SetData(int off_byte, int data);
        void SetData64(int off, ulong data);
        void SetData64(int off, double data);
        void Send();
    }
}
