using System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
    /// <summary>
    /// 
    /// </summary>
    public class InteractionCapabilityTrigger : StateTriggerBase, ITriggerValue
    {
        private bool m_IsActive;

        /// <summary>
		/// Occurs when the <see cref="IsActive" /> property has changed.
		/// </summary>
        public event EventHandler IsActiveChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionCapabilityTrigger"/> class.
        /// </summary>
        public InteractionCapabilityTrigger()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                var weakEvent =
                    new WeakEventListener<InteractionCapabilityTrigger, object, WindowSizeChangedEventArgs>(this)
                    {
                        OnEventAction = (instance, source, eventArgs) => InteractionCapabilitiesTrigger_SizeChanged(source, eventArgs),
                        OnDetachAction = (instance, weakEventListener) => Window.Current.SizeChanged -= weakEventListener.OnEvent
                    };
                Window.Current.SizeChanged += weakEvent.OnEvent;
                UpdateTrigger(InteractionMode);
            }
        }

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
		/// Gets or sets the InteractionMode to trigger on.
		/// </summary>
        public InteractionCapability InteractionMode
        {
            get { return (InteractionCapability)GetValue(InteractionModeProperty); }
            set { SetValue(InteractionModeProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="InteractionMode"/> parameter.
        /// </summary>
        public static readonly DependencyProperty InteractionModeProperty =
            DependencyProperty.Register("InteractionMode", typeof(InteractionCapability), typeof(InteractionCapabilityTrigger),
            new PropertyMetadata(InteractionCapability.SingleUserMultitouch, OnInteractionModeChanged));

        private static void OnInteractionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (InteractionCapabilityTrigger)d;
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                var result = (InteractionCapability)e.NewValue;
                obj.UpdateTrigger(result);
            }
        }

        private void UpdateTrigger(InteractionCapability interactionMode)
        {
            IsActive = interactionMode == InteractionCapabilityHelper.GetForCurrentView();
        }

        private void InteractionCapabilitiesTrigger_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            UpdateTrigger(InteractionMode);
        }


    }
}
