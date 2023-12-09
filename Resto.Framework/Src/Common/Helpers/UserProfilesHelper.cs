using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Resto.Framework.Attributes.JetBrains;
using log4net;
using Microsoft.Win32;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Хэлпер, для работы с профилями пользователей
    /// </summary>
    [ComVisible(false)]
    public static class UserProfilesHelper
    {
        #region

        private static readonly LogWrapper logWrapper = new LogWrapper(typeof(UserProfilesHelper));
        private static ILog Log
        {
            get { return logWrapper.Log; }
        }


        #endregion

        #region Consts

        // путь к профилям пользователей, зарегистрированных на компьютере (от HKLM)
        private const string ALL_PROFILES_PATH = @"Software\Microsoft\Windows NT\CurrentVersion\ProfileList";

        // название ключа с путем в папка профиля пользователя
        private const string PROFILE_IMAGE_KEY = "ProfileImagePath";

        #endregion

        #region Methods

        /// <summary>
        /// Получить SIDы и пути к локальным профилям пользователей, зарегистрированных на компьютере
        /// </summary>
        /// <returns>Коллекция SID, Путь к профилю пользователя</returns>
        public static Dictionary<string, string> GetAllSidsWithLocalProfile()
        {
            Log.Debug("Start getting all sids with local profiles..");
            var result = new Dictionary<string, string>();
            using (var profilesKey = Registry.LocalMachine.OpenSubKey(ALL_PROFILES_PATH, false))
            {
                if (profilesKey == null) 
                    return result;
                var profileNames = profilesKey.GetSubKeyNames();
                foreach (var profileName in profileNames)
                {
                    using (var profileKey = profilesKey.OpenSubKey(profileName, false))
                    {
                        if (profileKey == null) 
                            continue;
                        var val = (string)profileKey.GetValue(PROFILE_IMAGE_KEY, String.Empty);
                        if (!String.IsNullOrEmpty(val) && (Directory.Exists(val)))
                        {
                            Log.DebugFormat("Adding {0} : {1}", profileName, val);
                            result.Add(profileName, val);
                        }
                    }
                }
            }
            return result;
        }
       
        /// <summary>
        /// Найти первую подпапку с заданным именем
        /// </summary>
        /// <param name="rootFolder">Родительская папка</param>
        /// <param name="subFolderName">Название искомой подпапки</param>
        /// <returns>Полный путь к подпапке/String.Empty если не найдена</returns>
        private static string FindSubFolder(DirectoryInfo rootFolder, string subFolderName)
        {
            var result = string.Empty;
            // условие выхода из рекурсии - совпадение названия
            if (string.Compare(rootFolder.Name, subFolderName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return rootFolder.FullName;
            }
            try
            {
                // ищем среди подпапок
                foreach (var info in rootFolder.GetDirectories())
                {
                    result = FindSubFolder(info, subFolderName);
                    // если нашли - сразу выходим
                    if (!string.IsNullOrEmpty(result))
                    {
                        return result;
                    }
                }
            }
                // на некоторые папки может не быть доступа - пропускаем
            catch (UnauthorizedAccessException uae)
            {
                Log.DebugFormat("Access denied to directory '{0}': {1}", rootFolder.FullName, uae.Message);
            }
                // некоторые папки могут быть уже удалены - пропускаем
            catch (DirectoryNotFoundException)
            {
                Log.DebugFormat("Directory '{0}' not found", rootFolder.FullName);
            }
                // длина строки пути может быть больше максимальной длины - пропускаем
            catch (PathTooLongException ptle)
            {
                Log.DebugFormat("Path too long. '{0}' has {1} characters. {2}", rootFolder.FullName, rootFolder.FullName.Length, ptle.Message);
            }
            return result;
        }

        /// <summary>
        /// Получить путь к папке с данными приложения для профиля пользователя
        /// </summary>
        /// <param name="sidProfilePath">Путь к профилю</param>
        /// <param name="appHomeFolderName">Название корневой папки данных приложения</param>
        /// <returns>Путь к папке с данными приложения</returns>
        public static string GetAppHomeFolderPathForSidProfile(
            string sidProfilePath, string appHomeFolderName)
        {
            return string.IsNullOrEmpty(sidProfilePath)
                ? String.Empty
                : FindSubFolder(new DirectoryInfo(sidProfilePath), appHomeFolderName);
        }

        /// <summary>
        /// Получить все SIDы зарегистированных пользователей и имеющих в профиле папку с данными приложения
        /// </summary>
        /// <param name="sidsWithProfile">Коллекция SID, Путь к профилю пользователя</param>
        /// <param name="appHomeFolderName">Название корневой папки данных приложения</param>
        /// <returns>Коллекция SID, Путь к папке с данными приложения</returns>
        public static Dictionary<string, string> GetAllSidsWithAppHomeFolder(
            Dictionary<string, string> sidsWithProfile, string appHomeFolderName)
        {
            Log.Debug("Start getting all sids with app home folder..");
            var result = new Dictionary<string, string>();
            foreach (var sidProfile in sidsWithProfile)
            {
                Log.DebugFormat("Try get app folder for {0}:{1}", sidProfile.Key ?? "[NULL]", sidProfile.Value ?? "[NULL]");
                var appHomeFolder = GetAppHomeFolderPathForSidProfile(sidProfile.Value, appHomeFolderName);
                if (!String.IsNullOrEmpty(appHomeFolder))
                {
                    Log.DebugFormat("Adding {0} : {1}", sidProfile.Key, appHomeFolder);
                    result.Add(sidProfile.Key, appHomeFolder);
                }
            }
            return result;
        }

        /// <summary>
        /// Получить всех зарегистрированных пользователей
        /// </summary>
        /// <returns>Коллекция SID, Имя пользователя</returns>
        public static Dictionary<string, string> GetAllRegisteredUsers()
        {
            var result = new Dictionary<string, string>();
            var query = new SelectQuery("select SID, Name from Win32_UserAccount");
            using (var searcher = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject envVar in searcher.Get())
                {
                    result.Add(envVar["SID"].ToString(), envVar["Name"].ToString());
                }
            }
            return result;
        }

        /// <summary>
        /// Получить имя пользователя по его SID (Security Identifier)
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        [CanBeNull]
        public static string GetUserNameBySid(string sid)
        {
            var si = new SecurityIdentifier(sid);
            try
            {
                var ntAccount = si.Translate(typeof(NTAccount));
                return ntAccount.ToString();
            }
            catch (IdentityNotMappedException)
            {
                return null;
            }
        }

        public static string GetAppHomeFolderForSid(string sid, string homeFolderName)
        {
            const string DEFAULT_USER_PROFILE = "DefaultUserProfile";
            const string PROFILES_DIRECTORY = "ProfilesDirectory";

           // ищем профиль среди всех зарегистрированных
            Log.Debug("Search profile from in registered users..");
            var allSids = GetAllSidsWithLocalProfile();
            var sidsWithAppHomeFolder = GetAllSidsWithAppHomeFolder(allSids, homeFolderName);
            var result = sidsWithAppHomeFolder
                .Where(kvp => kvp.Key.ToUpper() == sid.ToUpper())
                .Select(kvp => kvp.Value)
                .FirstOrDefault();
            // ищем профиль у Default User
            if (!string.IsNullOrEmpty(result)) return result;
            Log.Debug("Not found. Search in default user profile");
            using (var profilesKey = Registry.LocalMachine.OpenSubKey(ALL_PROFILES_PATH, false))
            {
                if (profilesKey != null)
                {
                    var profilesDirectory = (string) profilesKey.GetValue(PROFILES_DIRECTORY, string.Empty);
                    var defaultUserProfile = (string) profilesKey.GetValue(DEFAULT_USER_PROFILE, string.Empty);
                    result = Path.Combine(profilesDirectory, defaultUserProfile);
                    result = GetAppHomeFolderPathForSidProfile(result, homeFolderName);
                }
            }
            return result;
        }

        #endregion

    }
}