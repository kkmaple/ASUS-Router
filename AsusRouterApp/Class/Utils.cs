using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace AsusRouterApp.Class
{
    public class Utils
    {
        public class UI
        {
            public static AcrylicBrush GetAcrylicBrush(AcrylicBackgroundSource acrylicBackground,Color TintColor, Color FallbackColor,double TintOpacity)
            {
                var brush = new AcrylicBrush();
                brush.BackgroundSource = acrylicBackground;
                brush.TintColor = TintColor;
                brush.FallbackColor = FallbackColor;
                brush.TintOpacity = TintOpacity;
                return brush;
            }
        }
    }
}
