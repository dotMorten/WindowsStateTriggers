// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
    /// <summary>
    /// Trigger for switching between Windows and Windows Phone
    /// </summary>
    public class PlatformAdaptiveTrigger : StateTriggerBase
	{
		public Platform PlatformType
		{
			get { return (PlatformAdaptiveTrigger.Platform)GetValue(DeviceTypeProperty); }
			set { SetValue(DeviceTypeProperty, value); }
		}

		public static readonly DependencyProperty DeviceTypeProperty =
			DependencyProperty.Register("PlatformType", typeof(Platform), typeof(PlatformAdaptiveTrigger), 
			new PropertyMetadata(Platform.None, OnPlatformTypePropertyChanged));

		private static void OnPlatformTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (PlatformAdaptiveTrigger)d;
			var val = (Platform)e.NewValue;

            var qualifiers = Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().QualifierValues;
            if (qualifiers.ContainsKey("DeviceFamily") && qualifiers["DeviceFamily"] == "Mobile")
                obj.SetTriggerValue(val == Platform.Phone);
            else
                obj.SetTriggerValue(val == Platform.Windows);
		}

		public enum Platform
		{
			None = 0, Windows = 1, Phone = 2,
		}
	}
}
