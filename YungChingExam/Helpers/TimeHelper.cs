namespace YungChingExam.Helpers
{
    public static class TimeHelper
    {
        /// <summary>
        /// Get Server Current UTC DateTime
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}
