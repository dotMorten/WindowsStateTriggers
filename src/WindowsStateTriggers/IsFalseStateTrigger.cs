using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
    /// <summary>
    /// Enables a state if the value is false
    /// </summary>
    public class IsFalseStateTrigger : StateTriggerBase
	{
		public bool Value
		{
			get { return (bool)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(bool), typeof(IsFalseStateTrigger), 
			new PropertyMetadata(true, OnValuePropertyChanged));

		private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (IsFalseStateTrigger)d;
			var val = (bool)e.NewValue;
			obj.SetTriggerValue(!val);
		}
	}
}
