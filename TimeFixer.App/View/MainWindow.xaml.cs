﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;
using TimeFixer.App.Model;
using TimeFixer.App.ViewModel.Converters;
using TimeFixer.App.ViewModel.Helpers;

namespace TimeFixer.App.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Minimize to system tray when application is minimized.
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized) this.Hide();

            base.OnStateChanged(e);
        }

        // Minimize to system tray when application is closed.
        protected override void OnClosing(CancelEventArgs e)
        {
            // setting cancel to true will cancel the close request
            // so the application is not closed
            e.Cancel = true;

            this.Hide();

            base.OnClosing(e);
        }

        private DispatcherTimer _timer;

        [Obsolete("Obsolete")]
        public MainWindow()
        {
            InitializeComponent();
            //Timer_Tick(null, null);

            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 30)
            };

            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        [Obsolete("Obsolete")]
        private async void Timer_Tick(object? sender, EventArgs? e)
        {
            var onlineTime = await NistTimeGetter.GetNistTime();
            GotTimeDisplay.Text = onlineTime.ToString(CultureInfo.InvariantCulture);

            int timeModifier = 0;
            
            try
            {
                timeModifier = TimeModifier.Text != "" ? Convert.ToInt32(TimeModifier.Text) : 0;
            }
            catch
            {
                timeModifier = 0;
            }

            var localTime = onlineTime.AddHours(timeModifier);

            var systemTimeToSet = DateTimeToMySystemTimeConverter.ConvertToMySystemTime(localTime);

            SystemTimeSetter.SetSystemTime(ref systemTimeToSet);

            ResultTimeDisplay.Text = localTime.ToString("dd/MMM/yyyy HH:mm:ss");
        }
    }
}
