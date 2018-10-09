using Client.ViewModels;
using SharedLib.ViewModels;
using SharedLib.Views;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for AppView.xaml
    /// </summary>
    [Export()]
    public partial class AppView : UserControl, IView
    {
        #region CONSTRUCTOR
        public AppView()
        {
            InitializeComponent();
        } 
        #endregion

        #region IMPORTS
        [Import()]
        public ISelectViewModelOfType<ICategoryDisplayViewModel> CategoryFilter
        {
            get; set;
        }
        #endregion

        #region EVENT HANDLER
        private void OnSelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            var added = new List<object>();
            var removed = new List<object>();

            if (e.NewValue is ICategoryDisplayViewModel current)
                added.Add(current);

            if (e.OldValue is ICategoryDisplayViewModel previous)
                removed.Add(previous);

            var args = new SelectionChangedEventArgs(e.RoutedEvent, removed, added)
            {
                Source = this
            };

            this.CategoryFilter.OnSelectionChanged(this, args);
        } 
        #endregion
    }
}
