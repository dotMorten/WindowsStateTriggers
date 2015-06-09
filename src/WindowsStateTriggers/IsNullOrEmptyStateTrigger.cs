// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Specialized;
using Windows.Foundation.Collections;

namespace WindowsStateTriggers
{
	/// <summary>
	/// Enables a state if an Object is <c>null</c> or a String/IEnumerable is empty
	/// </summary>
	public class IsNullOrEmptyStateTrigger : ConditionStateTriggerBase<object>
	{
		/// <summary>
		/// Predicate that causes the trigger to activate when satisfied.
		/// </summary>
		/// <param name="value">The value used as input to this trigger.</param>
		/// <returns>A <see cref="bool"/> indicating whether the trigger is active.</returns>
		protected override bool Condition(object value)
		{
			return IsNullOrEmpty(value);
		}

		/// <summary>
		/// Method called when the <see cref="ConditionStateTriggerBase{T}.Value"/> changes that can be overriden by classes that inherit from <see cref="ConditionStateTriggerBase{T}"/>.
		/// </summary>
		protected override void ValueChanged()
		{
			if (Value == null)
				return;

			// Try to listen for various notification events
			// Starting with INorifyCollectionChanged
			var valNotifyCollection = Value as INotifyCollectionChanged;
			if (valNotifyCollection != null)
			{
				var weakEvent = new WeakEventListener<INotifyCollectionChanged, object, NotifyCollectionChangedEventArgs>(valNotifyCollection)
				{
					OnEventAction = (instance, source, args) => UpdateTrigger(),
					OnDetachAction = (instance, weakEventListener) => instance.CollectionChanged -= weakEventListener.OnEvent
				};

				valNotifyCollection.CollectionChanged += weakEvent.OnEvent;
				return;
			}

			// Not INotifyCollectionChanged, try IObservableVector
			var valObservableVector = Value as IObservableVector<object>;
			if (valObservableVector != null)
			{
				var weakEvent = new WeakEventListener<IObservableVector<object>, object, IVectorChangedEventArgs>(valObservableVector)
				{
					OnEventAction = (instance, source, args) => UpdateTrigger(),
					OnDetachAction = (instance, weakEventListener) => instance.VectorChanged -= weakEventListener.OnEvent
				};

				valObservableVector.VectorChanged += weakEvent.OnEvent;
				return;
			}

			// Not INotifyCollectionChanged, try IObservableMap
			var valObservableMap = Value as IObservableMap<object,object>;
			if (valObservableMap != null)
			{
				var weakEvent = new WeakEventListener<IObservableMap<object, object>, object, IMapChangedEventArgs<object>>(valObservableMap)
				{
					OnEventAction = (instance, source, args) => UpdateTrigger(),
					OnDetachAction = (instance, weakEventListener) => instance.MapChanged -= weakEventListener.OnEvent
				};

				valObservableMap.MapChanged += weakEvent.OnEvent;
			}
		}

		private static bool IsNullOrEmpty(object val)
		{
			if (val == null) return true;

			// Object is not null, check for an empty string
			var valString = val as string;
			if (valString != null)
			{
				return (valString.Length == 0);
			}

			// Object is not a string, check for an empty ICollection (faster)
			var valCollection = val as ICollection;
			if (valCollection != null)
			{
				return (valCollection.Count == 0);
			}

			// Object is not an ICollection, check for an empty IEnumerable
			var valEnumerable = val as IEnumerable;
			if (valEnumerable != null)
			{
				foreach (var item in valEnumerable)
				{
					// Found an item, not empty
					return false;
				}

				return true;
			}

			// Not null and not a known type to test for emptiness
			return false;
		}
	}
}
