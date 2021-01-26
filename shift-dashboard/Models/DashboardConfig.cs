namespace shift_dashboard.Model
{
    public class DashboardConfig
    {
        /// <summary>
        /// Position of the config in appsetting.json
        /// </summary>
        public string Position = "DashboardConfig";

        /// <summary>
        /// Application Name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Database Connection String
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// API Url to retreive info
        /// </summary>
        public string APIUrl { get; set; }
    }
}