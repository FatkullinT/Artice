namespace Artice.Telegram.Models.Enums
{
    /// <summary>
    /// Type of a <see cref="FileToSend"/>
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// Unknown FileType
        /// </summary>
        Unknown,

        /// <summary>
        /// FileStream
        /// </summary>
        Stream,

        /// <summary>
        /// FileId
        /// </summary>
        Id,

        /// <summary>
        /// FileReference Url
        /// </summary>
        Url
    }
}
