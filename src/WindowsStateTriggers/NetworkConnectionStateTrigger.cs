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
    /// Trigger for switching when the network availability changes
    /// </summary>
	public class NetworkConnectionStateTrigger : StateTriggerBase, ITriggerValue
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NetworkConnectionStateTrigger"/> class.
		/// </summary>
		public NetworkConnectionStateTrigger()
		{
			var weakEvent =
				new WeakEventListener<NetworkConnectionStateTrigger, object>(this)
				{
					OnEventAction = (instance, source) => NetworkInformation_NetworkStatusChanged(source),
					OnDetachAction = (instance, weakEventListener) => NetworkInformation.NetworkStatusChanged -= weakEventListener.OnEvent
				};
			NetworkInformation.NetworkStatusChanged += weakEvent.OnEvent;
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
			IsActive = (
				 isConnected && ConnectionState == ConnectionState.Connected ||
				!isConnected && ConnectionState == ConnectionState.Disconnected);
		}

		/// <summary>
		/// Gets or sets the state of the connection to trigger on.
		/// </summary>
		public ConnectionState ConnectionState
		{
			get { return (ConnectionState)GetValue(ConnectionStateProperty); }
			set { SetValue(ConnectionStateProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="ConnectionState"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty ConnectionStateProperty =
			DependencyProperty.Register("ConnectionState", typeof(ConnectionState), typeof(NetworkConnectionStateTrigger), 
			new PropertyMetadata(ConnectionState.Connected, OnConnectionStatePropertyChanged));

		private static void OnConnectionStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (NetworkConnectionStateTrigger)d;
            obj.UpdateState();
		}

		#region ITriggerValue

		private bool m_IsActive;

		/// <summary>
		/// Gets a value indicating whether this trigger is active.
		/// </summary>
		/// <value><c>true</c> if this trigger is active; otherwise, <c>false</c>.</value>
		public bool IsActive
		{
			get { return m_IsActive; }
			private set
			{
				if (m_IsActive != value)
				{
					m_IsActive = value;
					base.SetActive(value);
					if (IsActiveChanged != null)
						IsActiveChanged(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Occurs when the <see cref="IsActive" /> property has changed.
		/// </summary>
		public event EventHandler IsActiveChanged;

		#endregion ITriggerValue
	}

	/// <summary>
	/// ConnectionStates
	/// </summary>
	public enum ConnectionState
	{
		/// <summary>
		/// Connected
		/// </summary>
		Connected,
		/// <summary>
		/// Disconnected
		/// </summary>
		Disconnected,
	}
}
