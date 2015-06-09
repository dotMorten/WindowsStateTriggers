// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.RegularExpressions;
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
	public class RegexStateTrigger : ConditionStateTriggerBase<string>
	{
		/// <summary>
		/// Predicate that causes the trigger to activate when satisfied.
		/// </summary>
		/// <param name="value">The value used as input to this trigger.</param>
		/// <returns>A <see cref="bool"/> indicating whether the trigger is active.</returns>
		protected override bool Condition(string value)
		{
			return
				value != null &&
				!string.IsNullOrEmpty(Expression) &&
				Regex.IsMatch(value, Expression, Options);
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
	}
}
