﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestApp.Samples
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class FullScreenSample : UserControl
	{
		public FullScreenSample()
		{
			this.InitializeComponent();
		}

		private void fullScreenMode_Click(object sender, RoutedEventArgs e)
		{
			var view = ApplicationView.GetForCurrentView();
			var isFullScreenMode = view.IsFullScreenMode;

			if (isFullScreenMode)
				view.ExitFullScreenMode();
			else
				view.TryEnterFullScreenMode();
		}
	}
}
