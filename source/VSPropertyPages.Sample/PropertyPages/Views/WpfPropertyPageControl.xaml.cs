using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace VSPropertyPages.Sample.PropertyPages
{
    /// <summary>
    /// Interaction logic for WpfPropertyPageControl.xaml
    /// </summary>
    internal partial class WpfPropertyPageControl : WpfPropertyPageUI
    {
        public WpfPropertyPageControl()
        {
            InitializeComponent();
        }

        public override Task SetViewModelAsync(PropertyPageViewModel propertyPageViewModel)
        {
            DataContext = propertyPageViewModel;
            return Task.CompletedTask;
        }
    }

    // code from: https://stackoverflow.com/a/5306565/4647866
    public class MarginSetter
    {
        public static Thickness GetMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(MarginProperty);
        }

        public static void SetMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(MarginProperty, value);
        }

        // Using a DependencyProperty as the backing store for Margin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarginProperty =
            DependencyProperty.RegisterAttached("Margin", typeof(Thickness), typeof(MarginSetter), new UIPropertyMetadata(new Thickness(), CreateThicknesForChildren));

        public static void CreateThicknesForChildren(object sender, DependencyPropertyChangedEventArgs e)
        {
            var panel = sender as Panel;

            if (panel == null) return;

            foreach (var child in panel.Children)
            {
                var fe = child as FrameworkElement;

                if (fe == null) continue;

                fe.Margin = GetMargin(panel);
            }
        }
    }
}
