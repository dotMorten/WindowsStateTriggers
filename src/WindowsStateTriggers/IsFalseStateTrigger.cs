// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
    /// <summary>
    /// Enables a state if the value is false
    /// </summary>
    public class IsFalseStateTrigger : StateTriggerBase
	{
		/// <summary>
		/// Gets or sets the value used to check for <c>false</c>.
		/// </summary>
		public bool Value
		{
			get { return (bool)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="Value"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(bool), typeof(IsFalseStateTrigger), 
			new PropertyMetadata(true, OnValuePropertyChanged));

		private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (IsFalseStateTrigger)d;
			var val = (bool)e.NewValue;
			obj.SetActive(!val);
		}
	}
}
