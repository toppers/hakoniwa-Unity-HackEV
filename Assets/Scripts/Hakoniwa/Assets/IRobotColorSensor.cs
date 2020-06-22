using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hakoniwa.Assets;

namespace Hakoniwa.Assets
{
    public enum ColorNumber
    {
        COLOR_NONE = 0, //!< \~English None 			  \~Japanese 無色
        COLOR_BLACK = 1, //!< \~English Black 			  \~Japanese 黒
        COLOR_BLUE = 2, //!< \~English Blue  			  \~Japanese 青
        COLOR_GREEN = 3, //!< \~English Green  		  \~Japanese 緑
        COLOR_YELLOW = 4, //!< \~English Yellow 		  \~Japanese 黄
        COLOR_RED = 5, //!< \~English Red  			  \~Japanese 赤
        COLOR_WHITE = 6, //!< \~English White 		      \~Japanese 白
        COLOR_BROWN = 7, //!< \~English Brown 			  \~Japanese 茶
        TNUM_COLOR        //!< \~English Number of colors \~Japanese 識別できるカラーの数
    }

    public struct ColorRGB
    {
        public int r;
        public int g;
        public int b;
    }

    public interface IRobotColorSensor: IRobotSensor
    {
        ColorNumber GetColorId();
        float GetLightValue();
        void GetRgb(out ColorRGB value);
    }
}
