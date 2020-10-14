using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace StickyHeaderList
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Boolean which determines if the sticked header is shown or not
        /// </summary>
        private bool _showStickyHeader; 
        public bool ShowStickyHeader
        {
            get => _showStickyHeader;
            set
            {
                if (value == _showStickyHeader)
                    return;

                _showStickyHeader = value;
                OnPropertyChanged(nameof(ShowStickyHeader));
            }
        }

        private List<string> _listItems;
        public List<string> ListItems
        {
            get => _listItems;
            set
            {
                if (_listItems == value)
                    return;

                _listItems = value;
                OnPropertyChanged(nameof(ListItems));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Command to stick the header
        /// </summary>
        private ICommand _stickHeaderCommand;
        public ICommand StickHeaderCommand => _stickHeaderCommand ?? (_stickHeaderCommand = new Command(StickHeader));

        /// <summary>
        /// Command to unstick the header
        /// </summary>
        private ICommand _unstickHeaderCommand;
        public ICommand UnstickHeaderCommand => _unstickHeaderCommand ?? (_unstickHeaderCommand = new Command(UnstickHeader));

        public MainPageViewModel()
        {
            var list = new List<string>();

            for (int i = 0; i < 100; i++)
            {
                list.Add($"Item {i}");
            }

            ListItems = list;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StickHeader()
        {
            ShowStickyHeader = true;
        }

        private void UnstickHeader()
        {
            ShowStickyHeader = false;
        }
    }
}
