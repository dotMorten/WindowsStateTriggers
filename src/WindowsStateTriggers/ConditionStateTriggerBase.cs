// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
	/// <summary>
	/// Base class for a trigger that takes a value of type <c>T</c> and activates depending on a condition (<c>Predicate&lt;T&gt;</c>)
	/// </summary>
	public abstract class ConditionStateTriggerBase<T> : StateTriggerBase, ITriggerValue
	{
		/// <summary>
		/// Re-evaluates the active state of this trigger according to the current <see cref="Value"/>
		/// </summary>
		protected void UpdateTrigger()
		{
			IsActive = Condition(Value);
		}

		/// <summary>
		/// Predicate that causes the trigger to activate when satisfied.
		/// </summary>
		/// <param name="value">The value used as input to this trigger.</param>
		/// <returns>A <see cref="bool"/> indicating whether the trigger is active.</returns>
		protected abstract bool Condition(T value);

		/// <summary>
		/// Method called when the <see cref="Value"/> changes that can be overriden by classes that inherit from <see cref="ConditionStateTriggerBase{T}"/>.
		/// </summary>
		/// <remarks>
		/// The intended use of this callback is for inheritors to know when to attach event listeners as the <see cref="Value"/> changes.
		/// </remarks>
		protected virtual void ValueChanged() { }

		/// <summary>
		/// Constructor for <see cref="ConditionStateTriggerBase{T}"/> which initializes the activation state of the trigger.
		/// </summary>
		public ConditionStateTriggerBase()
		{
			UpdateTrigger();
		}

		/// <summary>
		/// Gets or sets the value for comparison.
		/// </summary>
		public T Value
		{
			get { return (T)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="Value"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(T), typeof(CompareStateTrigger),
			new PropertyMetadata(default(T), OnValuePropertyChanged));

		/// <summary>
		/// Called when the <see cref="Value"/> property changes or when another 
		/// <see cref="DependencyProperty"/> causes the Trigger to be re-evaluated.
		/// </summary>
		/// <param name="d">The <see cref="DependencyObject"/> that triggered the change.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> with the arguments relative to this change.</param>
		protected static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (ConditionStateTriggerBase<T>)d;
			obj.UpdateTrigger();
			obj.ValueChanged();
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
