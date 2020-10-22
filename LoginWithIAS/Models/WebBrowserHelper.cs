using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
/*
 * Credit to @santosh  https://stackoverflow.com/users/1206824/santosh
 * https://stackoverflow.com/questions/45778466/how-to-clear-ie-cache-for-specific-site-using-c-sharp-not-using-js-or-jquery?answertab=votes#tab-top
 * 
 */
namespace LoginWithIAS.Models
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebBrowserHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpszUrlName"></param>
        /// <returns></returns>
        #region WINAPI        
        [DllImport("wininet", EntryPoint = "DeleteUrlCacheEntryA", SetLastError = true)]
        public static extern bool DeleteUrlCacheEntry(IntPtr lpszUrlName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="dwFlags"></param>
        /// <param name="lpReserved"></param>
        /// <returns></returns>
        [DllImport("wininet", SetLastError = true)]
        public static extern bool DeleteUrlCacheGroup(long GroupId, int dwFlags, IntPtr lpReserved);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpszUrlSearchPattern"></param>
        /// <param name="lpFirstCacheEntryInfo"></param>
        /// <param name="lpdwFirstCacheEntryInfoBufferSize"></param>
        /// <returns></returns>
        [DllImport("wininet", EntryPoint = "FindFirstUrlCacheEntryA", SetLastError = true)]
        public static extern IntPtr FindFirstUrlCacheEntry(string lpszUrlSearchPattern, IntPtr lpFirstCacheEntryInfo, ref int lpdwFirstCacheEntryInfoBufferSize);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwFlags"></param>
        /// <param name="dwFilter"></param>
        /// <param name="lpSearchCondition"></param>
        /// <param name="dwSearchCondition"></param>
        /// <param name="lpGroupId"></param>
        /// <param name="lpReserved"></param>
        /// <returns></returns>
        [DllImport("wininet", SetLastError = true)]
        public static extern IntPtr FindFirstUrlCacheGroup(int dwFlags, int dwFilter, IntPtr lpSearchCondition, int dwSearchCondition, ref long lpGroupId, IntPtr lpReserved);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hFind"></param>
        /// <param name="lpNextCacheEntryInfo"></param>
        /// <param name="lpdwNextCacheEntryInfoBufferSize"></param>
        /// <returns></returns>
        [DllImport("wininet", EntryPoint = "FindNextUrlCacheEntryA", SetLastError = true)]
        public static extern bool FindNextUrlCacheEntry(IntPtr hFind, IntPtr lpNextCacheEntryInfo, ref int lpdwNextCacheEntryInfoBufferSize);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hFind"></param>
        /// <param name="lpGroupId"></param>
        /// <param name="lpReserved"></param>
        /// <returns></returns>
        [DllImport("wininet", SetLastError = true)]
        public static extern bool FindNextUrlCacheGroup(IntPtr hFind, ref long lpGroupId, IntPtr lpReserved);
        #endregion

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        
        public struct INTERNET_CACHE_ENTRY_INFOA
        {
            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(0)]
            public uint dwStructSize;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(4)]
            public IntPtr lpszSourceUrlName;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(8)]
            public IntPtr lpszLocalFileName;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(12)]
            public uint CacheEntryType;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(16)]
            public uint dwUseCount;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(20)]
            public uint dwHitRate;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(24)]
            public uint dwSizeLow;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(28)]
            public uint dwSizeHigh;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(32)]
            public System.Runtime.InteropServices.ComTypes.FILETIME LastModifiedTime;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(40)]
            public System.Runtime.InteropServices.ComTypes.FILETIME ExpireTime;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(48)]
            public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(56)]
            public System.Runtime.InteropServices.ComTypes.FILETIME LastSyncTime;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(64)]
            public IntPtr lpHeaderInfo;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(68)]
            public uint dwHeaderInfoSize;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(72)]
            public IntPtr lpszFileExtension;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(76)]
            public uint dwReserved;

            /// <summary>
            /// 
            /// </summary>
            [FieldOffset(76)]
            public uint dwExemptDelta;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ClearCache()
        {
            bool flag;
            bool flag1;
            long num = (long)0;
            int num1 = 0;
            int num2 = 0;
            IntPtr zero = IntPtr.Zero;
            IntPtr intPtr = IntPtr.Zero;
            bool flag2 = false;
            intPtr = FindFirstUrlCacheGroup(0, 0, IntPtr.Zero, 0, ref num, IntPtr.Zero);
            if ((intPtr == IntPtr.Zero ? true : 259 != Marshal.GetLastWin32Error()))
            {
                while (true)
                {
                    flag = true;
                    if ((259 == Marshal.GetLastWin32Error() ? false : 2 != Marshal.GetLastWin32Error()))
                    {
                        flag2 = DeleteUrlCacheGroup(num, 2, IntPtr.Zero);
                        if ((flag2 ? false : 2 == Marshal.GetLastWin32Error()))
                        {
                            flag2 = FindNextUrlCacheGroup(intPtr, ref num, IntPtr.Zero);
                        }
                        if (flag2)
                        {
                            flag1 = true;
                        }
                        else
                        {
                            flag1 = (259 == Marshal.GetLastWin32Error() ? false : 2 != Marshal.GetLastWin32Error());
                        }
                        if (!flag1)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                intPtr = FindFirstUrlCacheEntry(null, IntPtr.Zero, ref num1);
                if ((intPtr == IntPtr.Zero ? true : 259 != Marshal.GetLastWin32Error()))
                {
                    num2 = num1;
                    zero = Marshal.AllocHGlobal(num2);
                    intPtr = FindFirstUrlCacheEntry(null, zero, ref num1);
                    while (true)
                    {
                        flag = true;
                        INTERNET_CACHE_ENTRY_INFOA structure = (INTERNET_CACHE_ENTRY_INFOA)Marshal.PtrToStructure(zero, typeof(INTERNET_CACHE_ENTRY_INFOA));
                        if (259 != Marshal.GetLastWin32Error())
                        {
                            num1 = num2;
                            flag2 = DeleteUrlCacheEntry(structure.lpszSourceUrlName);
                            if (!flag2)
                            {
                                flag2 = FindNextUrlCacheEntry(intPtr, zero, ref num1);
                            }
                            if (!(flag2 ? true : 259 != Marshal.GetLastWin32Error()))
                            {
                                break;
                            }
                            else if ((flag2 ? false : num1 > num2))
                            {
                                num2 = num1;
                                zero = Marshal.ReAllocHGlobal(zero, (IntPtr)num2);
                                flag2 = FindNextUrlCacheEntry(intPtr, zero, ref num1);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    Marshal.FreeHGlobal(zero);
                }
            }
        }
        /// <summary>
        /// For specific url
        /// </summary>
        /// <param name="url"></param>
        public static void ClearForSpecificUrl(string url)
        {
            try
            {
                int num = 0;
                var intPtr = FindFirstUrlCacheEntry(url, IntPtr.Zero, ref num);
                DeleteUrlCacheEntry(intPtr);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
