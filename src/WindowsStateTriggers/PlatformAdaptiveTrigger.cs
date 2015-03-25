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
			if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons")) //There's probably a better way!
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
