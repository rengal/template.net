using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using log4net;
using Resto.Data;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Framework.Data;
using Resto.Framework.Localization;

namespace Resto.Common
{
    public class CachedEntitesPreloader
    {
        #region Shared Members

        private static readonly CachedEntitesPreloader instance = new CachedEntitesPreloader();
        private static readonly ILog Log = LogFactory.Instance.GetLogger(typeof(CachedEntitesPreloader));
        private static readonly object mutex = new object();

        private static readonly List<Type> requiredObjectsTypes = new List<Type> { typeof(CafeSetup) };

        #endregion Shared Members

        #region Data Members

        private bool ok;
        private bool isFinished;
        private bool isWorking;
        private ParsedEntitiesUpdate update;

        #endregion Data Members

        #region Constructors

        private CachedEntitesPreloader()
        {
        }

        #endregion Constructors

        #region Properties

        public static CachedEntitesPreloader Instance
        {
            get { return instance; }
        }

        public bool IsWorking
        {
            get { return isWorking; }
        }

        public bool IsFinished
        {
            get { return isFinished; }
        }

        public bool Ok
        {
            get { return ok; }
        }

        [CanBeNull]
        public ParsedEntitiesUpdate Update
        {
            get { return update; }
        }

        #endregion Properties

        #region Methods

        public void DoWork()
        {
            lock (mutex)
            {
                if (isFinished && EntityManager.INSTANCE.ResetForReloadIfNeed())
                {
                    isFinished = false;
                }

                if (isFinished || isWorking)
                {
                    return;
                }

                Log.Info("Loading from cache work started");
                isWorking = true;
                LoadDataDelegate del = LoadData;

                del.BeginInvoke(AsyncCallback, null);
            }
        }

        public void WaitForFinish()
        {
            Log.Info("Start waiting");

            if (!isFinished && !isWorking)
                DoWork();

            while (isWorking)
            {
                Thread.Sleep(20);
                //Application.DoEvents(); //todo debugnow
            }

            Log.Info("End waiting");
        }

        #endregion Methods

        #region Implementation

        private void AsyncCallback(IAsyncResult ar)
        {
            if (ar.IsCompleted)
            {
                Log.Info("Loading from cache work completed - " + ok);
                isFinished = true;
                isWorking = false;
            }
        }

        private void LoadData()
        {
            try
            {
                Thread.CurrentThread.CurrentUICulture =
                    Thread.CurrentThread.CurrentCulture = Localizer.Culture;

                update = EntityManager.INSTANCE.LoadEntites();

                if (update == null)
                {
                    ok = false;
                    return;
                }

                ok = !requiredObjectsTypes
                    .Select(type => type.Name)
                    .Except(update.Items.Select(item => item.Type))
                    .Any();
            }
            finally
            {
                isWorking = false;
            }
        }

        #endregion Implementation

        #region Nested type: LoadDataDelegate

        private delegate void LoadDataDelegate();

        #endregion
    }
}