using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
    /// <summary>
    /// A state trigger for determining if the app is in a hosted state (for example, if share is being used)
    /// </summary>
    public class HostedTrigger : StateTriggerBase
    {
        /// <summary>
        /// The hosted required property
        /// </summary>
        public static readonly DependencyProperty HostedRequiredProperty = DependencyProperty.Register(
            "HostedRequired", typeof (bool), typeof (HostedTrigger), new PropertyMetadata(default(bool), OnHostedRequiredChanged));

        /// <summary>
        /// Gets or sets a value indicating whether [hosted required].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [hosted required]; otherwise, <c>false</c>.
        /// </value>
        public bool HostedRequired
        {
            get { return (bool) GetValue(HostedRequiredProperty); }
            set { SetValue(HostedRequiredProperty, value); }
        }

        /// <summary>
        /// Called when [hosted required changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnHostedRequiredChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as HostedTrigger)?.UpdateStateTrigger();
        }

        /// <summary>
        /// Updates the state trigger.
        /// </summary>
        private void UpdateStateTrigger()
        {
            var isHosted = CoreApplication.GetCurrentView().IsHosted;
            base.SetActive(isHosted && HostedRequired);
        }
    }
}
