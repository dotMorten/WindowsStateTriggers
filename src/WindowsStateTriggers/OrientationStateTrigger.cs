using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
    /// <summary>
    /// Trigger for switching when the screen orientation changes
    /// </summary>
	public class OrientationStateTrigger : StateTriggerBase
	{
		public OrientationStateTrigger()
		{
			//TODO: Make this a weak event reference!
			DisplayInformation.GetForCurrentView().OrientationChanged += 
                OrientationStateTrigger_OrientationChanged;
		}

		private void OrientationStateTrigger_OrientationChanged(DisplayInformation sender, object args)
		{
            UpdateTrigger(sender.CurrentOrientation);
		}

        private void UpdateTrigger(Windows.Graphics.Display.DisplayOrientations orientation)
        {
            if (orientation == Windows.Graphics.Display.DisplayOrientations.None)
            {
                SetTriggerValue(false);
            }
            else if (orientation == Windows.Graphics.Display.DisplayOrientations.Landscape ||
               orientation == Windows.Graphics.Display.DisplayOrientations.LandscapeFlipped)
            {
                SetTriggerValue(Orientation == Orientations.Landscape);
            }
            else if (orientation == Windows.Graphics.Display.DisplayOrientations.Portrait ||
                    orientation == Windows.Graphics.Display.DisplayOrientations.PortraitFlipped)
            {
                SetTriggerValue(Orientation == Orientations.Portrait);
            }
        }

        public Orientations Orientation
		{
			get { return (Orientations)GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty, value); }
		}

		public static readonly DependencyProperty OrientationProperty =
			DependencyProperty.Register("Orientation", typeof(Orientations), typeof(OrientationStateTrigger), 
			new PropertyMetadata(Orientations.None, OnOrientationPropertyChanged));

		private static void OnOrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (OrientationStateTrigger)d;
			var orientation = DisplayInformation.GetForCurrentView().CurrentOrientation;
            obj.UpdateTrigger(orientation);
        }

		public enum Orientations 
		{
			None,
			Landscape,
			Portrait 
		}
	}
}
