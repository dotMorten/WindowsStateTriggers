// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
	/// <summary>
	/// Enables a state if the value is equal to, greater than, or less than another value
	/// </summary>
	/// <remarks>
	/// <para>
	/// Example: Trigger if a value is greater than 0
	/// <code lang="xaml">
	///     &lt;triggers:CompareStateTrigger Value="{Binding MyValue}" CompareTo="0" Comparison="GreaterThan" />
	/// </code>
	/// </para>
	/// </remarks>
	public class CompareStateTrigger : ConditionStateTriggerBase<object>
	{
		/// <summary>
		/// Predicate that causes the trigger to activate when satisfied.
		/// </summary>
		/// <param name="value">The value used as input to this trigger.</param>
		/// <returns>A <see cref="bool"/> indicating whether the trigger is active.</returns>
		protected override bool Condition(object value)
		{
			return CompareValues() == Comparison;
		}

		/// <summary>
		/// Gets or sets the value to compare to.
		/// </summary>
		public object CompareTo
		{
			get { return GetValue(CompareToProperty); }
			set { SetValue(CompareToProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="CompareTo"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty CompareToProperty =
					DependencyProperty.Register("CompareTo", typeof(object), typeof(CompareStateTrigger), new PropertyMetadata(null, OnValuePropertyChanged));

		/// <summary>
		/// Gets or sets the comparison type
		/// </summary>
		public Comparison Comparison
		{
			get { return (Comparison)GetValue(ComparisonProperty); }
			set { SetValue(ComparisonProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="Comparison"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty ComparisonProperty =
			DependencyProperty.Register("Comparison", typeof(Comparison), typeof(CompareStateTrigger), new PropertyMetadata(Comparison.Equal, OnValuePropertyChanged));

		internal Comparison CompareValues()
		{
			var v1 = Value;
			var v2 = CompareTo;
			if (v1 == v2)
			{
				if (Comparison == Comparison.Equal)
					return Comparison.Equal;
			}
			if (v1 != null && v2 != null)
			{
				//Let's see if we can convert - for perf reasons though, try and use the right type in and out
				if (v1.GetType() != v2.GetType())
				{
					if (v1 is IComparable)
					{
						v2 = Convert.ChangeType(v2, v1.GetType(), CultureInfo.InvariantCulture);
					}
					else if (v2 is IComparable)
					{
						v1 = Convert.ChangeType(v1, v2.GetType(), CultureInfo.InvariantCulture);
					}
				}

				if (v1.GetType() == v2.GetType())
				{
					if (v1 is IComparable)
					{
						var result = ((IComparable)v1).CompareTo(v2);
						if (result < 0) return Comparison.LessThan;
						else if (result == 0) return Comparison.Equal;
						else return Comparison.GreaterThan;
					}
				}
			}
			return Comparison.NotComparable;
		}
	}
	/// <summary>
	/// Comparison types
	/// </summary>
	public enum Comparison
	{
		/// <summary>
		/// Not comparable
		/// </summary>
		NotComparable,
		/// <summary>
		/// Equals
		/// </summary>
		Equal,
		/// <summary>
		/// Less than
		/// </summary>
		LessThan,
		/// <summary>
		/// Greater than
		/// </summary>
		GreaterThan
	}
}
