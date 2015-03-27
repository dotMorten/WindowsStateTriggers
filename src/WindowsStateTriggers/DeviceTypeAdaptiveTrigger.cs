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
		public DeviceType DeviceType
		{
			get { return (DeviceType)GetValue(DeviceTypeProperty); }
			set { SetValue(DeviceTypeProperty, value); }
		}

		public static readonly DependencyProperty DeviceTypeProperty =
			DependencyProperty.Register("DeviceType", typeof(DeviceType), typeof(DeviceTypeAdaptiveTrigger), 
			new PropertyMetadata(DeviceType.None, OnDeviceTypePropertyChanged));

		private static void OnDeviceTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (DeviceTypeAdaptiveTrigger)d;
			var val = (DeviceType)e.NewValue;
			if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons")) //There's probably a better way!
				obj.SetTriggerValue(val == DeviceType.Phone);
			else
				obj.SetTriggerValue(val == DeviceType.Windows);
		}
	}

    public enum DeviceType
    {
        None = 0, Windows = 1, Phone = 2,
    }
}
