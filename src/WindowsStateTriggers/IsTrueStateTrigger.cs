// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
    /// <summary>
    /// Enables a state if the value is true
    /// </summary>
    public class IsTrueStateTrigger : StateTriggerBase
	{
		public bool Value
		{
			get { return (bool)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(bool), typeof(IsTrueStateTrigger), 
			new PropertyMetadata(false, OnValuePropertyChanged));

		private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (IsTrueStateTrigger)d;
			var val = (bool)e.NewValue;
			obj.SetTriggerValue(val);
		}

		internal void UpdateTrigger(bool val)
		{
			throw new NotImplementedException();
		}
	}
}
