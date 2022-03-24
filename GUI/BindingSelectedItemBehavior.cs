using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace GUI
{
    internal class BindingSelectedItemBehavior : Behavior<TreeView>
    {
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(BindingSelectedItemBehavior), new UIPropertyMetadata(null, OnSelectedItemChanged));
        
        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TreeViewItem item = e.NewValue as TreeViewItem;

            if (e.NewValue == null && e.OldValue is TreeViewItem)
                (e.OldValue as TreeViewItem).SetValue(TreeViewItem.IsSelectedProperty, false);

            if (item != null)
                item.SetValue(TreeViewItem.IsSelectedProperty, true);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectedItemChanged += treeView_SelectedItemChanged;
        }
        
        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {

                AssociatedObject.SelectedItemChanged -= treeView_SelectedItemChanged;
            }
        }

        private void treeView_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            if(sender is TreeView)
            {
                e.Handled = true;
                SelectedItem = null;
            }
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
        }
    }
}
