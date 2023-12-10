namespace Resto.Data
{
    public partial class OnlineLicensingStatus
    {
        public static OnlineLicensingStatus ParseCode(int code)
        {
            foreach (OnlineLicensingStatus status in VALUES)
            {
                if (status.Code == code)
                {
                    return status;
                }

            }
            return UNKNOWN;
        }
    }
}
