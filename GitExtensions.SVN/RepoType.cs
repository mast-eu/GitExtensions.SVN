namespace GitExtensions.SVN
{
    public enum RepoType
    {
        /// <summary>
        /// Has a SVN remote.
        /// </summary>
        SVN,

        /// <summary>
        /// Does not have a SVN remote.
        /// </summary>
        git,

        /// <summary>
        /// Not yet checked.
        /// </summary>
        Unknown
    };
}
