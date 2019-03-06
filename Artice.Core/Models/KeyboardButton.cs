namespace Artice.Core.Models
{
    public class KeyboardButton
    {
        public int ColumnOrder { get; set; }

        public int RowOrder { get; set; }

        public string ButtonText { get; set; }

        public string Url { get; set; }

        public string CallbackData { get; set; }
    }
}