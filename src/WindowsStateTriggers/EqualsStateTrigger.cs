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
			SetTriggerValue(EqualsStateTrigger.AreValuesEqual(Value, EqualTo, true));
		}

		/// <summary>
		/// Gets or sets the value for comparison.
		/// </summary>
		public object Value
		{
			get { return (object)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="Value"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(object), typeof(EqualsStateTrigger), 
			new PropertyMetadata(null, OnValuePropertyChanged));

		private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (EqualsStateTrigger)d;
			obj.UpdateTrigger();
		}

		/// <summary>
		/// Gets or sets the value to compare equality to.
		/// </summary>
		public object EqualTo
		{
			get { return (object)GetValue(EqualToProperty); }
			set { SetValue(EqualToProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="EqualTo"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty EqualToProperty =
					DependencyProperty.Register("EqualTo", typeof(object), typeof(EqualsStateTrigger), new PropertyMetadata(null, OnValuePropertyChanged));


		internal static bool AreValuesEqual(object value1, object value2, bool convertType)
		{
			if (value1 == value2)
			{
				return true;
			}
			if (value1 != null && value2 != null)
			{
				//Let's see if we can convert - for perf reasons though, try and use the right type in and out
				if (value1.GetType() != value2.GetType() && convertType)
				{
					var t2 = System.Convert.ChangeType(value1, value2.GetType(), CultureInfo.InvariantCulture);
					if (value2.Equals(t2))
					{
						return true;
					}
					//try the other way around
					t2 = System.Convert.ChangeType(value2, value1.GetType(), CultureInfo.InvariantCulture);
					if (value1.Equals(t2))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
