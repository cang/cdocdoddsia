using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SiGlaz.UI.CustomControls
{
    public interface IDataItemController
    {
        ListViewItem Create(int idx);
        int Search(SearchForVirtualItemEventArgs e);

        void ClearImageLists();

        void Dispose();
    }

    public class VirtualListView : ListView
    {
        #region Member fields
        protected IDataItemController _itemController = null;
        [System.ComponentModel.Browsable(false)]
        public IDataItemController DataItemController
        {
            get { return _itemController; }
            set
            {
                if (_itemController != value)
                {
                    _itemController.Dispose();
                    _itemController = null;
                    _itemController = value;
                }
            }
        }

        protected List<ListViewItem> _cachedItems = null;
        [System.ComponentModel.Browsable(false)]
        public List<ListViewItem> CahcedItems
        {
            get { return _cachedItems; }
            set 
            {
                if (_cachedItems != value)
                {
                    if (_cachedItems != null)
                    {
                        _cachedItems.Clear();
                        // force to GC collection memory
                        GC.Collect();
                    }

                    _cachedItems = value;
                }
            }
        }

        protected int _firstCachedItemIdx = 0;
        [System.ComponentModel.Browsable(false)]
        public int FirstCachedItemIdx
        {
            get { return _firstCachedItemIdx; }
            set { _firstCachedItemIdx = value; }
        }
        #endregion Member fields

        #region Constructors and destructors
        public VirtualListView(
            IDataItemController dataItemController) : base()
        {
            _itemController = dataItemController;
            this.VirtualMode = true;
        }

        public VirtualListView()
            : this(null)
        {

        }
        #endregion Constructors and destructors

        #region Overrides
        //The basic VirtualMode function.  Dynamically returns a ListViewItem
        protected override void OnRetrieveVirtualItem(RetrieveVirtualItemEventArgs e)
        {
            base.OnRetrieveVirtualItem(e);

            //Caching is not required but improves performance on large sets.
            //To leave out caching, don't connect the CacheVirtualItems event 
            //and make sure myCache is null.

            //check to see if the requested item is currently in the cache
            if (_cachedItems != null &&
                e.ItemIndex >= _firstCachedItemIdx &&
                e.ItemIndex < _firstCachedItemIdx + _cachedItems.Count)
            {
                //A cache hit, 
                //so get the ListViewItem from the cache instead of making a new one.
                e.Item = _cachedItems[e.ItemIndex - _firstCachedItemIdx];
            }
            else
            {
                //A cache miss, so create a new ListViewItem and pass it back.
                if (_itemController != null)
                    e.Item = _itemController.Create(e.ItemIndex);
                else
                    e.Item = null;
            }
        }

        // ListView calls this when it might need a cache refresh.
        protected override void OnCacheVirtualItems(CacheVirtualItemsEventArgs e)
        {
            base.OnCacheVirtualItems(e);

            //We've gotten a request to refresh the cache.
            //First check if it's really neccesary.
            if (_cachedItems != null &&
                e.StartIndex >= _firstCachedItemIdx &&
                e.EndIndex <= _firstCachedItemIdx + _cachedItems.Count)
            {
                //If the newly requested cache is a subset of the old cache, 
                //no need to rebuild everything, so do nothing.
                return;
            }

            //Now we need to rebuild the cache.
            _firstCachedItemIdx = e.StartIndex;

            //indexes are inclusive
            int length = e.EndIndex - e.StartIndex + 1;
            if (_cachedItems == null)
                _cachedItems = new List<ListViewItem>(length);
            else
            {
                ClearCachedItems();
            }

            //Fill the cache with the appropriate ListViewItems.
            for (int i = 0; i < length; i++)
            {
                ListViewItem cachedItem = 
                    (_itemController != null ? _itemController.Create(i+_firstCachedItemIdx) : null);
                _cachedItems.Add(cachedItem);
            }
        }

        //This event handler enables search functionality, and is called
        //for every search request when in Virtual mode.
        protected override void OnSearchForVirtualItem(SearchForVirtualItemEventArgs e)
        {
            base.OnSearchForVirtualItem(e);

            //We've gotten a search request.
            if (_itemController != null)
                e.Index = _itemController.Search(e);

            //If e.Index is not set, the search returns null.
            //Note that this only handles simple searches over the entire
            //list, ignoring any other settings.  Handling Direction, StartIndex,
            //and the other properties of SearchForVirtualItemEventArgs is up
            //to this handler.
        }
        #endregion Overrides

        #region Methods
        public void ClearCachedItems()
        {
            if (_cachedItems != null)
                _cachedItems.Clear();

            if (_itemController != null)
                _itemController.ClearImageLists();

            GC.Collect();
        }
        #endregion Methods
    }
}
