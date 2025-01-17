using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public static class Messages
    {
        public static string AkademikPersonelAdded = "Akademik Personel Eklendi.";
        public static string AkademikPersonelUpdated = "Akademik Personel Güncellendi.";
        public static string AkademikPersonelDeleted = "Akademik Personel Silindi.";

        public static string BolumAdded = "Bölüm Eklendi.";
        public static string BolumDeleted = "Bölüm Silindi.";
        public static string BolumUpdated = "Bölüm Güncellendi";

        public static string DersAdded = "Ders Eklendi.";
        public static string DersDeleted = "Ders Silindi.";
        public static string DersUpdated = "Ders Güncellendi.";

        public static string DBAPAdded = "Ders Bölüm Akademik Personel Eşleştirmesi yapıldı";
        public static string DBAPDeleted = "Ders Bölüm Akademik Personel Eşleştirmesi silindi!!";
        public static string DBAPUpdated = "Ders Bölüm Akademik Personel Eşleştirmesi güncellendi";

        public static string DerslikAdded = "Derslik Eklendi.";
        public static string DerslikDeleted = "Derslik Silindi.";
        public static string DerslikUpdated = "Derslik Güncellendi.";


        public static string SomethingWrong = "Bir şeyler yanlış gidiyor. @?!!#";

        public static string SinavDetayAdded { get; internal set; }
        public static string SinavDetayDeleted { get; internal set; }
        public static string SinavDetayUpdated { get; internal set; }
    }
}
