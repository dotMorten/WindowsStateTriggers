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

		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceFamilyAdaptiveTrigger"/> class.
		/// </summary>
		public DeviceFamilyAdaptiveTrigger()
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

		/// <summary>
		/// Gets or sets the device family to trigger on.
		/// </summary>
		/// <value>The device family.</value>
		public DeviceFamily DeviceFamily
		{
			get { return (DeviceFamily)GetValue(DeviceFamilyProperty); }
			set { SetValue(DeviceFamilyProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="DeviceFamily"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty DeviceFamilyProperty =
			DependencyProperty.Register("DeviceFamily", typeof(DeviceFamily), typeof(DeviceFamilyAdaptiveTrigger),
			new PropertyMetadata(DeviceFamily.Unknown, OnDeviceTypePropertyChanged));

		private static void OnDeviceTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (DeviceFamilyAdaptiveTrigger)d;
			var val = (DeviceFamily)e.NewValue;
			if (deviceFamily == "Mobile")
				obj.SetActive(val == DeviceFamily.Mobile);
			else if (deviceFamily == "Desktop")
				obj.SetActive(val == DeviceFamily.Desktop);
			else
				obj.SetActive(val == DeviceFamily.Unknown);
		}
	}

	/// <summary>
	/// Device Families
	/// </summary>
	public enum DeviceFamily
	{
		/// <summary>
		/// Unknown
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// Desktop
		/// </summary>
		Desktop = 1,
		/// <summary>
		/// Mobile
		/// </summary>
		Mobile = 2,
	}
}
