namespace BookMansionApi.Exceptions
{
    public enum BookManExceptionType
    {
        #region > HttpUtil

        Dns = -400,
        Timeout = -410,
        Internet = -415,
        Protocol = -420,

        #endregion
    }
}
