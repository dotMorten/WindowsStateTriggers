// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Windows.Devices.Input;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
	/// <summary>
	/// Enables a state based on input type used
	/// </summary>
	public class InputTypeTrigger : StateTriggerBase, ITriggerValue
	{
		/// <summary>
		/// Gets or sets the type of the pointer used.
		/// </summary>
		/// <value>The type of the pointer.</value>
		public PointerDeviceType PointerType
		{
			get { return (PointerDeviceType)GetValue(PointerTypeProperty); }
			set { SetValue(PointerTypeProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="PointerType"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty PointerTypeProperty =
			DependencyProperty.Register("PointerType", typeof(PointerDeviceType), typeof(InputTypeTrigger), new PropertyMetadata(PointerDeviceType.Pen));

		/// <summary>
		/// Gets or sets the target element.
		/// </summary>
		public FrameworkElement TargetElement
		{
			get { return GetValue(TargetElementProperty) as FrameworkElement; }
			set { SetValue(TargetElementProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="TargetElement"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty TargetElementProperty =
			DependencyProperty.Register("TargetElement", typeof(string), typeof(InputTypeTrigger), 
			new PropertyMetadata("", OnTargetElementPropertyChanged));

		private static void OnTargetElementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (InputTypeTrigger)d;
			var valOld = e.OldValue as FrameworkElement;
			if(valOld != null)
			{
				valOld.PointerPressed -= obj.TargetElement_PointerPressed;
			}
			var val = e.NewValue as FrameworkElement;
			if (val != null)
			{
				val.PointerPressed += obj.TargetElement_PointerPressed;
			}
		}

		private void TargetElement_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			IsActive = (e.Pointer.PointerDeviceType == PointerType);
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
