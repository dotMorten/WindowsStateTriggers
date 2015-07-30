using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestApp.Samples
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EnumSample : UserControl, INotifyPropertyChanged
    {
        public EnumSample()
        {
            this.InitializeComponent();
        }

        private AccessLevel selectedAccessLevel = AccessLevel.None;
        public AccessLevel SelectedAccessLevel
        {
            get { return selectedAccessLevel; }
            set
            {
                if (selectedAccessLevel != value)
                {
                    selectedAccessLevel = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedAccessLevel)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void accessLevelNone_Click(object sender, RoutedEventArgs e)
        {
            SelectedAccessLevel = AccessLevel.None;
        }

        private void accessLevelRead_Click(object sender, RoutedEventArgs e)
        {
            SelectedAccessLevel = AccessLevel.Reader;
        }

        private void accessLevelWrite_Click(object sender, RoutedEventArgs e)
        {
            SelectedAccessLevel = AccessLevel.Writer;
        }
    }

    public enum AccessLevel
    {
        None,
        Reader,
        Writer
    }
}
