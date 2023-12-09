using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Resto.Framework.Windows
{
    public static partial class WinApi
    {
        public const int CredUiMaxMessageLength = 32767;
        public const int CredUiMaxCaptionLength = 128;
        public const int CredUiMaxGenericTargetLength = 32767;
        public const int CredUiMaxDomainTargetLength = 256 + 1 + 80;
        public const int CredUiMaxUserNameLength = 256 + 1 + 256;
        public const int CredUiMaxPasswordLength = 512 / 2;
        public const int CredUiBannerHeight = 60;
        public const int CredUiBannerWidth = 320;
        
        /// <summary>
        /// Specifies special behavior for this function. 
        /// This value can be a bitwise-OR combination of zero or more of the following values. 
        /// </summary>
        // For more information of these flags see:
        // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/secauthn/security/creduipromptforcredentials.asp
        // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnnetsec/html/dpapiusercredentials.asp?frame=true
        [Flags]
        public enum CredUiFlags : uint
        {
            /// <summary>
            /// Notify the user of insufficient credentials by displaying the "Logon unsuccessful" balloon tip.
            /// </summary>
            IncorrectPassword = 0x00001, //Indicates the username is valid, but password is not
            /// <summary>
            /// Do not store credentials or display check boxes. You can pass CREDUI_FLAGS_SHOW_SAVE_CHECK_BOX with this flag to display the Save check box only, and the result is returned in the pfSave output parameter.
            /// </summary>
            DoNotPersist = 0x00002, // Do not show "Save" checkbox, and do not persist credentials
            /// <summary>
            /// Populate the combo box with local administrators only.
            /// Windows XP Home Edition:  This flag will filter out the well-known Administrator account.
            /// </summary>
            RequestAdministrator = 0x00004, // Populate list box with admin accounts
            /// <summary>
            /// Populate the combo box with user name/password only. Do not display certificates or smart cards in the combo box.
            /// </summary>
            ExcludeCertificates = 0x00008, // do not include certificates in the drop list
            /// <summary>
            /// Populate the combo box with certificates and smart cards only. Do not allow a user name to be entered.
            /// </summary>
            RequireCertificate = 0x00010,
            /// <summary>
            /// If the check box is selected, show the Save check box and return TRUE in the pfSave output parameter, otherwise, return FALSE. CREDUI_FLAGS_DO_NOT_PERSIST must be specified to use this flag. Check box uses the value in pfSave by default.
            /// </summary>
            ShowSaveCheckBox = 0x00040,
            /// <summary>
            /// Specifies that a user interface will be shown even if the credentials can be returned from an existing credential in credential manager. This flag is permitted only if CREDUI_FLAGS_GENERIC_CREDENTIALS is also specified.
            /// </summary>
            AlwaysShowUi = 0x00080,
            /// <summary>
            /// Populate the combo box with certificates or smart cards only. Do not allow a user name to be entered.
            /// </summary>
            RequireSmartcard = 0x00100,
            //PasswordOnlyOk = 0x00200, // Undocumented
            //ValidateUsername = 0x00400, // Undocumented
            //CompleteUsername = 0x00800, // Undocumented
            /// <summary>
            /// Do not show the Save check box, but the credential is saved as though the box were shown and selected.
            /// </summary>
            Persist = 0x01000, // Do not show "Save" checkbox, but persist credentials anyway
            /// <summary>
            /// This flag is meaningful only in locating a matching credential to prefill the dialog box, should authentication fail. When this flag is specified, wildcard credentials will not be matched. It has no effect when writing a credential. CredUI does not create credentials that contain wildcard characters. Any found were either created explicitly by the user or created programmatically, as happens when a RAS connection is made.
            /// </summary>
            ServerCredential = 0x04000,
            /// <summary>
            /// Specifies that the caller will call CredUIConfirmCredentials after checking to determine whether the returned credentials are actually valid. This mechanism ensures that credentials that are not valid are not saved to the credential manager. Specify this flag in all cases unless CREDUI_FLAGS_DO_NOT_PERSIST is specified.
            /// </summary>
            ExpectConfirmation = 0x20000, // do not persist unless caller later confirms credential via CredUIConfirmCredential() api
            /// <summary>
            /// Consider the credentials entered by the user to be generic credentials.
            /// </summary>
            GenericCredentials = 0x40000, // Credential is a generic credential
            /// <summary>
            /// The credential is a "runas" credential. The TargetName parameter specifies the name of the command or program being run. It is used for prompting purposes only.
            /// </summary>
            UsernameTargetCredentials = 0x80000, // Credential has a username as the target
            /// <summary>
            /// Undocumented, see comments for this enum value
            /// </summary>
            KeepUserName = 0x100000, // don't allow the user to change the supplied username
        }

        public enum CredUiPromptForCredentialsReturnCodes : uint
        {
            /// <summary>
            /// User chose OK. The userName, password, and save parameters will return the values documented earlier.
            /// </summary>
            NoError = 0,
            /// <summary>
            /// Either pszTargetName is NULL, the empty string, or longer than CREDUI_MAX_DOMAIN_LENGTH, or pUiInfo is not NULL and the CredUI_INFO structure pointed to did not meet one of the following requirements:
            /// The cbSize member must be one. 
            /// If the hbmBanner member is not NULL, it must be of type OBJ_BITMAP. 
            /// If the pszMessageText member is not NULL, it must not be greater than CREDUI_MAX_MESSAGE_LENGTH. 
            /// If the pszCaptionText member is not NULL, it must not be greater than CREDUI_MAX_CAPTION_LENGTH.            
            /// </summary>
            ErrorInvalidParameter = 87,
            /// <summary>
            /// This status is returned for any of the flag configurations that are not valid.
            /// </summary>
            ErrorInvalidFlags = 1004,
            /// <summary>
            /// User chose Cancel. The userName and password parameters have not changed.
            /// </summary>
            ErrorCancelled = 1223,
            /// <summary>
            /// The credential manager cannot be used. Typically, this error is handled by calling CredUIPromptForCredentials and passing in the CREDUI_FLAGS_DO_NOT_PERSIST flag.
            /// </summary>
            ErrorNoSuchLogonSession = 1312,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct CredUiInfo
        {
            public CredUiInfo(IntPtr owner, string caption, string message, IntPtr banner)
            {
                cbSize = Marshal.SizeOf(typeof(CredUiInfo));
                hwndParent = owner;
                pszCaptionText = caption;
                pszMessageText = message;
                hbmBanner = banner;
            }

            public int cbSize;
            public IntPtr hwndParent;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszMessageText;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszCaptionText;
            public IntPtr hbmBanner;
        }

        [DllImport("Credui.dll", EntryPoint = "CredUIPromptForCredentialsW", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.U4)]
        public static extern CredUiPromptForCredentialsReturnCodes CredUIPromptForCredentials(
            ref CredUiInfo uiInfo,
            string targetName,
            IntPtr reserved,
            int authError,
            StringBuilder userName,
            int userNameMaxChars,
            StringBuilder password,
            int passwordMaxChars,
            [MarshalAs(UnmanagedType.Bool)]
            ref bool save,
            CredUiFlags flags);

        public enum CredUiParseUserNameReturnCodes : uint
        {
            /// <summary>
            /// The user name is valid.
            /// </summary>
            NoError = 0,
            /// <summary>
            /// userMaxChars or domainMaxChars is zero. userName, user, or domain is NULL.
            /// </summary>
            ErrorInvalidParameter = 87,
            /// <summary>
            /// One of the buffers is too small.
            /// </summary>
            ErrorInsufficientBuffer = 122,
            /// <summary>
            /// The user name is not valid.
            /// </summary>
            ErrorInvalidAccountName = 1315,
        }

        [DllImport("Credui.dll", EntryPoint = "CredUIParseUserNameW", CharSet = CharSet.Unicode)]
        public static extern CredUiParseUserNameReturnCodes CredUiParseUserName(
            [MarshalAs(UnmanagedType.LPTStr)]
            string userName,
            StringBuilder user,
            int userMaxChars,
            StringBuilder domain,
            int domainMaxChars);


        public enum CredUiConfirmCredentialsReturnCodes : uint
        {
            /// <summary>
            /// Confirm operation succeeded.
            /// </summary>
            NoError = 0,
            /// <summary>
            /// An attempt to confirm a waiting credential failed because the credential contained data that was not valid or was inconsistent.
            /// </summary>
            ErrorInvalidParameter = 87,
            /// <summary>
            /// The subject credential could not be found on the confirmation waiting list.
            /// </summary>
            ErrorNotFound = 1168,
        }

        [DllImport("Credui.dll", EntryPoint = "CredUIConfirmCredentialsW", CharSet = CharSet.Unicode)]
        public static extern CredUiConfirmCredentialsReturnCodes CredUiConfirmCredentials(
            [MarshalAs(UnmanagedType.LPWStr)]
            string targetName,
            [MarshalAs(UnmanagedType.Bool)]
            bool confirm);

    }
}
