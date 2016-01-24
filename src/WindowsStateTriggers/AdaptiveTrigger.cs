using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace WindowsStateTriggers
{
    /// <summary>
    /// Extends the <see cref="Windows.UI.Xaml.AdaptiveTrigger"/> functionality with 
    /// <see cref="ITriggerValue"/> interface implementation 
    /// for <see cref="CompositeStateTrigger"/> usage
    /// </summary>
    public class AdaptiveTrigger : Windows.UI.Xaml.AdaptiveTrigger, ITriggerValue
    {
        /// <summary>
		/// Initializes a new instance of the <see cref="AdaptiveTrigger"/> class.
		/// </summary>
        public AdaptiveTrigger()
        {
            this.RegisterPropertyChangedCallback(MinWindowHeightProperty, OnMinWindowHeightPropertyChanged);
            this.RegisterPropertyChangedCallback(MinWindowWidthProperty, OnMinWindowWidthPropertyChanged);
            
            var window = CoreApplication.GetCurrentView()?.CoreWindow;
            if (window != null)
            {
                var weakEvent = new WeakEventListener<AdaptiveTrigger, CoreWindow, WindowSizeChangedEventArgs>(this)
                {
                    OnEventAction = (instance, s, e) => OnCoreWindowOnSizeChanged(s, e),
                    OnDetachAction = (instance, weakEventListener) => window.SizeChanged -= weakEventListener.OnEvent
                };
                window.SizeChanged += weakEvent.OnEvent;
            }
        }

        private void OnCoreWindowOnSizeChanged(CoreWindow sender, WindowSizeChangedEventArgs args)
        {
            IsActive = args.Size.Height >= MinWindowHeight && args.Size.Width >= MinWindowWidth;
        }

        private void OnMinWindowHeightPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            var window = CoreApplication.GetCurrentView()?.CoreWindow;
            if (window != null)
            {
                IsActive = window.Bounds.Height >= MinWindowHeight;
            }
        }

        private void OnMinWindowWidthPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            var window = CoreApplication.GetCurrentView()?.CoreWindow;
            if (window != null)
            {
                IsActive = window.Bounds.Width >= MinWindowWidth;
            }
        }

        #region ITriggerValue

        private bool _isActive;

        /// <summary>
        /// Gets a value indicating whether this trigger is active.
        /// </summary>
        /// <value><c>true</c> if this trigger is active; otherwise, <c>false</c>.</value>
        public bool IsActive
        {
            get { return _isActive; }
            private set
            {
                if (_isActive != value)
                {
                    _isActive = value;
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
