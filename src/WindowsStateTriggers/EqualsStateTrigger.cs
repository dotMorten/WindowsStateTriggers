// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
	/// <summary>
	/// Enables a state if the value is equal to another value
	/// </summary>
	/// <remarks>
	/// <para>
	/// Example: Trigger if a value is null
	/// <code lang="xaml">
	///     &lt;triggers:EqualsStateTrigger Value="{Binding MyObject}" EqualTo="{x:Null}" />
	/// </code>
	/// </para>
	/// </remarks>
	public class EqualsStateTrigger : ConditionStateTriggerBase<object>
	{
		/// <summary>
		/// Predicate that causes the trigger to activate when satisfied.
		/// </summary>
		/// <param name="value">The value used as input to this trigger.</param>
		/// <returns>A <see cref="bool"/> indicating whether the trigger is active.</returns>
		protected override bool Condition(object value)
		{
			return AreValuesEqual(value, EqualTo, true);
		}

		/// <summary>
		/// Gets or sets the value to compare equality to.
		/// </summary>
		public object EqualTo
		{
			get { return GetValue(EqualToProperty); }
			set { SetValue(EqualToProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="EqualTo"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty EqualToProperty =
					DependencyProperty.Register("EqualTo", typeof(object), typeof(EqualsStateTrigger), new PropertyMetadata(null, OnValuePropertyChanged));


		internal static bool AreValuesEqual(object value1, object value2, bool convertType)
		{
			Func<object, object, bool> areEqualAfterConversion = (x, y) => y.Equals(Convert.ChangeType(x, y.GetType(), CultureInfo.InvariantCulture));

			return(value1 == value2) ||
				(convertType && value1 != null && value2 != null && value1.GetType() != value2.GetType() &&
					(areEqualAfterConversion(value1, value2) || areEqualAfterConversion(value2, value1))
				);
		}
	}
}
