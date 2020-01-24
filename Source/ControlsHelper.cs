using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MaterialDesignSkin.Source
{
    /// <summary>
    /// Generic controls helper class.
    /// </summary>
    public static class ControlHelper
    {
        #region ATTACHED PROPERTIES

        public static readonly DependencyProperty DisableDoubleClickProperty = DependencyProperty.RegisterAttached("DisableDoubleClick", typeof(bool), typeof(ControlHelper), new FrameworkPropertyMetadata(false, OnDisableDoubleClickPropertyChanged));

        #endregion

        #region SETTERS
        public static void SetDisableDoubleClick(UIElement element, bool value)
        {
            element.SetValue(DisableDoubleClickProperty, value);
        }
        #endregion

        #region GETTERS
        public static bool GetDisableDoubleClick(UIElement element)
        {
            return (bool)element.GetValue(DisableDoubleClickProperty);
        }
        #endregion

        #region PROPERTY CHANGED CALLBACKS
        private static void OnDisableDoubleClickPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Control control)
            {
                if ((bool)e.NewValue)
                {
                    //attach handler
                    control.PreviewMouseDown += OnDependencyObjectPreviewMouseDown;
                }
                else
                {
                    //detach handler
                    control.PreviewMouseDown -= OnDependencyObjectPreviewMouseDown;
                }
            }
        }
        #endregion

        #region HANDLERS
        private static void OnDependencyObjectPreviewMouseDown(object sender, MouseButtonEventArgs args)
        {
            if (args.ClickCount > 1)
                args.Handled = true;
        }
        #endregion
    }
}
