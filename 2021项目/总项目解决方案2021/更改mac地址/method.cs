using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class method
    {
        // Token: 0x0200000E RID: 14
        internal class SetMac
        {
            // Token: 0x06000073 RID: 115 RVA: 0x000052E0 File Offset: 0x000034E0
            public static void ModifyMac(string desc)
            {
                foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (networkInterface.Description.ToString() == desc)
                    {
                        networkInterface.Description.ToString();
                    }
                }
            }

            // Token: 0x06000074 RID: 116 RVA: 0x00005324 File Offset: 0x00003524
            public static void FlowModifyMac(string mac)
            {
                foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (networkInterface.NetworkInterfaceType.ToString() == "Ethernet")
                    {
                        SetMac.FlowSetMACAddress(networkInterface.Id, mac);
                    }
                }
            }

            // Token: 0x06000075 RID: 117 RVA: 0x00005378 File Offset: 0x00003578
            public static void FlowSetMACAddress(string networkid, string mac)
            {
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\{4D36E972-E325-11CE-BFC1-08002BE10318}", true);
                foreach (string name in registryKey.GetSubKeyNames())
                {
                    try
                    {
                        RegistryKey registryKey2 = registryKey.OpenSubKey(name, true);
                        if (registryKey2.GetValue("NetCfgInstanceId").ToString() == networkid)
                        {
                            registryKey2.SetValue("NetworkAddress", mac);
                            RegistryKey registryKey3 = registryKey2.OpenSubKey("Ndi\\Params\\NetworkAddress", true);
                            registryKey3.SetValue("default", mac);
                            registryKey3.Close();
                        }
                        registryKey2.Close();
                    }
                    catch
                    {
                    }
                }
                registryKey.Close();
            }

            // Token: 0x06000076 RID: 118 RVA: 0x00005420 File Offset: 0x00003620
            public static string ExtractModifyMac()
            {
                string result = "";
                foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (networkInterface.NetworkInterfaceType.ToString() == "Ethernet")
                    {
                        result = SetMac.ExtractSetMACAddress(networkInterface.Id);
                        ManagementObject network = SetMac.NetWork(networkInterface.Description);
                        SetMac.DisableNetWork(network);
                        SetMac.EnableNetWork(network);
                    }
                }
                return result;
            }

            // Token: 0x06000077 RID: 119 RVA: 0x00005494 File Offset: 0x00003694
            public static string ExtractSetMACAddress(string networkid)
            {
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\{4D36E972-E325-11CE-BFC1-08002BE10318}", true);
                string text = SetMac.CreateNewMacAddress();
                foreach (string name in registryKey.GetSubKeyNames())
                {
                    try
                    {
                        RegistryKey registryKey2 = registryKey.OpenSubKey(name, true);
                        if (registryKey2.GetValue("NetCfgInstanceId").ToString() == networkid)
                        {
                            registryKey2.SetValue("NetworkAddress", text);
                        }
                        registryKey2.Close();
                    }
                    catch
                    {
                    }
                }
                registryKey.Close();
                return text;
            }

            // Token: 0x06000078 RID: 120 RVA: 0x00005528 File Offset: 0x00003728
            public static int GetRandomSeed()
            {
                byte[] array = new byte[4];
                new RNGCryptoServiceProvider().GetBytes(array);
                return BitConverter.ToInt32(array, 0);
            }

            // Token: 0x06000079 RID: 121 RVA: 0x00005550 File Offset: 0x00003750
            public static string CreateNewMacAddress()
            {
                string[] array = new string[]
                {
                "E0-D5-5E",
                "00-1F-D0",
                "04-8D-38",
                "0C-B6-D2",
                "04-95-E6",
                "B0-26-28",
                "18-C0-86",
                "00-10-18",
                "38-BA-B0",
                "EC-63-D7",
                "94-E2-3C",
                "14-85-7F",
                "98-E7-43",
                "10-32-7E",
                "A4-45-19",
                "D0-D0-03",
                "60-3A-7C",
                "60-3A-7C",
                "D0-9C-7A",
                "8C-79-F5",
                "54-A7-03",
                "C8-F7-50",
                "10-C5-95",
                "6C-2B-59",
                "A4-11-94",
                "B0-BE-76"
                };
                int num = new Random().Next(0, array.Length);
                int minValue = 0;
                int maxValue = 16;
                Random random = new Random(SetMac.GetRandomSeed());
                return array[num] + string.Format("-{0}{1}-{2}{3}-{4}{5}", new object[]
                {
                random.Next(minValue, maxValue).ToString("x"),
                random.Next(minValue, maxValue).ToString("x"),
                random.Next(minValue, maxValue).ToString("x"),
                random.Next(minValue, maxValue).ToString("x"),
                random.Next(minValue, maxValue).ToString("x"),
                random.Next(minValue, maxValue).ToString("x")
                }).ToUpper();
            }

            // Token: 0x0600007A RID: 122 RVA: 0x00005720 File Offset: 0x00003920
            public static bool DisableNetWork(ManagementObject network)
            {
                bool result;
                try
                {
                    network.InvokeMethod("Disable", null);
                    result = true;
                }
                catch
                {
                    result = false;
                }
                return result;
            }

            // Token: 0x0600007B RID: 123 RVA: 0x00005754 File Offset: 0x00003954
            public static bool EnableNetWork(ManagementObject network)
            {
                bool result;
                try
                {
                    network.InvokeMethod("Enable", null);
                    result = true;
                }
                catch
                {
                    result = false;
                }
                return result;
            }

            // Token: 0x0600007C RID: 124 RVA: 0x00005788 File Offset: 0x00003988
            public static bool NetWorkState(string netWorkName)
            {
                using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementObjectSearcher("SELECT * From Win32_NetworkAdapter").Get().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        if (((ManagementObject)enumerator.Current)["Name"].ToString() == netWorkName)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            // Token: 0x0600007D RID: 125 RVA: 0x000057FC File Offset: 0x000039FC
            public static ManagementObject NetWork(string networkname)
            {
                foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("SELECT * From Win32_NetworkAdapter").Get())
                {
                    ManagementObject managementObject = (ManagementObject)managementBaseObject;
                    if (managementObject["Name"].ToString() == networkname)
                    {
                        return managementObject;
                    }
                }
                return null;
            }

            // Token: 0x0600007E RID: 126 RVA: 0x00005870 File Offset: 0x00003A70
            public static string GetMac(string desc)
            {
                foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (networkInterface.Description.ToString() == desc)
                    {
                        return networkInterface.GetPhysicalAddress().ToString();
                    }
                }
                return "";
            }
        }

    }
}
