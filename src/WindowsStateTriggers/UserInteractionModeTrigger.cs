using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
    /// <summary>
    /// Trigger for switching when the User interaction mode changes (tablet mode)
    /// </summary>
    public sealed class UserInteractionModeTrigger : StateTriggerBase, ITriggerValue
    {
        private bool m_IsActive;

        /// <summary>
		/// Occurs when the <see cref="IsActive" /> property has changed.
		/// </summary>
        public event EventHandler IsActiveChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInteractionModeTrigger"/> class.
        /// </summary>
        public UserInteractionModeTrigger()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                var weakEvent =
                    new WeakEventListener<UserInteractionModeTrigger, object, WindowSizeChangedEventArgs>(this)
                    {
                        OnEventAction = (instance, source, eventArgs) => UserInteractionModeTrigger_SizeChanged(source, eventArgs),
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
        public UserInteractionMode InteractionMode
        {
            get { return (UserInteractionMode)GetValue(InteractionModeProperty); }
            set { SetValue(InteractionModeProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="InteractionMode"/> parameter.
        /// </summary>
        public static readonly DependencyProperty InteractionModeProperty = 
            DependencyProperty.Register("InteractionMode", typeof(UserInteractionMode), typeof(UserInteractionModeTrigger), 
            new PropertyMetadata(UserInteractionMode.Mouse, OnInteractionModeChanged));

        private static void OnInteractionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (UserInteractionModeTrigger)d;
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                var orientation = (UserInteractionMode)e.NewValue;
                obj.UpdateTrigger(orientation);
            }
        }

        private void UpdateTrigger(UserInteractionMode interactionMode)
        {
            IsActive = interactionMode == UIViewSettings.GetForCurrentView().UserInteractionMode;
        }

        private void UserInteractionModeTrigger_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            UpdateTrigger(InteractionMode);
        }
    }
}
