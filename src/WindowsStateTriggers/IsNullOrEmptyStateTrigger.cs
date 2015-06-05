// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Linq;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
    /// <summary>
    /// Enables a state if an Object is <c>null</c> or a String/IEnumerable is empty
    /// </summary>
    public class IsNullOrEmptyStateTrigger : StateTriggerBase, ITriggerValue
	{
		/// <summary>
		/// Gets or sets the value used to check for <c>null</c> or empty.
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
			DependencyProperty.Register("Value", typeof(object), typeof(IsNullOrEmptyStateTrigger), 
			new PropertyMetadata(true, OnValuePropertyChanged));

		private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (IsNullOrEmptyStateTrigger)d;
			var val = e.NewValue;

			var result = (val == null);

			if (!result)
			{
				// Object is not null, check for an empty string
				var valString = val as string;
				if (valString != null)
				{
					result = (valString.Length == 0);
				}
				else
				{
					// Object is not a string, check for an empty IEnumerable
					var valEnumerable = val as IEnumerable;
					result = ((valEnumerable != null) && !(valEnumerable.GetEnumerator().MoveNext()));
				}
			}

			obj.IsActive = result;
		}

		#region ITriggerValue

		private bool m_IsActive;

		/// <summary>
		/// Gets a value indicating whether this trigger is active.
		/// </summary>
		/// <value><c>true</c> if this trigger is active; otherwise, <c>false</c>.</value>
		public bool IsActive
		{
			get { return m_IsActive; }
			private set
			{
				if (m_IsActive != value)
				{
					m_IsActive = value;
					base.SetActive(value);
					if (IsActiveChanged != null)
						IsActiveChanged(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Occurs when the <see cref="IsActive" /> property has changed.
		/// </summary>
		public event EventHandler IsActiveChanged;

		#endregion ITriggerValue
	}
}
