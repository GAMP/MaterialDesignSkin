using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    [Export()]
    public partial class HomeView : UserControl, IView
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ///Handle mouse wheel events to fix the scrolling problem
            if (e.Source is ItemsControl element)
            {
                e.Handled = true;
                var eventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = MouseWheelEvent
                };
                element.RaiseEvent(eventArgs);
            }
        }
    }
}
