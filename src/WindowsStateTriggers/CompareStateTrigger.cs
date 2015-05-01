// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
	/// <summary>
	/// Enables a state if the value is equal to another value
	/// </summary>
	/// <remarks>
	/// <para>
	/// Example: Trigger if a value is greater than 0
	/// <code lang="xaml">
	///     &lt;triggers:CompareStateTrigger Value="{Binding MyValue}" CompareTo="0" Comparison="GreaterThan" />
	/// </code>
	/// </para>
	/// </remarks>
	public class CompareStateTrigger : StateTriggerBase
	{
		private void UpdateTrigger()
		{
			var result = CompareValues() == Comparison;
			SetActive(result);
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
			DependencyProperty.Register("Value", typeof(object), typeof(CompareStateTrigger),
			new PropertyMetadata(null, OnValuePropertyChanged));

		private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (CompareStateTrigger)d;
			obj.UpdateTrigger();
		}

		/// <summary>
		/// Gets or sets the value to compare to.
		/// </summary>
		public object CompareTo
		{
			get { return (object)GetValue(CompareToProperty); }
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
						v2 = System.Convert.ChangeType(v2, v1.GetType(), CultureInfo.InvariantCulture);
					}
					else if (v2 is IComparable)
					{
						v1 = System.Convert.ChangeType(v1, v2.GetType(), CultureInfo.InvariantCulture);
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