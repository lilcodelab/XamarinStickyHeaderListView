using System;
using StickyHeaderList.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xarvio.FieldManager.iOS.CustomRenderers;

[assembly: ExportRenderer(typeof(StickyHeaderListView), typeof(StickyHeaderListViewRenderer))]
namespace Xarvio.FieldManager.iOS.CustomRenderers
{
    public class StickyHeaderListViewRenderer : ListViewRenderer
    {
        private bool _stickyHeader;
        private StickyHeaderListView _listView;

        private IDisposable _offsetObserver;

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (Control != null && Element is StickyHeaderListView list)
            {
                _listView = list;
            }

            if (e.NewElement is StickyHeaderListView)
                _offsetObserver = Control.AddObserver("contentOffset",
                             Foundation.NSKeyValueObservingOptions.New, HandleAction);
        }

        private void HandleAction(Foundation.NSObservedChange obj)
        {
            Scrolled();
        }

        public void Scrolled()
        {
            var scrollOffset = Control.ContentOffset.Y;

            if (scrollOffset > _listView.TreshholdOffset)
            {
                if (!_stickyHeader)
                {
                    _listView?.StickHeaderCommand?.Execute(null);
                    _stickyHeader = true;
                }
            }
            else
            {
                if (_stickyHeader)
                {
                    _listView?.UnstickHeaderCommand?.Execute(null);
                    _stickyHeader = false;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing && _offsetObserver != null)
            {
                _offsetObserver.Dispose();
                _offsetObserver = null;
            }
        }
    }
}