﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace AsusRouterApp.Class
{
    public class Setting
    {
        public static ApplicationDataContainer localSetting = ApplicationData.Current.RoamingSettings;

        public static void SetSetting(string key,string value)
        {
            localSetting.Values[key] = value;
        }

        public static object GetSetting(string key,string def)
        {
            if(localSetting.Values.ContainsKey(key))
            {
                return localSetting.Values[key];
            }
            else
            {
                localSetting.Values[key] = def;
                return def;
            }
        }

        public class Data
        {
            /// <summary>
            /// 导出数据
            /// </summary>
            /// <returns></returns>
            public static string Export()
            {
                var res = new DataModel();
                res.devName = DeviceName.GetDeviceName();
                res.devType = DeviceType.GetDeviceType();
                return JsonConvert.SerializeObject(res);
            }

            /// <summary>
            /// 导入数据
            /// </summary>
            /// <param name="json"></param>
            public static void Load(string json)
            {
                try
                {
                    DataModel data = JsonConvert.DeserializeObject<DataModel>(json);
                    if(data.devName!=null)
                    {
                        foreach (var mac in data.devName.Keys)
                        {
                            DeviceName.SetDeviceName(mac, data.devName[mac]);
                        }
                    }
                    if (data.devType != null)
                    {
                        foreach (var mac in data.devType.Keys)
                        {
                            DeviceType.SetDeviceType(mac, (Model.DeviceType)data.devType[mac]);
                        }
                    }
                }
                catch (Exception)
                {
                    
                }
            }

            public class DataModel
            {
                public Dictionary<string, string> devName { get; set; }
                public Dictionary<string, int> devType { get; set; }
            }
        }

        public class DeviceName
        {
            public static Dictionary<string,string> GetDeviceName()
            {
                ApplicationDataContainer deviceNamesContainers = null;
                if (!localSetting.Containers.ContainsKey("decviceNames"))
                    localSetting.CreateContainer("decviceNames", ApplicationDataCreateDisposition.Always);
                else
                    deviceNamesContainers = localSetting.Containers["decviceNames"];
                var res = new Dictionary<string, string>();
                foreach (var item in deviceNamesContainers.Values.Keys)
                {
                    res.Add(item, (string)deviceNamesContainers.Values[item]);
                }
                return res;
            }

            public static string GetDeviceName(string mac, string def)
            {
                ApplicationDataContainer deviceNamesContainers = null;
                if (!localSetting.Containers.ContainsKey("decviceNames"))
                    localSetting.CreateContainer("decviceNames", ApplicationDataCreateDisposition.Always);
                else
                    deviceNamesContainers = localSetting.Containers["decviceNames"];
                if (deviceNamesContainers.Values.ContainsKey(mac))
                {
                    return (string)deviceNamesContainers.Values[mac];
                }
                else
                {
                    deviceNamesContainers.Values[mac] = def;
                    return def;
                }
            }

            public static void SetDeviceName(string mac, string name)
            {
                ApplicationDataContainer deviceNamesContainers = null;
                if (!localSetting.Containers.ContainsKey("decviceNames"))
                    localSetting.CreateContainer("decviceNames", ApplicationDataCreateDisposition.Always);
                else
                    deviceNamesContainers = localSetting.Containers["decviceNames"];
                deviceNamesContainers.Values[mac] = name;
            }

            public static bool ContainsDeviceName(string mac)
            {
                ApplicationDataContainer deviceNamesContainers = null;
                if (!localSetting.Containers.ContainsKey("decviceNames"))
                    localSetting.CreateContainer("decviceNames", ApplicationDataCreateDisposition.Always);
                else
                    deviceNamesContainers = localSetting.Containers["decviceNames"];
                return deviceNamesContainers.Values.ContainsKey(mac);
            }
        }

        public class DeviceType
        {
            public static Dictionary<string, int> GetDeviceType()
            {
                ApplicationDataContainer deviceNamesContainers = null;
                if (!localSetting.Containers.ContainsKey("decviceType"))
                    localSetting.CreateContainer("decviceType", ApplicationDataCreateDisposition.Always);
                else
                    deviceNamesContainers = localSetting.Containers["decviceType"];
                var res = new Dictionary<string, int>();
                foreach (var item in deviceNamesContainers.Values.Keys)
                {
                    res.Add(item, (int)deviceNamesContainers.Values[item]);
                }
                return res;
            }

            public static Model.DeviceType GetDeviceType(string mac, Model.DeviceType def)
            {
                ApplicationDataContainer deviceTypeContainers = null;
                if (!localSetting.Containers.ContainsKey("decviceType"))
                    localSetting.CreateContainer("decviceType", ApplicationDataCreateDisposition.Always);
                else
                    deviceTypeContainers = localSetting.Containers["decviceType"];
                if (deviceTypeContainers.Values.ContainsKey(mac))
                {
                    return (Model.DeviceType)((int)deviceTypeContainers.Values[mac]);
                }
                else
                {
                    deviceTypeContainers.Values[mac] = (int)def;
                    return def;
                }
            }

            public static void SetDeviceType(string mac, Model.DeviceType type)
            {
                ApplicationDataContainer deviceTypeContainers = null;
                if (!localSetting.Containers.ContainsKey("decviceType"))
                    localSetting.CreateContainer("decviceType", ApplicationDataCreateDisposition.Always);
                else
                    deviceTypeContainers = localSetting.Containers["decviceType"];
                deviceTypeContainers.Values[mac] = (int)type;
            }

            public static bool ContainsDeviceType(string mac)
            {
                ApplicationDataContainer deviceTypeContainers = null;
                if (!localSetting.Containers.ContainsKey("decviceType"))
                    localSetting.CreateContainer("decviceType", ApplicationDataCreateDisposition.Always);
                else
                    deviceTypeContainers = localSetting.Containers["decviceType"];
                return deviceTypeContainers.Values.ContainsKey(mac);
            }

            public static Model.DeviceType GetDefType(bool isWL,string name=null)
            {
                if(name!=null)
                {
                    if (name.ToLower().Contains("laptop"))
                    {
                        return Model.DeviceType.Laptop;
                    }
                    else if(name.ToLower().Contains("mac"))
                    {
                        return Model.DeviceType.MAC;
                    }
                    else if (name.ToLower().Contains("pad"))
                    {
                        return Model.DeviceType.PAD;
                    }
                    else if (name.ToLower().Contains("phone"))
                    {
                        return Model.DeviceType.Phone;
                    }
                    else if (name.ToLower().Contains("tv"))
                    {
                        return Model.DeviceType.TV;
                    }
                    else if (name.ToLower().Contains("pc"))
                    {
                        return Model.DeviceType.PC;
                    }
                    else if (name.ToLower().Contains("windows"))
                    {
                        return Model.DeviceType.Windows;
                    }
                    else if (name.ToLower().Contains("desktop"))
                    {
                        return Model.DeviceType.Windows;
                    }
                    else if (name.ToLower().Contains("android"))
                    {
                        return Model.DeviceType.Android;
                    }
                    else if (name.ToLower().Contains("linux"))
                    {
                        return Model.DeviceType.Linux;
                    }
                    else if (name.ToLower().Contains("ubuntu"))
                    {
                        return Model.DeviceType.Linux;
                    }
                    else if (name.ToLower().Contains("nas"))
                    {
                        return Model.DeviceType.NAS_Server;
                    }
                    else if (name.ToLower().Contains("printer"))
                    {
                        return Model.DeviceType.Printer;
                    }
                    else if (name.ToLower().Contains("scanner"))
                    {
                        return Model.DeviceType.Scanner;
                    }
                    else
                    {
                        if (isWL)
                            return Model.DeviceType.Unknown_Wireless;
                        else
                            return Model.DeviceType.Unknown_Wired;
                    }
                }
                else
                {
                    if(isWL)
                        return Model.DeviceType.Unknown_Wireless;
                    else
                        return Model.DeviceType.Unknown_Wired;
                }
            }
        }
    }
}
