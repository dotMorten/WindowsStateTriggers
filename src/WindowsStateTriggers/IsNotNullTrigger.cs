using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
    public class IsNotNullTrigger : StateTriggerBase
    {
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(
            "Item", typeof(object), typeof(IsNotNullTrigger), new PropertyMetadata(default(object), OnItemChanged));

        public object Item
        {
            get { return GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        private static void OnItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var trigger = sender as IsNotNullTrigger;
            var item = e.NewValue;

            trigger?.SetTriggerValue(item != null);
        }
    }
}