// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
	/// <summary>
	/// Trigger for switching between Windows and Windows Phone
	/// </summary>
	public class DeviceTypeAdaptiveTrigger : StateTriggerBase
	{
		private static string deviceFamily;

		public DeviceTypeAdaptiveTrigger()
		{
			if (deviceFamily == null)
			{
				var qualifiers = Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().QualifierValues;
				if (qualifiers.ContainsKey("DeviceFamily"))
					deviceFamily = qualifiers["DeviceFamily"];
				else
					deviceFamily = "";
			}
		}

		public DeviceType DeviceType
		{
			get { return (DeviceType)GetValue(DeviceTypeProperty); }
			set { SetValue(DeviceTypeProperty, value); }
		}

		public static readonly DependencyProperty DeviceTypeProperty =
			DependencyProperty.Register("DeviceType", typeof(DeviceType), typeof(DeviceTypeAdaptiveTrigger),
			new PropertyMetadata(DeviceType.Unknown, OnDeviceTypePropertyChanged));

		private static void OnDeviceTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (DeviceTypeAdaptiveTrigger)d;
			var val = (DeviceType)e.NewValue;
			if (deviceFamily == "Mobile")
				obj.SetTriggerValue(val == DeviceType.Phone);
			else if (deviceFamily == "Desktop")
				obj.SetTriggerValue(val == DeviceType.Windows);
			else
				obj.SetTriggerValue(val == DeviceType.Unknown);
		}
	}

	public enum DeviceType
	{
		Unknown = 0, Windows = 1, Phone = 2,
	}
}
