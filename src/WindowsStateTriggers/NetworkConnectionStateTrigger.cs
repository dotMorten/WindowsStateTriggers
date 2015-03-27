// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
    /// <summary>
    /// Trigger for switching when the screen orientation changes
    /// </summary>
	public class NetworkConnectionStateTrigger : StateTriggerBase
	{
		public NetworkConnectionStateTrigger()
		{
			//TODO: Make this a weak event reference!
			NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
			UpdateState();
		}

		private void NetworkInformation_NetworkStatusChanged(object sender)
		{
			var _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, UpdateState);
		}

		private void UpdateState()
		{
			bool isConnected = false;
			var profile = NetworkInformation.GetInternetConnectionProfile();
			if(profile != null)
				isConnected = profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
			SetTriggerValue(
				 isConnected && ConnectionState == ConnectionState.Connected ||
				!isConnected && ConnectionState == ConnectionState.Disconnected);
		}

        public ConnectionState ConnectionState
		{
			get { return (ConnectionState)GetValue(ConnectionStateProperty); }
			set { SetValue(ConnectionStateProperty, value); }
		}

		public static readonly DependencyProperty ConnectionStateProperty =
			DependencyProperty.Register("ConnectionState", typeof(ConnectionState), typeof(NetworkConnectionStateTrigger), 
			new PropertyMetadata(ConnectionState.Connected, OnConnectionStatePropertyChanged));

		private static void OnConnectionStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (NetworkConnectionStateTrigger)d;
            obj.UpdateState();
        }
	}

	public enum ConnectionState
	{
		Connected,
		Disconnected,
	}
}
