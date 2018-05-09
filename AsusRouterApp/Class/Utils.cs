using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Threading;
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

            /// <summary>
            /// 循环查询用户主题色,变化时事件通知
            /// </summary>
            public class AccentColor
            {
                public Color accentColor;

                private TimeSpan updateSpan;

                private ThreadPoolTimer updateTimer;

                public delegate void AccentColorChangedDelegate(Color value);

                public event AccentColorChangedDelegate AccentColorChanged;

                public AccentColor()
                {
                    accentColor = Edi.UWP.Helpers.UI.GetAccentColor();
                    updateSpan = TimeSpan.FromMilliseconds(1000);
                    updateTimer = ThreadPoolTimer.CreatePeriodicTimer(TimerEvent,updateSpan);
                }

                private void TimerEvent(ThreadPoolTimer timer)
                {
                    var temp = Edi.UWP.Helpers.UI.GetAccentColor();
                    if(temp!=accentColor)
                    {
                        accentColor = temp;
                        AccentColorChanged?.Invoke(accentColor);
                    }
                }
            }
        }
    }
}
