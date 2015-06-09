// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Specialized;
using Windows.Foundation.Collections;
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
		public object Value
		{
			get { return GetValue(ValueProperty); }
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

			obj.IsActive = IsNullOrEmpty(val);

			if (val == null)
				return;

			// Try to listen for various notification events
			// Starting with INorifyCollectionChanged
			var valNotifyCollection = val as INotifyCollectionChanged;
			if (valNotifyCollection != null)
			{
				var weakEvent = new WeakEventListener<INotifyCollectionChanged, object, NotifyCollectionChangedEventArgs>(valNotifyCollection)
				{
					OnEventAction = (instance, source, args) => obj.IsActive = IsNullOrEmpty(instance),
					OnDetachAction = (instance, weakEventListener) => instance.CollectionChanged -= weakEventListener.OnEvent
				};

				valNotifyCollection.CollectionChanged += weakEvent.OnEvent;
				return;
			}

			// Not INotifyCollectionChanged, try IObservableVector
			var valObservableVector = val as IObservableVector<object>;
			if (valObservableVector != null)
			{
				var weakEvent = new WeakEventListener<IObservableVector<object>, object, IVectorChangedEventArgs>(valObservableVector)
				{
					OnEventAction = (instance, source, args) => obj.IsActive = IsNullOrEmpty(instance),
					OnDetachAction = (instance, weakEventListener) => instance.VectorChanged -= weakEventListener.OnEvent
				};

				valObservableVector.VectorChanged += weakEvent.OnEvent;
				return;
			}

			// Not INotifyCollectionChanged, try IObservableMap
			var valObservableMap = val as IObservableMap<object,object>;
			if (valObservableMap != null)
			{
				var weakEvent = new WeakEventListener<IObservableMap<object, object>, object, IMapChangedEventArgs<object>>(valObservableMap)
				{
					OnEventAction = (instance, source, args) => obj.IsActive = IsNullOrEmpty(instance),
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
