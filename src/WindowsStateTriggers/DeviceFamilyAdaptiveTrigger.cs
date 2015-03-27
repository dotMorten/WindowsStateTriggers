// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
	/// <summary>
	/// Trigger for switching between Windows and Windows Phone
	/// </summary>
	public class DeviceFamilyAdaptiveTrigger : StateTriggerBase
	{
		private static string deviceFamily;

		public DeviceFamilyAdaptiveTrigger()
		{
			if (deviceFamily == null)
			{
				var qualifiers = Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().QualifierValues;
				if (qualifiers.ContainsKey("DeviceFamily"))
					deviceFamily = qualifiers["DeviceFamily"];
			}
		}

		public DeviceFamily DeviceFamily
		{
			get { return (DeviceFamily)GetValue(DeviceFamilyProperty); }
			set { SetValue(DeviceFamilyProperty, value); }
		}

		public static readonly DependencyProperty DeviceFamilyProperty =
			DependencyProperty.Register("DeviceFamily", typeof(DeviceFamily), typeof(DeviceFamilyAdaptiveTrigger),
			new PropertyMetadata(DeviceFamily.None, OnDeviceFamilyPropertyChanged));

		private static void OnDeviceFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (DeviceFamilyAdaptiveTrigger)d;
			var val = (DeviceFamily)e.NewValue;
			if (deviceFamily == "Mobile")
				obj.SetTriggerValue(val == DeviceFamily.Mobile);
			else
				obj.SetTriggerValue(val == DeviceFamily.Desktop);
		}
	}

	public enum DeviceFamily
	{
		None = 0, Desktop = 1, Mobile = 2,
	}
}
