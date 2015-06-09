// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
	/// <summary>
	/// Enables a state if the value is not equal to another value
	/// </summary>
	public class NotEqualStateTrigger : ConditionStateTriggerBase<object>
	{
		/// <summary>
		/// Predicate that causes the trigger to activate when satisfied.
		/// </summary>
		/// <param name="value">The value used as input to this trigger.</param>
		/// <returns>A <see cref="bool"/> indicating whether the trigger is active.</returns>
		protected override bool Condition(object value)
		{
			return !EqualsStateTrigger.AreValuesEqual(Value, NotEqualTo, true);
		}

		/// <summary>
		/// Gets or sets the value to compare inequality to.
		/// </summary>
		public object NotEqualTo
		{
			get { return GetValue(NotEqualToProperty); }
			set { SetValue(NotEqualToProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="NotEqualTo"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty NotEqualToProperty =
					DependencyProperty.Register("NotEqualTo", typeof(object), typeof(NotEqualStateTrigger), new PropertyMetadata(null, OnValuePropertyChanged));
	}
}
