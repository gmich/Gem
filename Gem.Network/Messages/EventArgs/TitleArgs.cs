namespace  Gem.Network.Args
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TitleArgs : EventArgs
    {

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerArgs"/> class.
        /// </summary>

        public TitleArgs(string title, bool show)
        {
            this.Title = title;
            this.Show = show;
        }

        #endregion

        #region Public Properties

        public string Title { get; private set; }

        public bool Show { get; private set; }
                
        #endregion

    }
}