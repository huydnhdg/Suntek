using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Suntek.Utils
{
    public class SmsTemp
    {
        //
        public static string WARRANTI()
        {
            return string.Format("Yeu cau bao hanh cua ban da duoc ghi nhan, chung toi se lien he lai trong vong 24h. Moi thac mac lien he P. BH 0937.73.29.73. Cam on quy khach");
        }

        public static string WARRANTI_NOTVALID()
        {
            return string.Format("So dien thoai Yeu cau bao hanh cua ban khong ton tai.Moi thac mac lien P.BH 0937.73.29.73.Cam on quy khach.");
        }



        //
        public static string PHONE_NOTVALID = "So dien thoai khong dung. Lien he 0908936736 hoac truy cap http://baohanh.vusonSolar.vn de duoc ho tro";
        public static string ACTIVE_NOTVALID()
        {
            return string.Format("Ma Cao SP cua ban khong ton tai. Vui long kiem tra lai Ma Cao. Lien he CSKH 0908936736 de duoc ho tro. Chi tiet truy cap http://baohanh.vusonSolar.vn");
        }
        public static string ACTIVE(string s2, string s3, string s4)
        {
            return string.Format("San pham {0} da kich hoat thanh cong tu ngay {1} den ngay {2}. LH CSKH 0908936736 duoc ho tro them. Cam on quy khach", s2, s3, s4);
        }
        public static string ACTIVATED(string s2, string s3, string s4)
        {
            return string.Format("San pham {0} da duoc kich hoat tu ngay {1} den ngay {2}, moi thac mac lien he CSKH 0908936736. Cam on quy khach", s2, s3, s4);
        }

        public static string ACTIVE_AGENT_NOTVALID = "Ma dai ly cua ban khong ton tai. Kich hoat khong thanh cong. Moi thac mac lien he CSKH 0908936736. Cam on quy khach.";
        //
        public static string TRACUU_NOTVALID()
        {
            return string.Format("Ma Cao san pham cua ban khong ton tai. Vui long kiem tra lai Ma Cao va lien he CSKH 0908936736 truy cap http://baohanh.vusonSolar.vn de duoc ho tro");
        }
        //public static string TRACUU_ACTIVATED_DATE_GREATER_NOW()
        //{
        //    return string.Format("Serial san pham cua ban khong ton tai. Vui long kiem tra lai so serial va lien he 02873038123 hoac truy cap https://baohanh.phuongbinhgroup.com.vn de duoc ho tro");
        //}
        public static string TRACUU_OUTOFDATE(string s1, string s2)
        {
            return string.Format("San pham {0} da het han bao hanh vao {1}, moi thac mac lien he CSKH 0908936736.Cam on quy khach", s1, s2);
        }
        public static string TRACUU_ACTIVE(string s1, string s2, string s3)
        {
            return string.Format("San pham {0} da duoc kich hoat tu ngay {1} den ngay {2}, moi thac mac lien he CSKH 0908936736. Cam on quy khach", s1, s2, s3);
        }
        public static string TRACUU_NOACTIVE()
        {
            return "Ma Cao san pham cua ban khong ton tai. Vui long kiem tra lai Ma Cao va lien he CSKH 0908936736. Chi tiet truy cap http://baohanh.vusonSolar.vn de duoc ho tro";
        }
        public static string WRONG_SYNTAX()
        {
            return string.Format("Loi ngoai le. Lien he 0908936736 hoac truy cap http://baohanh.vusonSolar.vn de duoc ho tro");
        }
        //
    }
}