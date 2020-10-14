using Android.Content;
using Android.Runtime;
using Android.Widget;
using ListViewStickyHeader.Droid.Renderers;
using StickyHeaderList.Controls;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Widget.AbsListView;

[assembly: ExportRenderer(typeof(StickyHeaderListView), typeof(StickyHeaderListViewRenderer))]
namespace ListViewStickyHeader.Droid.Renderers
{
    public class StickyHeaderListViewRenderer : ListViewRenderer
    {
        public StickyHeaderListViewRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            if (Control != null && Element is StickyHeaderListView list)
            {
                Control.SetOnScrollListener(new MyScrollListener(list));
            }
        }
    }

    public class MyScrollListener : Java.Lang.Object, IOnScrollListener
    {
        private StickyHeaderListView _customListView;

        private bool stickyShown = false;
        private Android.Views.View topView;

        public MyScrollListener(StickyHeaderListView customList)
        {
            _customListView = customList;
        }

        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            var headerView = view.GetChildAt(0);

            var thresholdHeight = _customListView.TreshholdOffset * DeviceDisplay.MainDisplayInfo.Density;

            if (topView == null && headerView != null)
                topView = headerView;

            if (headerView?.Top <= -thresholdHeight && headerView == topView || headerView != topView)
            {
                if (!stickyShown)
                {
                    stickyShown = true;
                    _customListView.StickHeaderCommand?.Execute(null);
                }
            }
            else
            {
                if (stickyShown)
                {
                    stickyShown = false;
                    _customListView.UnstickHeaderCommand?.Execute(null);
                }
            }
        }

        public void OnScrollStateChanged(AbsListView view, [GeneratedEnum] ScrollState scrollState)
        {

        }
    }
}