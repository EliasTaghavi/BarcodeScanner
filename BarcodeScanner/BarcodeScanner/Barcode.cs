using System;
using SQLite;

namespace BarcodeScanner
{
    public class Barcode
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public DateTime Time { get; set; }
    }
}
