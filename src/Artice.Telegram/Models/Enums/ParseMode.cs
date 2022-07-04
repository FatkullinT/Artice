namespace Artice.Telegram.Models.Enums
{
    /// <summary>
    /// Text parsing mode
    /// </summary>
    public enum ParseMode
    {
        /// <summary>
        /// <see cref="Message.Text"/> is plain text
        /// </summary>
        Default = 0,

        /// <summary>
        /// <see cref="Message.Text"/> is formated in Markdown
        /// </summary>
        Markdown,

        /// <summary>
        /// <see cref="Message.Text"/> is formated in HTML
        /// </summary>
        Html
    }
}
