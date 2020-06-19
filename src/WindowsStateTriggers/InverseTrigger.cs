// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
    /// <summary>
    /// Inverts another State Trigger that is using ITriggerValue
    /// </summary>
    public class InverseTrigger : StateTriggerBase, ITriggerValue
    {
        /// <summary>
        /// Gets or sets the State Trigger to invert.
        /// </summary>
        public ITriggerValue StateTrigger
        {
            get { return (ITriggerValue)GetValue(StateTriggerProperty); }
            set { SetValue(StateTriggerProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="StateTrigger"/> DependencyProperty
        /// </summary>
        public static readonly DependencyProperty StateTriggerProperty =
            DependencyProperty.Register("StateTrigger", typeof(ITriggerValue), typeof(InverseTrigger),
            new PropertyMetadata(null, OnStateTriggerPropertyChanged));

        private static void OnStateTriggerPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (InverseTrigger)d;
            var val = (ITriggerValue)e.NewValue;
            if (val != null)
            {
                obj.IsActive = !val.IsActive;
                WeakEventListener<ITriggerValue, object, EventArgs> weakEvent = new WeakEventListener<ITriggerValue, object, EventArgs>(val)
                {
                    OnEventAction = (instance, source, args) => obj.IsActive = !instance.IsActive,
                    OnDetachAction = (instance, weakEventListener) => instance.IsActiveChanged -= weakEventListener.OnEvent
                };
                val.IsActiveChanged += weakEvent.OnEvent;
            }
            else
            {
                obj.IsActive = false;
            }
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
