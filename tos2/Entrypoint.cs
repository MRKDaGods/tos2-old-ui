using Services;
using System;
using System.Runtime.InteropServices;

namespace MRK
{
	public class Entrypoint
	{
        private const uint MB_OK = 0x00000000U;

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
		private static extern int MessageBoxA(IntPtr hWnd, string text, string caption, uint type);

		public static bool Main()
		{
			MessageBoxA(IntPtr.Zero, $"Hello {Service.Home.UserService.UserInfo.AccountName}", "Test", MB_OK);
			return true;
		}
	}
}
