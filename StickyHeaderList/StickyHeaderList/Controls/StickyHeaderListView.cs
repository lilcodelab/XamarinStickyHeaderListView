using System.Windows.Input;
using Xamarin.Forms;

namespace StickyHeaderList.Controls
{
    public class StickyHeaderListView : ListView
    {
        public StickyHeaderListView() { }

        public StickyHeaderListView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy) { }


        public static BindableProperty TreshholdOffsetProperty =
            BindableProperty.Create(nameof(TreshholdOffset), typeof(double), typeof(StickyHeaderListView), 150.0);

        public double TreshholdOffset
        {
            get => (double)GetValue(TreshholdOffsetProperty);
            set => SetValue(TreshholdOffsetProperty, value);
        }

        public static BindableProperty StickHeaderCommandProperty =
            BindableProperty.Create(nameof(StickHeaderCommand), typeof(ICommand), typeof(StickyHeaderListView), defaultBindingMode: BindingMode.TwoWay);

        public ICommand StickHeaderCommand
        {
            get => (ICommand)GetValue(StickHeaderCommandProperty);
            set => SetValue(StickHeaderCommandProperty, value);
        }

        public static BindableProperty UnstickHeaderCommandProperty =
            BindableProperty.Create(nameof(UnstickHeaderCommand), typeof(ICommand), typeof(StickyHeaderListView), defaultBindingMode: BindingMode.TwoWay);

        public ICommand UnstickHeaderCommand
        {
            get => (ICommand)GetValue(UnstickHeaderCommandProperty);
            set => SetValue(UnstickHeaderCommandProperty, value);
        }
    }
}
