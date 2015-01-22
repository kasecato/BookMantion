using Windows.Networking.Connectivity;

namespace BookMansionApi.Util
{
    public sealed class NetworkUtil
    {
        #region > Public Method

        public static bool IsNetworkAvailable()
        {
            ConnectionProfile InternetConnectionProfile =NetworkInformation.GetInternetConnectionProfile();
            
            if (InternetConnectionProfile == null)
            {
                return false;
            }

            NetworkConnectivityLevel level = InternetConnectionProfile.GetNetworkConnectivityLevel();
            return level == NetworkConnectivityLevel.InternetAccess;
        }

        #endregion
    }
}
