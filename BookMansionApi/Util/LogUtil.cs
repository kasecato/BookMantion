using System;
using System.Diagnostics.Tracing;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace BookMansionApi.Util
{
    public sealed class LogUtil
    {
        #region > Field

        private const string m_Format = "{0:yyyy-MM-dd HH\\:mm\\:ss\\:ffff}\tType: {1}\tMessage: '{2}'";

        private static LogUtil m_Logging = new LogUtil();

        private StorageFile m_StorageFile = null;

        private SemaphoreSlim m_SemaphoreSlim = new SemaphoreSlim(1);

        #endregion

        #region > Property
        
        public static LogUtil Logging { get { return m_Logging; } private set { m_Logging = value; } }

        #endregion

        #region > Constructor

        private LogUtil()
        {
            string filename = string.Format("BookMansionApi_{0}.log", DateTime.Now.ToString("yyyyMMdd"));
            AssignLocalFile(filename);
            LogUtil.Logging.Info("Log Filename: " + m_StorageFile.Path);
        }

        #endregion

        #region > Public Method

        public void Debug(string message)
        {
            this.WriteEvent(EventLevel.Verbose, message);
        }

        public void Info(string message)
        {
            this.WriteEvent(EventLevel.Informational, message);
        }

        public void Warn(string message)
        {
            this.WriteEvent(EventLevel.Warning, message);
        }

        public void Error(string message)
        {
            this.WriteEvent(EventLevel.Error, message);
        }

        public void Critical(string message)
        {
            this.WriteEvent(EventLevel.Critical, message);
        }

        #endregion

        #region > Private Method

        private async void AssignLocalFile(string filename)
        {
            m_StorageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename,
                                                                                      CreationCollisionOption.OpenIfExists);
        }

        private void WriteEvent(EventLevel level, string message)
        {
            if (m_StorageFile == null) return;

            string newFormatedLine = string.Format(m_Format, DateTime.Now, level, message) + Environment.NewLine;

            System.Diagnostics.Debug.WriteLine(newFormatedLine);

            WriteToFile(newFormatedLine);
        }

        private async void WriteToFile(string line)
        {
            await m_SemaphoreSlim.WaitAsync();

            await Task.Run(async () =>
            {
                try
                {
                    await FileIO.AppendTextAsync(m_StorageFile, line);
                }
                catch (Exception)
                {
                }
                finally
                {
                    m_SemaphoreSlim.Release();
                }
            });
        }

        #endregion
    }
}
