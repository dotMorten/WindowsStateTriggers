using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
    /// <summary>
    /// Enables a state if a type is present on the device
    /// </summary>
    /// <remarks>
    ///     <triggers:IsTypePresentTrigger TypeName="Windows.Phone.UI.Input.HardwareButtons" />
    /// </remarks>
    public class IsTypePresentStateTrigger : StateTriggerBase
	{
		public string TypeName
		{
			get { return (string)GetValue(TypeNameProperty); }
			set { SetValue(TypeNameProperty, value); }
		}

		public static readonly DependencyProperty TypeNameProperty =
			DependencyProperty.Register("TypeName", typeof(string), typeof(IsTypePresentStateTrigger), 
			new PropertyMetadata("", OnTypeNamePropertyChanged));

		private static void OnTypeNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (IsTypePresentStateTrigger)d;
			var val = (string)e.NewValue;
			obj.SetTriggerValue(!string.IsNullOrWhiteSpace(val) && ApiInformation.IsTypePresent(val));
		}
	}
}
