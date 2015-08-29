using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
    /// <summary>
    /// A state trigger for determining if the app is in a hosted state (for example, if share is being used)
    /// </summary>
    public class HostedStateTrigger : StateTriggerBase
    {
        /// <summary>
        /// Identifies the <see cref="IsHosted" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsHostedProperty = DependencyProperty.Register(
            "IsHosted", typeof (HostedState), typeof (HostedStateTrigger), new PropertyMetadata(HostedState.Unknown, OnIsHostedRequiredChanged));

        /// <summary>
        /// Gets or sets the is hosted value.
        /// </summary>
        public HostedState IsHosted
        {
            get { return (HostedState) GetValue(IsHostedProperty); }
            set { SetValue(IsHostedProperty, value); }
        }

        /// <summary>
        /// Called when the IsHosted depenedency property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnIsHostedRequiredChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as HostedStateTrigger)?.UpdateStateTrigger();
        }

        /// <summary>
        /// Updates the state trigger.
        /// </summary>
        private void UpdateStateTrigger()
        {
            var isHosted = CoreApplication.GetCurrentView().IsHosted;
            if (isHosted)
            {
                SetActive(IsHosted == HostedState.Hosted);
            }
            else
            {
                SetActive(IsHosted == HostedState.NotHosted);
            }
        }

        /// <summary>
        /// Hosted State enum
        /// </summary>
        public enum HostedState
        {
            /// <summary>
            /// The unknown state
            /// </summary>
            Unknown,

            /// <summary>
            /// The hosted state
            /// </summary>
            Hosted,

            /// <summary>
            /// The not hosted state
            /// </summary>
            NotHosted
        }
    }
}
