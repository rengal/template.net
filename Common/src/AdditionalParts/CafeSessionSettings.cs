namespace Resto.Data
{
    public partial class CafeSessionSettings
    {
        public bool ConfirmEmployeeAttendancesOnSessionClosing
        {
            get { return PersonalSessionsVerificationSetup == CafeSessionClosePersonalSessionsVerificationSetup.EMPLOYEE_ATTENDANCES; }
        }
    }
}
