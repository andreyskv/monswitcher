using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

namespace MonSwitcher
{
    class Program
    {
        public static WinApi.DEVMODE NewDevMode()
        {
            WinApi.DEVMODE dm = new WinApi.DEVMODE();
            dm.dmDeviceName = new String(new char[31]);
            dm.dmFormName = new String(new char[31]);
            dm.dmSize = (ushort)Marshal.SizeOf(dm);
            return dm;
        }

        public static string GetScreenName(int id)
        {
            WinApi.DISPLAY_DEVICE ddOne = new WinApi.DISPLAY_DEVICE();
            ddOne.cb = Marshal.SizeOf(ddOne);
            WinApi.User_32.EnumDisplayDevices(null, id, ref ddOne, 0);
            return ddOne.DeviceName;

        }

        public static WinApi.DisplaySetting_Results SetScreenPosition(string screen, int x, int y, bool isMain)
        {
             
            WinApi.DEVMODE ndm= NewDevMode();
            ndm.dmFields = WinApi.DEVMODE_Flags.DM_POSITION;
            ndm.dmPosition.x = x;
            ndm.dmPosition.y = y;

            int flags = (int)WinApi.DeviceFlags.CDS_UPDATEREGISTRY;// | (int)WinApi.DeviceFlags.CDS_NORESET;
            if (isMain)
                 flags |= (int)WinApi.DeviceFlags.CDS_SET_PRIMARY; 

             return (WinApi.DisplaySetting_Results)WinApi.User_32.ChangeDisplaySettingsEx( screen, ref ndm, (IntPtr)null, flags, IntPtr.Zero);
         
        }

        public static WinApi.DisplaySetting_Results ApplySettings()
        {
            WinApi.DEVMODE ndm = NewDevMode();
            return (WinApi.DisplaySetting_Results)WinApi.User_32.ChangeDisplaySettingsEx(null, ref ndm, (IntPtr)null, 0, (IntPtr)null);
        }

        public static void SetPrimaryMon()
        {

            WinApi.DisplaySetting_Results result = 0;

            string tv = GetScreenName(3);
            string monBottom = GetScreenName(2);
            string monTop = GetScreenName(0);

            //-------------------------------------------------------------------------------------------------

          

            result = SetScreenPosition(tv, -1920, 0, false);
            Console.WriteLine(String.Format("Action for {0} result: {1}", tv, result.ToString()));

            result = SetScreenPosition(monTop, 0, -1152, false);
            Console.WriteLine(String.Format("Action for {0} result: {1}", monTop, result.ToString()));

            result = SetScreenPosition(monBottom, 0, 0, true);
            Console.WriteLine(String.Format("Action for {0} result: {1}", monBottom, result.ToString()));
        }

        public static void SetPrimaryTV()
        {
          
            WinApi.DisplaySetting_Results result = 0;
          
            string tv = GetScreenName(3);
            string monBottom = GetScreenName(2);
            string monTop = GetScreenName(0);

            //-------------------------------------------------------------------------------------------------

        

            result = SetScreenPosition(monBottom, 1920, 0, false);
            Console.WriteLine(String.Format( "Action for {0} result: {1}",monBottom, result.ToString()));

            result = SetScreenPosition(monTop, 1920, -1152, false);
            Console.WriteLine(String.Format( "Action for {0} result: {1}",monTop, result.ToString()));

            result = SetScreenPosition(tv, 0, 0, true);
            Console.WriteLine(String.Format("Action for {0} result: {1}", tv, result.ToString()));

        }

        static void Main(string[] args)
        {

            try 
            { 
                if (args[0].ToString() == "tv")
                {
                    SetPrimaryTV();
                }
                else if (args[0].ToString() == "mon")
                {
                    SetPrimaryMon(); 
                }
              //  ApplySettings();
            }
            catch
            {
                Console.WriteLine("Usage MonSwitcher {tv|mon}  ");
            }

            //Console.ReadLine();
        }
    }
}
