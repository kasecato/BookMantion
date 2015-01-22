using BookMansion.Control;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Search;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using BMAN = BookMansionApi;

namespace BookMansion
{
    public sealed partial class BookMansion : Page
    {
        #region > Field

        private SearchHistory SearchHistory = new SearchHistory();        

        #endregion

        #region > Constructor

        public BookMansion()
        {
            this.InitializeComponent();
        }

        #endregion

        #region > Back/Forward Button History Control

        private async void AppBarButton_Back_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchHistory.GetQueryBack();
            await RestoreSearchHistory(query);
        }

        private async void AppBarButton_Forward_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchHistory.GetQueryForward();
            await RestoreSearchHistory(query);
        }

        private void AddSearchHistory(string query)
        {
            SearchHistory.Add(query);
            this.AppBarButton_Back.IsEnabled = SearchHistory.CanGoBack;
            this.AppBarButton_Forward.IsEnabled = SearchHistory.CanGoForward;
        }

        private async Task RestoreSearchHistory(string history)
        {
            if (history == null)
            {
                return;
            }

            this.AppBarButton_Back.IsEnabled = SearchHistory.CanGoBack;
            this.AppBarButton_Forward.IsEnabled = SearchHistory.CanGoForward;

            this.SearchBox.QueryText = history;
            await Search(history, false);
        }

        private async void AppBarButton_Back_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            var history = SearchHistory.GetQueryBackAll();
            await PopupHistory(history);
        }

        private async void AppBarButton_Forward_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            var history = SearchHistory.GetQueryForwardAll();
            await PopupHistory(history);
        }

        #endregion

        #region > Search Button

        private async void SearchBox_SuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            /**********************************************************************
             * Guard
             **********************************************************************/
            if (String.IsNullOrWhiteSpace(args.QueryText))
            {
                return;
            }

            /**********************************************************************
             * IME (Imputting)
             **********************************************************************/
            SearchSuggestionsRequestDeferral deferral = args.Request.GetDeferral();
            foreach (string linguistic in args.LinguisticDetails.QueryTextAlternatives)
            {
                var suggestionsime = await BMAN.Util.GoogleSuggestUtil.GetSuggestAsync(linguistic);
                args.Request.SearchSuggestionCollection.AppendQuerySuggestions(suggestionsime);
                deferral.Complete();
                return;
            }

            /**********************************************************************
             * IME (Imputed)
             **********************************************************************/
            var suggestions = await BMAN.Util.GoogleSuggestUtil.GetSuggestAsync(args.QueryText);
            args.Request.SearchSuggestionCollection.AppendQuerySuggestions(suggestions);
            deferral.Complete();
        }

        private async void SearchBox_QuerySubmitted(Windows.UI.Xaml.Controls.SearchBox sender, Windows.UI.Xaml.Controls.SearchBoxQuerySubmittedEventArgs args)
        {
            await Search(args.QueryText);
        }

        #endregion

        #region > Settings Button

        private void SettingsButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Windows.UI.ApplicationSettings.SettingsPane.Show();
        }

        #endregion

        #region > Grid Book

        private async void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool isGridView = sender is GridView;
            if (isGridView)
            {
                var gridview = sender as GridView;
                var item = gridview.SelectedItem;
                bool hasBook = item is BMAN.Model.Search.BookMansionCompletion;
                if (hasBook)
                {
                    var book = item as BMAN.Model.Search.BookMansionCompletion;
                    await ShowPopupMenu(book.Title, book.Url);
                }
            }
        }

        #endregion

        #region > ToggleSwitch Book Sort

        private async void ToggleSwitch_Sort_Toggled(object sender, RoutedEventArgs e)
        {
            await SortBooks();
        }

        #endregion

        #region > Private Method

        #region > Search

        private async Task Search(string query, bool canAddHistory = true)
        {
            /**********************************************************************
             * Start Progress Ring
             **********************************************************************/
            this.ProgressRing.IsActive = true;

            /**********************************************************************
             * Search
             **********************************************************************/
            var books = await BMAN.BuildBookMansion.SearchAsync(query);
            this.DataContext = books;

            bool noBook = books.Count == 0;
            if (noBook)
            {
                await new MessageDialog("電子書籍が見つかりませんでした。").ShowAsync();
            }
            else
            {
                await SortBooks();
            }

            /**********************************************************************
             * Add Search History
             **********************************************************************/
            if (canAddHistory)
            {
                AddSearchHistory(query);
            }

            /**********************************************************************
             * Stop Progress Ring
             **********************************************************************/
            this.ProgressRing.IsActive = false;
        }

        #endregion

        #region > PopupMenu

        private async Task ShowPopupMenu(string query, object tag)
        {
            PopupMenu menu = new PopupMenu();
            menu.Commands.Add(new UICommand("絞り込む", async (command) =>
            {
                this.SearchBox.QueryText = query;
                await Search(query);
            }));
            menu.Commands.Add(new UICommand("購入", (command) =>
            {
            }));
            menu.Commands.Add(new UICommand("Webブラウザで開く", async (command) =>
            {
                await OpenWebBrowser(tag as Uri);
            }));
            menu.Commands.Add(new UICommand("キャンセル", (command) =>
            {
            }));

            var point = Window.Current.CoreWindow.PointerPosition;
            var rect = new Rect(point, new Size(0, 0));
            var chosenCommand = await menu.ShowForSelectionAsync(rect);
            if (chosenCommand == null)
            {
            }
        }

        #endregion

        #region > PopupHistory

        private async Task PopupHistory(List<string> history)
        {
            PopupMenu menu = new PopupMenu();
            for (int i = 0; i < history.Count() && i < 6; i++)
            {
                string query = history.ElementAt(i);
                menu.Commands.Add(new UICommand(query, async (command) =>
                {
                    SearchHistory.CurrentIndex = i;
                    this.AppBarButton_Back.IsEnabled = SearchHistory.CanGoBack;
                    this.AppBarButton_Forward.IsEnabled = SearchHistory.CanGoForward;
                    this.SearchBox.QueryText = query;
                    await Search(query, false);
                }));
            }
            var point = Window.Current.CoreWindow.PointerPosition;
            var rect = new Rect(point, new Size(0, 0));
            await menu.ShowForSelectionAsync(rect);
        }

        #endregion

        #region > Open Web Browser

        private async Task OpenWebBrowser(Uri url)
        {
            LauncherOptions options = new LauncherOptions()
            {
                TreatAsUntrusted = false,
                DesiredRemainingView = Windows.UI.ViewManagement.ViewSizePreference.UseMinimum,
                DisplayApplicationPicker = false,
            };
            bool success = await Launcher.LaunchUriAsync(url, options);
            if (success)
            {

            }
            else
            {

            }
        }

        #endregion

        #region > Sort

        private async Task SortBooks()
        {
            if (this.GridView_book == null || this.GridView_book.Items == null)
            {
                return;
            }

            var books = this.GridView_book.Items.ToList().OfType<BMAN.Model.Search.BookMansionCompletion>();
            bool isSortPrice = ToggleSwitch_Sort.IsOn;
            if (isSortPrice)
            {
                Func<IOrderedEnumerable<BMAN.Model.Search.BookMansionCompletion>> asyncJob = () =>
                {
                    return books.OrderBy(x => x.Price);
                };
                this.DataContext = await Task.Run(asyncJob);
            }
            else
            {
                Func<IOrderedEnumerable<BMAN.Model.Search.BookMansionCompletion>> asyncJob = () =>
                {
                    return books.OrderBy(x => x.SimilarityRank);
                };
                this.DataContext = await Task.Run(asyncJob);
            }
        }

        #endregion

        #endregion
    }
}
