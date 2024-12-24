using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace KütüphaneOtomasyonu.Entities
{
    //Kullanıcıların bilgilerinin tutulduğu Kullanıcı sınıfı
    public class Kullanıcı
    {
        //Veritabanı kısmında ID lerin otomatik şekkilde gitmesini sağlayan kod bölümü
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string kulllanıcıID;
        public String KulllanıcıID { get; set; }
        public string KullanıcıAdı { get; set; }
        public string KullanıcıSoyad { get; set; }
        public string Email { get; set; }
        public string Şifre { get; set; }
        public long Tc { get; set; }
        public DateTime DoğumTarihi { get; set; }
        public string TelNo { get; set; }
        public string ÖğrenimTürü { get; set; }
        public string KullanıcıFoto { get; set; }
    }
}
