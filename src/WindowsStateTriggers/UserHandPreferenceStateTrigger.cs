// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
	/// <summary>
	/// Trigger for switching UI based on whether the user favours their left or right hand.
	/// </summary>
	public class UserHandPreferenceStateTrigger : StateTriggerBase, ITriggerValue
	{
		private static HandPreference handPreference;

		static UserHandPreferenceStateTrigger()
		{
			handPreference = new Windows.UI.ViewManagement.UISettings().HandPreference;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UserHandPreferenceStateTrigger"/> class.
		/// </summary>
		public UserHandPreferenceStateTrigger()
		{
			IsActive = (handPreference == HandPreference.RightHanded);
		}

		/// <summary>
		/// Gets or sets the hand preference to trigger on.
		/// </summary>
		/// <value>A value from the <see cref="Windows.UI.ViewManagement.HandPreference"/> enum.</value>
		public HandPreference HandPreference
		{
			get { return (HandPreference)GetValue(HandPreferenceProperty); }
			set { SetValue(HandPreferenceProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="HandPreference"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty HandPreferenceProperty =
			DependencyProperty.Register("HandPreference", typeof(HandPreference), typeof(UserHandPreferenceStateTrigger),
			new PropertyMetadata(Windows.UI.ViewManagement.HandPreference.RightHanded, OnHandPreferencePropertyChanged));

		private static void OnHandPreferencePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (UserHandPreferenceStateTrigger)d;
			var val = (HandPreference)e.NewValue;
			obj.IsActive = (handPreference == val);
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