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
    public class NotEqualStateTrigger : StateTriggerBase
	{
		private void UpdateTrigger()
		{
			SetTriggerValue(!EqualsStateTrigger.AreValuesEqual(Value, NotEqualTo, true));
		}

		public object Value
		{
			get { return (object)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(object), typeof(NotEqualStateTrigger), 
			new PropertyMetadata(null, OnValuePropertyChanged));

		private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (NotEqualStateTrigger)d;
			obj.UpdateTrigger();
		}

		public object NotEqualTo
		{
			get { return (object)GetValue(NotEqualToProperty); }
			set { SetValue(NotEqualToProperty, value); }
		}

		/// <summary>
		/// Identified the <see cref="NotEqualTo"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty NotEqualToProperty =
					DependencyProperty.Register("NotEqualTo", typeof(object), typeof(NotEqualStateTrigger), new PropertyMetadata(null, OnValuePropertyChanged));
	}
}
