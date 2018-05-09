using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace AsusRouterApp.Class
{
    public class Model
    {
        public class CpuMemInfo
        {
            public CPU cpu_usage { get; set; }
            
            public MEM memory_usage { get; set; }

            public class CPU
            {
                public long cpu1_total { get; set; }
                public long cpu1_usage { get; set; }
                public long cpu2_total { get; set; }
                public long cpu2_usage { get; set; }
            }

            public class MEM
            {
                public long mem_total { get; set; }
                public long mem_free { get; set; }
                public long mem_used { get; set; }
            }
        }
        
        public class NetRate
        {
            public long internet_rx
            {
                get
                {
                    if (INTERNET_rx == null || INTERNET_rx.Length == 0) return -1;
                    return Convert.ToInt64(INTERNET_rx, 16);
                }
            }

            public long internet_tx
            {
                get
                {
                    if (INTERNET_tx == null || INTERNET_tx.Length == 0) return -1;
                    return Convert.ToInt64(INTERNET_tx, 16);
                }
            }

            public long wl2g_rx
            {
                get
                {
                    if (WIRELESS0_rx == null || WIRELESS0_rx.Length == 0) return -1;
                    return Convert.ToInt64(WIRELESS0_rx, 16);
                }
            }

            public long wl2g_tx
            {
                get
                {
                    if (WIRELESS0_tx == null || WIRELESS0_tx.Length == 0) return -1;
                    return Convert.ToInt64(WIRELESS0_tx, 16);
                }
            }

            public long wl5g_rx
            {
                get
                {
                    if (WIRELESS1_rx == null || WIRELESS1_rx.Length == 0) return -1;
                    return Convert.ToInt64(WIRELESS1_rx, 16);
                }
            }

            public long wl5g_tx
            {
                get
                {
                    if (WIRELESS1_tx == null || WIRELESS1_tx.Length == 0) return -1;
                    return Convert.ToInt64(WIRELESS1_tx, 16);
                }
            }

            public string INTERNET_rx { get; set; }
            public string INTERNET_tx { get; set; }
            public string WIRELESS0_rx { get; set; }
            public string WIRELESS0_tx { get; set; }
            public string WIRELESS1_rx { get; set; }
            public string WIRELESS1_tx { get; set; }
        }

        public class WLANInfo
        {
            public int wl0_radio { get; set; }
            public string wl0_ssid { get; set; }
            public string wl0_wpa_psk { get; set; }
            public int wl1_radio { get; set; }
            public string wl1_ssid { get; set; }
            public string wl1_wpa_psk { get; set; }

            public bool wl0_enable
            {
                get
                {
                    if (wl0_radio == 1)
                        return true;
                    else
                        return false;
                }
                set
                {
                    if (value)
                        wl0_radio = 1;
                    else
                        wl0_radio = 0;
                }
            }

            public bool wl1_enable
            {
                get
                {
                    if (wl1_radio == 1)
                        return true;
                    else
                        return false;
                }
                set
                {
                    if (value)
                        wl1_radio = 1;
                    else
                        wl1_radio = 0;
                }
            }
        }
        
        public class WANInfo
        {
            [JsonProperty("model")]
            public string model { get; set; }
            [JsonProperty("daapd_friendly_name")]
            public string name { get; set; }
            [JsonProperty("0:macaddr")]
            public string mac { get; set; }
            [JsonProperty("ddns_hostname_x")]
            public string ddns { get; set; }
            [JsonProperty("wanlink-status")]
            public int status { get; set; }
            [JsonProperty("wanlink-statusstr")]
            public string statusstr { get; set; }
            [JsonProperty("wanlink-ipaddr")]
            public string ipaddr { get; set; }
            [JsonProperty("wanlink-dns")]
            public string dns { get; set; }
        }

        public class Client
        {
            public List<string> macList { get; set; }

            public Dictionary<string,ClientInfo> clientList { get; set; }

            public List<ClientInfo> LAN
            {
                get
                {
                    var res = new List<ClientInfo>();
                    foreach (var item in clients)
                    {
                        if (!item.isWL) res.Add(item);
                    }
                    return res;
                }
            }

            public List<ClientInfo> WL5G
            {
                get
                {
                    var res = new List<ClientInfo>();
                    foreach (var item in clients)
                    {
                        if (item.isOnline&&item.isWL&&item.netType == NetType.WL5G) res.Add(item);
                    }
                    return res;
                }
            }

            public List<ClientInfo> WL2G
            {
                get
                {
                    var res = new List<ClientInfo>();
                    foreach (var item in clients)
                    {
                        if (item.isOnline && item.isWL && item.netType == NetType.WL2G) res.Add(item);
                    }
                    return res;
                }
            }

            public List<ClientInfo> clients
            {
                get
                {
                    var res = new List<ClientInfo>();
                    foreach (var mac in macList)
                    {
                        if (clientList.ContainsKey(mac)&& clientList[mac].isOnline) res.Add(clientList[mac]);
                    }
                    return res;
                }
            }

            public Dictionary<string,WlanInfo> wl5g { get; set; }

            public Dictionary<string,WlanInfo> wl2g { get; set; }

            /// <summary>
            /// 更新设备名称
            /// </summary>
            public void UpdateDeviceName()
            {
                if (clientList != null)
                {
                    for (int i = 0; i < macList.Count; i++)
                    {
                        try
                        {
                            if (clientList[macList[i]] != null)
                            {
                                clientList[macList[i]].name = Setting.DeviceName.GetDeviceName(macList[i], clientList[macList[i]].name);
                                if(clientList[macList[i]].name==null|| clientList[macList[i]].name.Length==0)
                                {
                                    Setting.DeviceName.SetDeviceName(clientList[macList[i]].mac, clientList[macList[i]].mac);
                                    clientList[macList[i]].name = clientList[macList[i]].mac;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
            }

            /// <summary>
            /// 更新设备类型
            /// </summary>
            public void UpdateDeviceType()
            {
                if (clientList != null)
                {
                    for (int i = 0; i < macList.Count; i++)
                    {
                        try
                        {
                            if (clientList[macList[i]] != null)
                            {
                                clientList[macList[i]].deviceType = Setting.DeviceType.GetDeviceType(macList[i], Setting.DeviceType.GetDefType(clientList[macList[i]].isWL, clientList[macList[i]].name));
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
            }

            public void Update()
            {
                UpdateDeviceName();
                foreach (var mac in macList)
                {
                    if (clientList.ContainsKey(mac))
                    {
                        clientList[mac].isWL = false;
                        if (wl2g.ContainsKey(mac))
                        {
                            clientList[mac].isWL = true;
                            clientList[mac].netType = NetType.WL2G;
                            clientList[mac].wlanInfo = wl2g[mac];
                        }else if (wl5g.ContainsKey(mac))
                        {
                            clientList[mac].isWL = true;
                            clientList[mac].netType = NetType.WL5G;
                            clientList[mac].wlanInfo = wl5g[mac];
                        }
                        if (!clientList[mac].isWL) clientList[mac].netType = NetType.LAN;
                    }
                }
                UpdateDeviceType();
            }

            public class ClientInfo : INotifyPropertyChanged
            {
                public string name { get; set; }
                public string ip { get; set; }
                public string mac { get; set; }
                public string from { get; set; }
                public bool isOnline { get; set; }
                public bool isWL { get; set; }
                public NetType netType { get; set; }
                public WlanInfo wlanInfo { get; set; }
                public DeviceRate rate { get; set; }

                public DeviceType deviceType { get; set; } = DeviceType.Unknown_Wired;
                public bool isBan { get; set; } = false;

                public BitmapImage netTypeImage
                {
                    get
                    {
                        string path = "ms-appx:///Assets/icon/";
                        switch (netType)
                        {
                            case Model.Client.NetType.LAN:
                                path += "icon_signal_wired.png";
                                break;
                            case Model.Client.NetType.WL2G:
                                path += "icon_signal_24g.png";
                                break;
                            case Model.Client.NetType.WL5G:
                                path += "icon_signal_5g.png";
                                break;
                            default:
                                path += "icon_signal_wired.png";
                                break;
                        }
                        return new BitmapImage() { UriSource = new Uri(path) };
                    }
                }

                public BitmapImage devTypeImage
                {
                    get
                    {
                        string path = "ms-appx:///Assets/icon/device/";
                        switch (deviceType)
                        {
                            case DeviceType.Laptop:
                                path += "device_laptop.png";
                                break;
                            case DeviceType.MAC:
                                path += "device_mac.png";
                                break;
                            case DeviceType.PAD:
                                path += "device_pad.png";
                                break;
                            case DeviceType.Phone:
                                path += "device_phone.png";
                                break;
                            case DeviceType.TV:
                                path += "device_tv.png";
                                break;
                            case DeviceType.PC:
                                path += "device_windows.png";
                                break;
                            case DeviceType.Windows:
                                path += "icon_windows.png";
                                break;
                            case DeviceType.Android:
                                path += "icon_android.png";
                                break;
                            case DeviceType.Linux:
                                path += "icon_linux_pc.png";
                                break;
                            case DeviceType.NAS_Server:
                                path += "icon_nas_server.png";
                                break;
                            case DeviceType.Printer:
                                path += "icon_printer.png";
                                break;
                            case DeviceType.Repeater:
                                path += "icon_repeater.png";
                                break;
                            case DeviceType.Scanner:
                                path += "icon_scanner.png";
                                break;
                            case DeviceType.Unknown_Wired:
                                path += "icon_unknown_wired.png";
                                break;
                            case DeviceType.Unknown_Wireless:
                                path += "icon_unknown_wireless.png";
                                break;
                            default:
                                path += "icon_unknown_wired.png";
                                break;
                        }
                        return new BitmapImage() { UriSource = new Uri(path) };
                    }
                }

                public BitmapImage devBanImage
                {
                    get
                    {
                        if (isBan)
                        {
                            return new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/icon/icon_mac_filter_block.png") };
                        }
                        else
                            return null;
                    }
                }

                public void UpdateRate(DeviceRate rate)
                {
                    this.rate = new DeviceRate()
                    {
                        rx=rate.rx / 1024 / 1024,
                        tx=rate.tx / 1024 / 1024
                    };
                    RaisePropertyChanged("rate");
                }

                public void UpdateBanState(string[] banList)
                {
                    var query = banList.Where(o => o == this.mac).ToArray();
                    if (query.Length == 0)
                        this.isBan = false;
                    else
                        this.isBan = true;
                    RaisePropertyChanged("isBan");
                    RaisePropertyChanged("devBanImage");
                }

                public event PropertyChangedEventHandler PropertyChanged;
                public void RaisePropertyChanged(string name)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                }
            }
            
            public enum NetType
            {
                LAN,WL2G, WL5G
            }

            public class WlanInfo
            {
                public string isWL { get; set; }
                public string rssi { get; set; }
            }

            public List<ClientInGroup> GetGroup()
            {
                var res = new List<ClientInGroup>();
                res.Add(new ClientInGroup("有线", LAN));
                res.Add(new ClientInGroup("5G", WL5G));
                res.Add(new ClientInGroup("2.4G", WL2G));
                return res;
            }

            public class ClientInGroup : INotifyPropertyChanged
            {
                public string key { get; set; }

                public string name
                {
                    get
                    {
                        string res = key;
                        int num = 0;
                        if (Clients != null) num = Clients.Count;
                        res = res + " ("+num+")";
                        return res;
                    }
                }

                public ObservableCollection<ClientInfo> Clients { get; set; }

                public ClientInGroup() { }

                public ClientInGroup(string key,IEnumerable<ClientInfo> collection) {
                    this.key = key;
                    this.Clients = new ObservableCollection<ClientInfo>(collection);
                }

                public void AsyncList(IEnumerable<ClientInfo> data)
                {
                    for (int i = 0; i < Clients.Count; i++)
                    {
                        var query = data.Where(o => o.mac == Clients[i].mac).ToArray();
                        if (query.Length == 0) Clients.RemoveAt(i);
                    }
                    foreach (var item in data)
                    {
                        var query = Clients.Where(o => o.mac == item.mac).ToArray();
                        if (query.Length == 0) Clients.Add(item);
                    }
                    RaisePropertyChanged("name");
                }

                public event PropertyChangedEventHandler PropertyChanged;
                public void RaisePropertyChanged(string name)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                }
            }
        }

        public enum DeviceType
        {
            Laptop, //笔记本
            MAC,  //MAC
            PAD, //平板
            Phone,  //电话
            TV,  //电视
            PC, //PC
            Windows, //Windows
            Android, //安卓
            Linux,  //Linux
            NAS_Server,  //网络存储器
            Printer,  //打印机
            Repeater, //网络设备
            Scanner,   //扫描仪
            Unknown_Wired,   //未知的有线设备
            Unknown_Wireless  //未知的无线设备
        }

        public class DeviceRate
        {
            public long rx { get; set; }
            public long tx { get; set; }
        }
    }
}