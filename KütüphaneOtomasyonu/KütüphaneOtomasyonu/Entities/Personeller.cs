using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace KütüphaneOtomasyonu.Entities
{
    //Personellerimizin özelliklerini tuttuğumuz sınıf
    public class Personeller
    {
        //Veritabanı kısmında ID lerin otomatik şekkilde gitmesini sağlayan kod bölümü
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public String PersonelID { get; set; }
        public string PersonelAdı { get; set; }
        public string PersonelSoyad { get; set; }
        public string Email { get; set; }
        public long Tc { get; set; }
        public DateTime DoğumTarihi { get; set; }
        public string TelNo { get; set; }
        public string KullanıcıAdı { get; set; }
        public string PersonelFoto { get; set; }
        public string Şifre { get; set; }
    }
}
