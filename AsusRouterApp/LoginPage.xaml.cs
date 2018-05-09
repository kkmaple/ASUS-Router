using AsusRouterApp.Class;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace AsusRouterApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public string host { get; set; } = "http://";

        public string user { get; set; } = "";

        public string pwd { get; set; } = "";

        public LoginPageBrush pageBrush = new LoginPageBrush();

        public LoginPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Fluent Design System兼容方案
        /// </summary>
        public class LoginPageBrush : INotifyPropertyChanged
        {
            public Brush mainGrid;
            public Brush btnGrid;

            public Utils.UI.AccentColor accentColor;

            public LoginPageBrush()
            {
                accentColor = new Utils.UI.AccentColor();
                accentColor.AccentColorChanged += async (value) =>
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.AcrylicBrush"))
                        {
                            //整个页面采用FDS
                            mainGrid = Utils.UI.GetAcrylicBrush(AcrylicBackgroundSource.HostBackdrop, accentColor.accentColor, Color.FromArgb(255, 70, 171, 255), 0.3);
                            btnGrid = Utils.UI.GetAcrylicBrush(AcrylicBackgroundSource.HostBackdrop, accentColor.accentColor, Color.FromArgb(255, 70, 171, 255), 0.6);
                        }
                        else
                        {
                            btnGrid = new SolidColorBrush(accentColor.accentColor);
                        }
                        RaisePropertyChanged("mainGrid");
                        RaisePropertyChanged("btnGrid");
                    });
                };
                if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.AcrylicBrush"))
                {
                    //整个页面采用FDS
                    mainGrid = Utils.UI.GetAcrylicBrush(AcrylicBackgroundSource.HostBackdrop, accentColor.accentColor, Color.FromArgb(255, 70, 171, 255), 0.3);
                    btnGrid = Utils.UI.GetAcrylicBrush(AcrylicBackgroundSource.HostBackdrop, accentColor.accentColor, Color.FromArgb(255, 70, 171, 255), 0.6);
                }
                else
                {
                    var imageSource = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/background.jpg") };
                    mainGrid = new ImageBrush() { ImageSource = imageSource, Stretch = Stretch.UniformToFill };
                    btnGrid = new SolidColorBrush(accentColor.accentColor);
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            public void RaisePropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if(!host.Contains("http"))
            {
                notificationError.Show("地址错误");
                return;
            }
            if (user.Length==0)
            {
                notificationError.Show("请输入账号");
                return;
            }
            if (pwd.Length == 0)
            {
                notificationError.Show("请输入密码");
                return;
            }
            RouterAPI.Url.Host = host;
            var loginRes = await RouterAPI.Login(user,pwd);
            if (loginRes)
                this.Frame.Navigate(typeof(MainPage), null);
            else
                notificationError.Show("登录失败", 2000);
        }
    }
}
