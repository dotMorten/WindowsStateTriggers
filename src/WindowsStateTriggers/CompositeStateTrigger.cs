// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Windows.Devices.Input;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace WindowsStateTriggers
{
	/// <summary>
	/// Enables a state based on a collection of other triggers.
	/// </summary>
	/// <remarks>
	/// The CompositeTrigger ONLY supports <see cref="StateTrigger"/> and 
	/// custom triggers implementing <see cref="ITriggerValue"/>.
	/// </remarks>
	[ContentProperty(Name = "StateTriggers")]
	public class CompositeStateTrigger : StateTriggerBase, ITriggerValue
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CompositeStateTrigger"/> class.
		/// </summary>
		public CompositeStateTrigger()
		{

			StateTriggers = new StateTriggerCollection();
		}

		private void EvaluateTriggers()
		{
			if (!StateTriggers.Any())
			{
				IsActive = false;
			}
			else if (Operator == LogicalOperator.Or)
			{
				bool result = GetValues().Where(t => t).Any();
				IsActive = (result);
			}
			else if (Operator == LogicalOperator.And)
			{
				bool result = !GetValues().Where(t => !t).Any();
				IsActive = (result);
			}
			else if (Operator == LogicalOperator.Xor)
			{
				bool result = GetValues().Where(t => t).Count() == 1;
				IsActive = (result);
			}
		}

		private IEnumerable<bool> GetValues()
		{
			foreach (var trigger in StateTriggers)
			{
				if (trigger is ITriggerValue)
				{
					yield return ((ITriggerValue)trigger).IsActive;
				}
				else if (trigger is StateTrigger)
				{
					yield return ((StateTrigger)trigger).IsActive;
                    //bool? value = null;
					//try
					//{
					//	value = ((StateTrigger)trigger).IsActive;
					//}
					//catch { }
					//if (value.HasValue)
					//	yield return value.Value;
				}
			}
		}

		private void CompositeTrigger_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			OnTriggerCollectionChanged(e.OldItems == null ? null : e.OldItems.OfType<StateTriggerBase>(),
				e.OldItems == null ? null : e.NewItems.OfType<StateTriggerBase>());
			//TODO: handle reset
		}
		private void CompositeStateTrigger_VectorChanged(Windows.Foundation.Collections.IObservableVector<DependencyObject> sender, Windows.Foundation.Collections.IVectorChangedEventArgs e)
		{
			if (e.CollectionChange == Windows.Foundation.Collections.CollectionChange.ItemInserted)
			{
				var item = sender[(int)e.Index] as StateTriggerBase;
				if (item != null)
				{
					OnTriggerCollectionChanged(null, new StateTriggerBase[] { item });
				}
			}
			//else: Handle remove and reset
		}
		private void OnTriggerCollectionChanged(IEnumerable<StateTriggerBase> oldItems, IEnumerable<StateTriggerBase> newItems)
		{
			if (newItems != null)
			{
				foreach (var item in newItems)
				{
					if (item is StateTrigger)
					{
						long id = item.RegisterPropertyChangedCallback(
								StateTrigger.IsActiveProperty, TriggerIsActivePropertyChanged);
						item.SetValue(RegistrationTokenProperty, id);
					}
					else if (item is ITriggerValue)
					{
						((ITriggerValue)item).IsActiveChanged += CompositeTrigger_IsActiveChanged;
					}
					else
					{
						throw new NotSupportedException("Only StateTrigger or triggers implementing ITriggerValue are supported in a Composite trigger");
					}
				}
			}
			if (oldItems != null)
			{
				foreach (var item in oldItems)
				{
					if (item is StateTrigger)
					{
						var value = item.GetValue(RegistrationTokenProperty);
						if (value is long)
						{
							if (((long)value) > 0)
							{
								item.ClearValue(RegistrationTokenProperty);
								item.UnregisterPropertyChangedCallback(StateTrigger.IsActiveProperty, (long)value);
							}
						}
					}
					else if (item is ITriggerValue)
					{
						((ITriggerValue)item).IsActiveChanged -= CompositeTrigger_IsActiveChanged;
					}
				}
			}
			EvaluateTriggers();
		}

		private void CompositeTrigger_IsActiveChanged(object sender, EventArgs e)
		{
			EvaluateTriggers();
		}

		private void TriggerIsActivePropertyChanged(DependencyObject sender, DependencyProperty dp)
		{
			EvaluateTriggers();
		}

		/// <summary>
		/// Gets or sets the state trigger collection.
		/// </summary>
		public StateTriggerCollection StateTriggers
		{
			get { return (StateTriggerCollection)GetValue(StateTriggersProperty); }
			set { SetValue(StateTriggersProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="StateTriggers"/> dependency property
		/// </summary>
		private static readonly DependencyProperty StateTriggersProperty =
			DependencyProperty.Register("StateTriggers", typeof(StateTriggerCollection), typeof(CompositeStateTrigger), new PropertyMetadata(null, OnStateTriggersPropertyChanged));

		private static void OnStateTriggersPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CompositeStateTrigger trigger = (CompositeStateTrigger)d;
			if (e.OldValue is INotifyCollectionChanged)
			{
				(e.OldValue as INotifyCollectionChanged).CollectionChanged -= trigger.CompositeTrigger_CollectionChanged;
			}
			else if (e.OldValue is Windows.Foundation.Collections.IObservableVector<DependencyObject>)
			{
				(e.OldValue as Windows.Foundation.Collections.IObservableVector<DependencyObject>).VectorChanged += trigger.CompositeStateTrigger_VectorChanged;
				trigger.OnTriggerCollectionChanged((e.NewValue as Windows.Foundation.Collections.IObservableVector<DependencyObject>).OfType<StateTriggerBase>(),
					null);
			}

			if (e.NewValue is INotifyCollectionChanged)
			{
				//TODO: Should be weak reference just in case
				(e.NewValue as INotifyCollectionChanged).CollectionChanged += trigger.CompositeTrigger_CollectionChanged;
			}
			else if (e.NewValue is Windows.Foundation.Collections.IObservableVector<DependencyObject>)
			{
				(e.NewValue as Windows.Foundation.Collections.IObservableVector<DependencyObject>).VectorChanged += trigger.CompositeStateTrigger_VectorChanged;
				trigger.OnTriggerCollectionChanged(null,
					(e.NewValue as Windows.Foundation.Collections.IObservableVector<DependencyObject>).OfType<StateTriggerBase>());
			}
			if (e.NewValue is IEnumerable<StateTriggerBase>)
			{
				foreach (var item in e.NewValue as IEnumerable<StateTriggerBase>)
				{
					if (!(item is StateTrigger || !(item is ITriggerValue)))
					{
						try
						{
							throw new NotSupportedException("Only StateTrigger or triggers implementing ITriggerValue are supported in a Composite trigger");
						}
						finally
						{
							trigger.SetValue(StateTriggersProperty, e.OldValue); //Undo change
						}
					}
				}
				trigger.CompositeTrigger_CollectionChanged(e.NewValue,
					new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (e.NewValue as IEnumerable<StateTriggerBase>).ToList()));
			}
			trigger.EvaluateTriggers();
		}

		/// <summary>
		/// Used for remembering what token was used for event listening
		/// </summary>
		private static readonly DependencyProperty RegistrationTokenProperty =
			DependencyProperty.RegisterAttached("RegistrationToken", typeof(long), typeof(CompositeStateTrigger), new PropertyMetadata(0));

		/// <summary>
		/// Gets or sets the logical operation to apply to the triggers.
		/// </summary>
		/// <value>The evaluation.</value>
		public LogicalOperator Operator
		{
			get { return (LogicalOperator)GetValue(OperatorProperty); }
			set { SetValue(OperatorProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="Operator"/> dependency property
		/// </summary>
		public static readonly DependencyProperty OperatorProperty =
			DependencyProperty.Register("Operator", typeof(LogicalOperator), typeof(CompositeStateTrigger), new PropertyMetadata(LogicalOperator.And, OnEvaluatePropertyChanged));

		private static void OnEvaluatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CompositeStateTrigger trigger = (CompositeStateTrigger)d;
			trigger.EvaluateTriggers();
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

	/// <summary>
	/// Logical operations
	/// </summary>
	public enum LogicalOperator
	{
		/// <summary>
		/// And (All must be active)
		/// </summary>
		And,
		/// <summary>
		/// Or (Any can be active)
		/// </summary>
		Or,
		/// <summary>
		/// Exclusive-Or (only one can be active)
		/// </summary>
		Xor
	}
	/// <summary>
	/// Collection for the <see cref="CompositeStateTrigger"/>.
	/// </summary>
	public sealed class StateTriggerCollection : DependencyObjectCollection { }
}