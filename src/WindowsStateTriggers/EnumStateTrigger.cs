using System;
using Windows.UI.Xaml;
using System.Linq;
using System.Collections.Generic;

namespace WindowsStateTriggers
{
    /// <summary>
    /// Enables a state if the enum has one of the specified values.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <code lang="xaml">
    ///     &lt;triggers:EnumStateTrigger Value="{x:Bind CurrentAccessLevel}" ActiveValues="Writer, Reader" />
    /// </code>
    /// </para>
    /// </remarks>
    public class EnumStateTrigger : StateTriggerBase, ITriggerValue
    {
        private void UpdateTrigger()
        {
            if (Value == null || ActiveValues == null)
            {
                IsActive = false;
            }
            else
            {
                var currentStates = Value.ToString().ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim()).ToList();
                var stateStrings = ActiveValues.ToString().ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim()).ToList();

                IsActive = currentStates.Intersect(stateStrings).Any();
            }
        }

        /// <summary>
        /// Gets or sets the value of the enum.
        /// </summary>
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Value"/> DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(EnumStateTrigger),
            new PropertyMetadata(null, ValuePropertyChanged));

        /// <summary>
        /// Gets or sets the valid values for the state to be enabled.
        /// </summary>
        public string ActiveValues
        {
            get { return (string)GetValue(ActiveValuesProperty); }
            set { SetValue(ActiveValuesProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ActiveValues"/> DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty ActiveValuesProperty =
            DependencyProperty.Register("ActiveValues", typeof(string), typeof(EnumStateTrigger),
            new PropertyMetadata(null, ValuePropertyChanged));

        private static void ValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var obj = (EnumStateTrigger)sender;
            obj.UpdateTrigger();
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
                    IsActiveChanged?.Invoke(this, EventArgs.Empty);
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
