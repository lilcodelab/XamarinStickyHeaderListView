# List view with a sticky header in xamarin forms

Today I will show you how to create a list view with a sticky header in Xamarin Forms for iOS and Android.

![alt-text](https://github.com/JukiJ/XamarinStickyHeaderListView/blob/main/example.gif)

The idea behind it is that often mobile applications have a part on top of the screen which gets scrolled of the screen but small part of it is left and sticked on top. It will be a very simple list view just to showcase the functionality and how to achieve
it without much styling added to it.

### Step 1: Create a control in shared project:

Create a new class in your shared project and name it something like StickyHeaderListView. I usually put all these classes under one folder named Controls or something similar to that. That class you make should inherit from ListView of Xamarin forms as we will just enhance its functionality. It will look something like this:

``` csharp
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
```

First of all it has 2 constructors. The parameterless one and the one with one parameter for caching strategy of the list view which is needed if you will specify it in xaml. After that it has a bindable property for a threshold value of a scroll offset when it will show the sticking header. Here that value is specified directly in the BindableProperty definition,
but also you bind it from a view model if that value will depend on the data in the list. Next up are two bindable properties for commands which are fired either when the header is sticked or unsticked (shown or unshown).

### Step 2: Add StickyHeaderListView to your page:
	
In the Xaml of the page you want to add the sticky header to just add the StickyHeaderListView and assign all the properties as you would normally do with a list view. You will also need to bind those two commands from the control itself (Stick and UnstickHeaderCommand). Caching strategy needs to be added inside x:Arguments block as otherwise it does not work. Since
usually if you need to have a sticky header your whole page will usually revolve around that list and the part above the list would be scrolled off so you can put the things above inside a list view header. Otherwise a different approach would be needed which will be discussed in a different article. 

Whole page is visible below:

``` xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage
  x:Class="StickyHeaderList.MainPage"
  xmlns="http://xamarin.com/schemas/2014/forms"
  xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
  xmlns:controls="clr-namespace:StickyHeaderList.Controls"
  xmlns:ios="clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core"
  ios:Page.UseSafeArea="True"
  BackgroundColor="DarkGray">
  <Grid>

    <controls:StickyHeaderListView
      BackgroundColor="DarkGray"
      HasUnevenRows="true"
      HeightRequest="1000000"
      ItemsSource="{Binding ListItems}"
      SelectionMode="None"
      SeparatorVisibility="None"
      StickHeaderCommand="{Binding StickHeaderCommand}"
      UnstickHeaderCommand="{Binding UnstickHeaderCommand}">

      <x:Arguments>
        <ListViewCachingStrategy>RecycleElement</ListViewCachingStrategy>
      </x:Arguments>

      <controls:StickyHeaderListView.Header>
        <Grid BackgroundColor="DarkGray" HeightRequest="250">
          <Grid.RowDefinitions>
            <RowDefinition Height="150" />
            <RowDefinition Height="100" />
          </Grid.RowDefinitions>
          <Label
            Text="This is a showcase for sticky header list view in xamarin forms"
            TextColor="White"
            VerticalOptions="CenterAndExpand" />

          <StackLayout
            Grid.Row="1"
            BackgroundColor="DarkGray"
            HeightRequest="100"
            HorizontalOptions="FillAndExpand"
            VerticalOptions="Start">
            <Label
              FontSize="Medium"
              HorizontalOptions="Center"
              Text="Still here!"
              TextColor="White"
              VerticalOptions="CenterAndExpand" />
          </StackLayout>
        </Grid>
      </controls:StickyHeaderListView.Header>

      <controls:StickyHeaderListView.ItemTemplate>
        <DataTemplate>
          <ViewCell>
            <StackLayout BackgroundColor="Gray" HeightRequest="100">
              <Label
                HorizontalOptions="Center"
                Text="{Binding .}"
                TextColor="White"
                VerticalOptions="CenterAndExpand" />
            </StackLayout>
          </ViewCell>
        </DataTemplate>
      </controls:StickyHeaderListView.ItemTemplate>

    </controls:StickyHeaderListView>

    <StackLayout
      BackgroundColor="DarkGray"
      HeightRequest="100"
      HorizontalOptions="FillAndExpand"
      IsVisible="{Binding ShowStickyHeader}"
      VerticalOptions="Start">
      <Label
        FontSize="Medium"
        HorizontalOptions="Center"
        Text="Still here!"
        TextColor="White"
        VerticalOptions="CenterAndExpand" />
    </StackLayout>
  </Grid>

</ContentPage>

```
As you can see in the picture the ListView and the part which will be sticked on top of it are both inside a same grid so the elements can be shown one above other. The elements above the list are inside its header so they can be scrolled of the screen. The bottom part of the header should stick on the list. Bottom part is replicated in StackLayout below the list and that part will be shown when needed.

### Step 3: ViewModel:

Inside the view model you should just add two commands for which you created a binding in the Xaml of the page. Actions performed by these two commands are shown below. In this case it is just simply Showing or hiding a sticky header through a boolean property binded to it.

``` csharp
private void StickHeader()
{
    ShowStickyHeader = true;
}

private void UnstickHeader()
{
    ShowStickyHeader = false;
}
```

ShowStickyHeader is a boolean property added to the view model to be binded to the Xaml of the page.

### Step 4: Custom renderer for android:

Next step is creating a custom renderer for android where the logic for displaying the sticky header or not is contained. First of all you need to create a custom scroll listener. We shall call it MyScrollListener and it should inherit from Java.Lang.Object and implement IOnScrollListener interface. 

``` csharp
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
```

As you can see in the code above the logic is pretty simple. Most important part of it is the part where we are checking if the top offset of headerView is bigger than thresholdHeight you set earlier in caase that header view is the top view in list (list header), or if it isnt then we know it is scrolled much more down and the sticky header needs to be shown. Also in that if-else block you immediately fire StickHeaderCommand or UnstickHeaderCommand when stickyShown variable is changed. 

### Step 5: Custom renderer for iOS:

Also you need to do the same for iOS, just the iOS is easier a bit. First you need to create a method to handle the scroll when it happens which will take NSObservedChange as a parameter and use it to call other method which contains the logic for determining should the sticky header be shown or not based on the scroll offset of the native Control. Then you just
need to add a new observer to the native Control in the renderer and create it with the method you created earlier.

``` csharp
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
```

That is it, I just showed you a very simple yet effective way on how to show a sticky header in the list view in xamarin forms. I hope you enjoyed the article, if you have any questions feel free to ask me!
