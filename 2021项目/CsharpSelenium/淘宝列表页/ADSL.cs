using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace 淘宝列表页
{
    class ADSL
    {
        public struct RASCONN
        {
            public int dwSize;
            public IntPtr hrasconn;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 257)]
            public string szEntryName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
            public string szDeviceType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
            public string szDeviceName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct RasStats
        {
            public int dwSize;
            public int dwBytesXmited;
            public int dwBytesRcved;
            public int dwFramesXmited;
            public int dwFramesRcved;
            public int dwCrcErr;
            public int dwTimeoutErr;
            public int dwAlignmentErr;
            public int dwHardwareOverrunErr;
            public int dwFramingErr;
            public int dwBufferOverrunErr;
            public int dwCompressionRatioIn;
            public int dwCompressionRatioOut;
            public int dwBps;
            public int dwConnectionDuration;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct RasEntryName
        {
            public int dwSize;
            //[MarshalAs(UnmanagedType.ByValTStr,SizeConst=(int)RasFieldSizeConstants.RAS_MaxEntryName + 1)]
            public string szEntryName;
            //#if WINVER5
            // public int dwFlags;
            // [MarshalAs(UnmanagedType.ByValTStr,SizeConst=260+1)]
            // public string szPhonebookPath;
            //#endif
        }
        public class RAS
        {
            [DllImport("Rasapi32.dll", EntryPoint = "RasEnumConnectionsA",
               SetLastError = true)]

            internal static extern int RasEnumConnections
              (
              ref RASCONN lprasconn, // buffer to receive connections data
              ref int lpcb, // size in bytes of buffer
              ref int lpcConnections // number of connections written to buffer
              );


            [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]
            internal static extern uint RasGetConnectionStatistics(
              IntPtr hRasConn,    // handle to the connection
              [In, Out]RasStats lpStatistics // buffer to receive statistics
              );
            [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]
            public extern static uint RasHangUp(
              IntPtr hrasconn // handle to the RAS connection to hang up
              );

            [DllImport("rasapi32.dll", CharSet = CharSet.Auto)]
            public extern static uint RasEnumEntries(
              string reserved,       // reserved, must be NULL
              string lpszPhonebook,     // pointer to full path and
                                        // file name of phone-book file
              [In, Out]RasEntryName[] lprasentryname, // buffer to receive
                                                      // phone-book entries
              ref int lpcb,         // size in bytes of buffer
              out int lpcEntries       // number of entries written
                                       // to buffer
              );

            [DllImport("wininet.dll", CharSet = CharSet.Auto)]
            public extern static int InternetDial(
              IntPtr hwnd,
              [In]string lpszConnectoid,
              uint dwFlags,
              ref int lpdwConnection,
              uint dwReserved
              );

            public RAS()
            {
            }
        }
        public enum DEL_CACHE_TYPE //要删除的类型。
        {
            File,//表示internet临时文件
            Cookie //表示Cookie
        }

        public class RASDisplay
        {
            [DllImport("wininet.dll", CharSet = CharSet.Auto)]
            public static extern bool DeleteUrlCacheEntry(
              DEL_CACHE_TYPE type
              );
            private string m_duration;
            private string m_ConnectionName;
            private string[] m_ConnectionNames;
            private double m_TX;
            private double m_RX;
            private bool m_connected;
            private IntPtr m_ConnectedRasHandle;

            RasStats status = new RasStats();
            public RASDisplay()
            {
                m_connected = true;

                RAS lpras = new RAS();
                RASCONN lprasConn = new RASCONN();

                lprasConn.dwSize = Marshal.SizeOf(typeof(RASCONN));
                lprasConn.hrasconn = IntPtr.Zero;

                int lpcb = 0;
                int lpcConnections = 0;
                int nRet = 0;
                lpcb = Marshal.SizeOf(typeof(RASCONN));

                nRet = RAS.RasEnumConnections(ref lprasConn, ref lpcb, ref
                  lpcConnections);

                if (nRet != 0)
                {
                    m_connected = false;
                    return;

                }

                if (lpcConnections > 0)
                {
                    //for (int i = 0; i < lpcConnections; i++)

                    //{
                    RasStats stats = new RasStats();

                    m_ConnectedRasHandle = lprasConn.hrasconn;
                    RAS.RasGetConnectionStatistics(lprasConn.hrasconn, stats);


                    m_ConnectionName = lprasConn.szEntryName;

                    int Hours = 0;
                    int Minutes = 0;
                    int Seconds = 0;

                    Hours = ((stats.dwConnectionDuration / 1000) / 3600);
                    Minutes = ((stats.dwConnectionDuration / 1000) / 60) - (Hours * 60);
                    Seconds = ((stats.dwConnectionDuration / 1000)) - (Minutes * 60) - (Hours * 3600);


                    m_duration = Hours + " hours " + Minutes + " minutes " + Seconds + " secs";
                    m_TX = stats.dwBytesXmited;
                    m_RX = stats.dwBytesRcved;
                    //}
                }
                else
                {
                    m_connected = false;
                }


                int lpNames = 1;
                int entryNameSize = 0;
                int lpSize = 0;
                RasEntryName[] names = null;

                entryNameSize = Marshal.SizeOf(typeof(RasEntryName));
                lpSize = lpNames * entryNameSize;

                names = new RasEntryName[lpNames];
                names[0].dwSize = entryNameSize;

                uint retval = RAS.RasEnumEntries(null, null, names, ref lpSize, out lpNames);

                //if we have more than one connection, we need to do it again
                if (lpNames > 1)
                {
                    names = new RasEntryName[lpNames];
                    for (int i = 0; i < names.Length; i++)
                    {
                        names[i].dwSize = entryNameSize;
                    }

                    retval = RAS.RasEnumEntries(null, null, names, ref lpSize, out lpNames);

                }
                m_ConnectionNames = new string[names.Length];


                if (lpNames > 0)
                {
                    for (int i = 0; i < names.Length; i++)
                    {
                        m_ConnectionNames[i] = names[i].szEntryName;
                    }
                }
            }

            public string Duration
            {
                get
                {
                    return m_connected ? m_duration : "";
                }
            }

            public string[] Connections
            {
                get
                {
                    return m_ConnectionNames;
                }
            }

            public double BytesTransmitted
            {
                get
                {
                    return m_connected ? m_TX : 0;
                }
            }
            public double BytesReceived
            {
                get
                {
                    return m_connected ? m_RX : 0;

                }
            }
            public string ConnectionName
            {
                get
                {
                    return m_connected ? m_ConnectionName : "";
                }
            }
            public bool IsConnected
            {
                get
                {
                    return m_connected;
                }
            }

            public int Connect(string Connection)
            {
                int temp = 0;
                uint INTERNET_AUTO_DIAL_UNATTENDED = 2;
                int retVal = RAS.InternetDial(IntPtr.Zero, Connection, INTERNET_AUTO_DIAL_UNATTENDED, ref temp, 0);
                return retVal;
            }
            public void Disconnect()
            {
                RAS.RasHangUp(m_ConnectedRasHandle);
            }
        }

    }
}
