namespace MaterialDesignSkin.Source
{
    /// <summary>
    /// Static skin helper.
    /// </summary>
    public static class SettingsHelper
    {
        #region PROPERTIES
        
        /// <summary>
        /// Indicates if user points balance should be hidden.
        /// </summary>
        public static bool HidePoints
        {
            get; set;
        }

        /// <summary>
        /// Indicates if user balance should be hidden.
        /// </summary>
        public static bool HideBalance
        {
            get; set;
        }

        /// <summary>
        /// Indicates if active applications should be hidden.
        /// </summary>
        public static bool HideActiveApplications
        {
            get; set;
        }

        /// <summary>
        /// Indicates if application rating should be hidden.
        /// </summary>
        public static bool HideAppRating
        {
            get; set;
        }

        /// <summary>
        /// Indicates if application filters should be hidden.
        /// </summary>
        public static bool HideAppFilters
        {
            get; set;
        }

        /// <summary>
        /// Indicates if application rating filter should be hidden.
        /// </summary>
        public static bool HideAppRatingFilter
        {
            get
            {
                return HideAppRating || HideAppFilters;
            }
        }

        /// <summary>
        /// Indicates if application info should be hidden.
        /// </summary>
        public static bool HideAppInfo
        {
            get; set;
        }

        /// <summary>
        /// Indicates if user login component should be hidden.
        /// </summary>
        public static bool HideUserLoginComponent
        {
            get;set;
        }

        /// <summary>
        /// Indicates if input language should be hidden.
        /// </summary>
        public static bool HideInputLanguage
        {
            get;set;
        }

        /// <summary>
        /// Indicates if display language should be hidden.
        /// </summary>
        public static bool HideDisplayLanguage
        {
            get;set;
        }

        #endregion
    }
}
