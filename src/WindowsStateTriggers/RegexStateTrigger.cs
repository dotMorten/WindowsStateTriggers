// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
	/// <summary>
	/// Enables a state if the regex expression is true for a given string value
	/// </summary>
	/// <remarks>
	/// <para>
	/// Example: Trigger user entered a valid email
	/// <code lang="xaml">
	///     &lt;triggers:RegexStateTrigger Value="{x:Bind myTextBox.Text}" Expression="^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$" Options="IgnoreCase" />
	/// </code>
	/// </para>
	/// </remarks>
	public class RegexStateTrigger : StateTriggerBase, ITriggerValue
	{
		private void UpdateTrigger()
		{
			IsActive =
					Value != null && 
					!string.IsNullOrEmpty(Expression) &&
                    Regex.IsMatch(Value, Expression, Options);
		}

		/// <summary>
		/// Gets or sets the value for regex evaluation.
		/// </summary>
		public string Value
		{
			get { return (string)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="Value"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register(nameof(Value), typeof(string), typeof(RegexStateTrigger), 
			new PropertyMetadata(null, OnValuePropertyChanged));

		private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (RegexStateTrigger)d;
			obj.UpdateTrigger();
		}

		/// <summary>
		/// Gets or sets the regular expression.
		/// </summary>
		public string Expression
		{
			get { return (string)GetValue(ExpressionProperty); }
			set { SetValue(ExpressionProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="Expression"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty ExpressionProperty =
					DependencyProperty.Register(nameof(Expression), typeof(string), typeof(RegexStateTrigger), new PropertyMetadata(null, OnValuePropertyChanged));
		
		/// <summary>
		/// Gets or sets the regular expression options
		/// </summary>
		public RegexOptions Options
		{
			get { return (RegexOptions)GetValue(OptionsProperty); }
			set { SetValue(OptionsProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="Options"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty OptionsProperty =
			DependencyProperty.Register(nameof(Options), typeof(RegexOptions), typeof(RegexStateTrigger), new PropertyMetadata(RegexOptions.None, OnValuePropertyChanged));
		
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
