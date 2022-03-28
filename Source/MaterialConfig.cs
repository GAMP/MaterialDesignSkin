using System.ComponentModel;
using System.Runtime.Serialization;

namespace MaterialDesignSkin.Source
{
    #region MaterialConfig
    /// <summary>
    /// Extended configuration file.
    /// </summary>
    [DataContract()]
    public class MaterialConfig : SharedLib.Configuration.SkinConfig
    {
        #region PROPERTIES

        /// <summary>
        /// Enable or disable shadows.
        /// </summary>
        [DataMember()]
        [DefaultValue(true)]
        public bool DisableShadows
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets if showing of points should be disabled.
        /// </summary>
        [DataMember()]
        [DefaultValue(false)]
        public bool HidePoints
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets if balance should be hidden.
        /// </summary>
        [DataMember()]
        [DefaultValue(false)]
        public bool HideBalance
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets if active applications should be hidden.
        /// </summary>
        [DataMember()]
        [DefaultValue(false)]
        public bool HideActiveApplications
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets if app rating should be hidden.
        /// </summary>
        [DataMember()]
        [DefaultValue(false)]
        public bool HideAppRating
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets if user login component should be hidden.
        /// </summary>
        [DataMember()]
        [DefaultValue(false)]
        public bool HideUserLoginComponent
        {
            get;set;
        }

        /// <summary>
        /// Gets or sets if display language selection should be hidden.
        /// </summary>
        [DataMember()]
        [DefaultValue(false)]
        public bool HideDisplayLanguage
        {
            get;set;
        }

        /// <summary>
        /// Gets or sets if input language selection should be hidden.
        /// </summary>
        [DataMember()]
        [DefaultValue(false)]
        public bool HideInputLanguage
        {
            get;set;
        }

        #endregion
    }
    #endregion
}
