using System;

namespace Suntek.Models
{
    public class StoreViewModel : Product
    {
        public Nullable<DateTime> NgayNK { get; set; }
        public Nullable<DateTime> NgayKH { get; set; }
        //public string Name { get; set; }
        //public string Serial { get; set; }
        //public string Code { get; set; }
        //public int? Limited { get; set; }
    }
}