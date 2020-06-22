using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hakoniwa.Assets;

namespace Hakoniwa.Core
{
    public interface IAssetController
    {
        bool IsConnected();
        long GetControllerTime();
        void DoUpdate();
        void DoPublish(long hakoniwa_time);
    }
}
