using System;
using System.Windows.Forms;

namespace Focus
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FocusApplicationContext());
        }
    }

    internal class FocusApplicationContext : ApplicationContext
    {
        private readonly NotifyIcon _notifyIcon;

        private const int Intervalsec = 60;

        private static int _countdown;

        private readonly Timer _timer1 = new Timer
        {
            Enabled = true,
            Interval = 1000
        };

        public FocusApplicationContext()
        {
            _timer1.Tick += timer1_Tick;

            var exitMenuItem = new MenuItem("Exit", Exit);

            _notifyIcon = new NotifyIcon
            {
                Icon = Properties.Resources.AppIconwht,
                ContextMenu = new ContextMenu(new[]
                    {exitMenuItem}),
                Visible = true,
                Text = Properties.Resources.AppName
            };
        }

        private void Exit(object sender, EventArgs e)
        {
            // We must manually tidy up and remove the icon before we exit.
            // Otherwise it will be left behind until the user mouses over.
            _notifyIcon.Visible = false;
            Application.Exit();
        }

        private static void timer1_Tick(object sender, EventArgs e)
        {
            _countdown--;
            if (_countdown > 0) return;

            switch ((KeepawakeMethod)Properties.Settings.Default["KeepawakeMethod"])
            {
                case KeepawakeMethod.Keyboard:
                    SendKeys.Send("{NUMLOCK}{NUMLOCK}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _countdown = Intervalsec;
        }

    }
}
