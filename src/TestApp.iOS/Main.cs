using Microsoft.Extensions.Logging;
using UIKit;
using Uno.Extensions;

namespace TestApp.iOS
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main(string[] args)
		{
#if DEBUG
			ConfigureFilters(LogExtensionPoint.AmbientLoggerFactory);
#endif

			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main(args, null, typeof(App));
		}


		static void ConfigureFilters(ILoggerFactory factory)
		{
			factory
				.WithFilter(new FilterLoggerSettings
					{
						{ "Uno", LogLevel.Warning },
						{ "Windows", LogLevel.Warning },
						{ "SampleControl.Presentation", LogLevel.Debug },

					// Generic Xaml events
					// { "Windows.UI.Xaml", LogLevel.Debug },

					// { "Uno.UI.Controls.AsyncValuePresenter", LogLevel.Debug },
					// { "Uno.UI.Controls.IfDataContext", LogLevel.Debug },

					// Layouter specific messages
					// { "Windows.UI.Xaml.Controls", LogLevel.Debug },
					//{ "Windows.UI.Xaml.Controls.Layouter", LogLevel.Debug },
					//{ "Windows.UI.Xaml.Controls.Panel", LogLevel.Debug },

					// Binding related messages
					 // { "Windows.UI.Xaml.Data", LogLevel.Debug },
					//{ "Windows.UI.Xaml.DependencyObjectStore", LogLevel.Debug },
					 { "Uno.UI.DataBinding.BindingPropertyHelper", LogLevel.Debug },

					//  Binder memory references tracking
					// { "ReferenceHolder", LogLevel.Debug },
				}
				)
				.AddConsole(LogLevel.Debug);
		}

	}
}