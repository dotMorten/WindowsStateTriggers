// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
    /// <summary>
    /// Enables a state if the value is equal to another value
    /// </summary>
    public class EqualsStateTrigger : StateTriggerBase
	{
		private void UpdateTrigger()
		{
			if (Value == EqualTo)
			{
				SetTriggerValue(true);
				return;
			}
			if(Value != null && EqualTo != null)
			{
				if(Value.GetType() != EqualTo.GetType()) //Let's see if we can convert
				{
					var t2 = System.Convert.ChangeType(Value, EqualTo.GetType(), CultureInfo.InvariantCulture);
					if (EqualTo.Equals(t2))
					{
						SetTriggerValue(true);
						return;
					}
					t2 = System.Convert.ChangeType(EqualTo, Value.GetType(), CultureInfo.InvariantCulture);
					if (Value.Equals(t2))
					{
						SetTriggerValue(true);
						return;
					}
				}
			}
			SetTriggerValue(false);
		}

		public object Value
		{
			get { return (object)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(object), typeof(EqualsStateTrigger), 
			new PropertyMetadata(null, OnValuePropertyChanged));

		private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (EqualsStateTrigger)d;
			obj.UpdateTrigger();
		}

		public object EqualTo
		{
			get { return (object)GetValue(EqualToProperty); }
			set { SetValue(EqualToProperty, value); }
		}

		public static readonly DependencyProperty EqualToProperty =
					DependencyProperty.Register("EqualTo", typeof(object), typeof(EqualsStateTrigger), new PropertyMetadata(null, OnValuePropertyChanged));

	}
}
