// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace WindowsStateTriggers
{
	/// <summary>
	/// Implements a weak event listener that allows the owner to be garbage
	/// collected if its only remaining link is an event handler.
	/// </summary>
	/// <typeparam name="TInstance">Type of instance listening for the event.</typeparam>
	/// <typeparam name="TSource">Type of source for the event.</typeparam>
	/// <typeparam name="TEventArgs">Type of event arguments for the event.</typeparam>
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Used as link target in several projects.")]
	internal class WeakEventListener<TInstance, TSource, TEventArgs> where TInstance : class
	{
		/// <summary>
		/// WeakReference to the instance listening for the event.
		/// </summary>
		private WeakReference _weakInstance;

		/// <summary>
		/// Gets or sets the method to call when the event fires.
		/// </summary>
		public Action<TInstance, TSource, TEventArgs> OnEventAction { get; set; }

		/// <summary>
		/// Gets or sets the method to call when detaching from the event.
		/// </summary>
		public Action<TInstance, WeakEventListener<TInstance, TSource, TEventArgs>> OnDetachAction { get; set; }

		/// <summary>
		/// Initializes a new instances of the WeakEventListener class.
		/// </summary>
		/// <param name="instance">Instance subscribing to the event.</param>
		public WeakEventListener(TInstance instance)
		{
			if (null == instance)
			{
				throw new ArgumentNullException("instance");
			}
			_weakInstance = new WeakReference(instance);
		}

		/// <summary>
		/// Handler for the subscribed event calls OnEventAction to handle it.
		/// </summary>
		/// <param name="source">Event source.</param>
		/// <param name="eventArgs">Event arguments.</param>
		public void OnEvent(TSource source, TEventArgs eventArgs)
		{
			TInstance target = (TInstance)_weakInstance.Target;
			if (null != target)
			{
				// Call registered action
				if (null != OnEventAction)
				{
					OnEventAction(target, source, eventArgs);
				}
			}
			else
			{
				// Detach from event
				Detach();
			}
		}

		/// <summary>
		/// Detaches from the subscribed event.
		/// </summary>
		public void Detach()
		{
			TInstance target = (TInstance)_weakInstance.Target;
			if (null != OnDetachAction)
			{
				OnDetachAction(target, this);
				OnDetachAction = null;
			}
		}
	}


	/// <summary>
	/// Implements a weak event listener that allows the owner to be garbage
	/// collected if its only remaining link is an event handler.
	/// </summary>
	/// <typeparam name="TInstance">Type of instance listening for the event.</typeparam>
	/// <typeparam name="TSource">Type of source for the event.</typeparam>
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Used as link target in several projects.")]
	internal class WeakEventListener<TInstance, TSource> where TInstance : class
	{
		/// <summary>
		/// WeakReference to the instance listening for the event.
		/// </summary>
		private WeakReference _weakInstance;

		/// <summary>
		/// Gets or sets the method to call when the event fires.
		/// </summary>
		public Action<TInstance, TSource> OnEventAction { get; set; }

		/// <summary>
		/// Gets or sets the method to call when detaching from the event.
		/// </summary>
		public Action<TInstance, WeakEventListener<TInstance, TSource>> OnDetachAction { get; set; }

		/// <summary>
		/// Initializes a new instances of the WeakEventListener class.
		/// </summary>
		/// <param name="instance">Instance subscribing to the event.</param>
		public WeakEventListener(TInstance instance)
		{
			if (null == instance)
			{
				throw new ArgumentNullException("instance");
			}
			_weakInstance = new WeakReference(instance);
		}

		/// <summary>
		/// Handler for the subscribed event calls OnEventAction to handle it.
		/// </summary>
		/// <param name="source">Event source.</param>
		public void OnEvent(TSource source)
		{
			TInstance target = (TInstance)_weakInstance.Target;
			if (null != target)
			{
				// Call registered action
				if (null != OnEventAction)
				{
					OnEventAction(target, source);
				}
			}
			else
			{
				// Detach from event
				Detach();
			}
		}

		/// <summary>
		/// Detaches from the subscribed event.
		/// </summary>
		public void Detach()
		{
			TInstance target = (TInstance)_weakInstance.Target;
			if (null != OnDetachAction)
			{
				OnDetachAction(target, this);
				OnDetachAction = null;
			}
		}
	}

}
