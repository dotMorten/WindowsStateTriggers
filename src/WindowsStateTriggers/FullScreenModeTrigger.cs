using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
	/// <summary>
	/// Trigger for switching when in full screen mode.
	/// </summary>
	public class FullScreenModeTrigger : StateTriggerBase, ITriggerValue
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FullScreenModeTrigger"/> class.
		/// </summary>
		public FullScreenModeTrigger()
		{
			if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
			{
				var weakEvent =
					new WeakEventListener<FullScreenModeTrigger, ApplicationView, object>(this)
					{
						OnEventAction = (instance, source, eventArgs) => FullScreenModeTrigger_VisibleBoundsChanged(source, eventArgs),
						OnDetachAction = (instance, weakEventListener) => ApplicationView.GetForCurrentView().VisibleBoundsChanged -= weakEventListener.OnEvent
					};
				ApplicationView.GetForCurrentView().VisibleBoundsChanged += weakEvent.OnEvent;
			}
		}

		private void FullScreenModeTrigger_VisibleBoundsChanged(ApplicationView sender, object args)
		{
			UpdateTrigger(sender.IsFullScreenMode);
		}

		private void UpdateTrigger(bool isFullScreen)
		{
			IsActive = (isFullScreen == IsFullScreen);
		}

		/// <summary>
		/// Gets or sets the full screen preference to trigger on.
		/// </summary>
		public bool IsFullScreen
		{
			get { return (bool)GetValue(IsFullScreenProperty); }
			set { SetValue(IsFullScreenProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="IsFullScreen"/> parameter.
		/// </summary>
		public static readonly DependencyProperty IsFullScreenProperty =
			DependencyProperty.Register("IsFullScreen", typeof(bool), typeof(FullScreenModeTrigger),
			new PropertyMetadata(false, OnIsFullScreenPropertyChanged));

		private static void OnIsFullScreenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (FullScreenModeTrigger)d;
			if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
			{
				var isFullScreen = ApplicationView.GetForCurrentView().IsFullScreenMode;
				obj.UpdateTrigger(isFullScreen);
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
