// Copyright (c) Shawn Kendrot. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
    /// <summary>
    /// Enables a state if the value is greater than another value
    /// </summary>
    /// <remarks>
    /// <para>
    /// Example: Trigger if a value is greater than 0
    /// <code lang="xaml">
    ///     &lt;triggers:GreaterThanStateTrigger Value="{Binding Count}" MaxValue="0" />
    /// </code>
    /// </para>
    /// </remarks>
    public class GreaterThanStateTrigger : StateTriggerBase
    {
        /// <summary>
        /// Gets or sets the value for comparison.
        /// </summary>
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Value"/> DependencyProperty
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(int),
            typeof(GreaterThanStateTrigger),
            new PropertyMetadata(0, OnValuePropertyChanged));

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="MaxValue"/> DependencyProperty
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            "MaxValue",
            typeof(int),
            typeof(GreaterThanStateTrigger),
            new PropertyMetadata(0, OnValuePropertyChanged));

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var trigger = (GreaterThanStateTrigger)d;
            trigger.SetTriggerValue(trigger.Value > trigger.MaxValue);
        }
    }
}