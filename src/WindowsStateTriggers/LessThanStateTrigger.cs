// Copyright (c) Shawn Kendrot. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
    /// <summary>
    /// Enables a state if the value is less than another value.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Example: Trigger if a value is less than 1
    /// <code lang="xaml">
    ///     &lt;triggers:LessThanStateTrigger Value="{Binding Count}" MinValue="1" />
    /// </code>
    /// </para>
    /// </remarks>
    public class LessThanStateTrigger : StateTriggerBase
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
            typeof(LessThanStateTrigger),
            new PropertyMetadata(0, OnValuePropertyChanged));

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="MinValue"/> DependencyProperty
        /// </summary>
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            "MinValue",
            typeof(int),
            typeof(LessThanStateTrigger),
            new PropertyMetadata(0, OnValuePropertyChanged));

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var trigger = (LessThanStateTrigger)d;
            trigger.SetTriggerValue(trigger.Value < trigger.MinValue);
        }
    }
}