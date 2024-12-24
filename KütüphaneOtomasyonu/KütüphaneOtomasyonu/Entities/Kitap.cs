using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace KütüphaneOtomasyonu.Entities
{
    //Kitapların özelliklerini tanımladığımız kitap sınıfı
    public class Kitap
    {
        //Veritabanı kısmında ID lerin otomatik şekkilde gitmesini sağlayan kod bölümü
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public String KitapId { get; set; }
        public String KitapAdı { get; set; }
        public String KitapYazar { get; set; }
        public String KitapYayınevi { get; set; }
        public int KitapSayfaSayısı { get; set; }
        public int KitapBaskıSayısı { get; set; }
        public String KitapDil {  get; set; }
    }
}
