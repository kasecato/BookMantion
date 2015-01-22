using System.Collections.Generic;
using System.Linq;

namespace BookMansion.Control
{
    class SearchHistory
    {
        #region > Field

        public int CurrentIndex = 0;
        public Stack<string> Query = new Stack<string>();

        #endregion

        #region > Property

        public int Count { get { return Query.Count; } }
        public bool IsTop { get { return CurrentIndex == 0; } }
        public bool CanGoBack { get { return CurrentIndex != Count - 1; } }
        public bool CanGoForward { get { return CurrentIndex != 0; } }

        #endregion

        #region > Method

        public void Add(string query)
        {
            if (!IsTop)
            {
                for (int i = CurrentIndex; i < Count; i++)
                {
                    Query.Pop();
                }
            }
            Query.Push(query);
            CurrentIndex = 0;
        }

        public string GetQueryBack()
        {
            return GoBack() ? Query.ElementAt(CurrentIndex) : null;
        }

        public string GetQueryForward()
        {
            return GoForward() ? Query.ElementAt(CurrentIndex) : null;
        }

        public List<string> GetQueryBackAll()
        {
            return Query.Skip(CurrentIndex + 1).ToList();
        }

        public List<string> GetQueryForwardAll()
        {
            return Query.Take(CurrentIndex + 1).ToList();
        }

        public bool GoBack()
        {
            return CanGoBack ? ++CurrentIndex != Count : false;
        }

        public bool GoForward()
        {
            return CanGoForward ? --CurrentIndex != -1 : false;
        }

        #endregion
    }
}
