// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
	/// <summary>
	/// Enables a state if a type is present on the device
	/// </summary>
	/// <remarks>
	/// <para>
	/// Example: Checking for hardware back button availability:
	/// <code lang="xaml">
	///     &lt;triggers:IsTypePresentTrigger TypeName="Windows.Phone.UI.Input.HardwareButtons" />
	/// </code>
	/// </para>
	/// </remarks>
	public class IsTypePresentStateTrigger : StateTriggerBase, ITriggerValue
	{
		/// <summary>
		/// Gets or sets the name of the type.
		/// </summary>
		/// <remarks>
		/// Example: <c>Windows.Phone.UI.Input.HardwareButtons</c>
		/// </remarks>
		public string TypeName
		{
			get { return (string)GetValue(TypeNameProperty); }
			set { SetValue(TypeNameProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="TypeName"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty TypeNameProperty =
			DependencyProperty.Register("TypeName", typeof(string), typeof(IsTypePresentStateTrigger), 
			new PropertyMetadata("", OnTypeNamePropertyChanged));

		private static void OnTypeNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (IsTypePresentStateTrigger)d;
			var val = (string)e.NewValue;
			obj.IsActive = (!string.IsNullOrWhiteSpace(val) && ApiInformation.IsTypePresent(val));
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
