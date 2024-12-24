using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KütüphaneOtomasyonu.Entities;
using KütüphaneOtomasyonu.Bağlantı;
using MongoDB.Bson;
using MongoDB.Driver;

namespace KütüphaneOtomasyonu.Bağlantı
{
    //Uygulamayı kullanan kullanıcıların yapacağı işlemleri tanımlayan sınıf
    public class KullanıcıIslemleri
    {
        //Kullanıcı kaydı için kullanıcıEkle metodu oluşturuyoruz
        public void KullanıcıEkle(Kullanıcı kullanıcı)
        {
            //MongoDB bağlantı cümlesi 
            var connection = new MongoBağlantı();
            var KullanıcıKoleksiyonu = connection.GetKullanıcıKoleksiyonu();
            //Kullanıcıların kayıt olurken girdiği bilgiler bson türünde veritabanına gönderilir. InsertOne kullanıcı koleksiyonuna kullanıcı ekler
            BsonDocument document = new BsonDocument
            {
                {"KullanıcıAdı",kullanıcı.KullanıcıAdı},
                {"KullanıcıSoyad",kullanıcı.KullanıcıSoyad },
                {"Email",kullanıcı.Email },
                {"Şifre",kullanıcı.Şifre },
                {"Tc",kullanıcı.Tc },
                {"DoğumTarihi",kullanıcı.DoğumTarihi },
                {"TelNo",kullanıcı.TelNo },
                {"ÖğrenimTürü",kullanıcı.ÖğrenimTürü },
                {"KullanıcıFoto",kullanıcı.KullanıcıFoto }
            };
            KullanıcıKoleksiyonu.InsertOne(document);

        }
        //Kullanıcı giriş kontrolü. Kullanıcılar sisteme Tc ve şifreleri ile giriş yaparlar. Burada tc ve şifre filtrelenir
        public bool KullaniciGirişi(long Tc, string Şifre)
        {
            //MongoDB bağlantı cümlesi 
            var connection = new MongoBağlantı();
            var KullanıcıKoleksiyonu = connection.GetKullanıcıKoleksiyonu();
            //Filter filtreleme işlemini gerçekleştirir Eq ise veritabaındaki tc ile uygulama sistemindeki tc eşit mi bunu kontrol eder.
            var filtre = Builders<BsonDocument>.Filter.Eq("Tc", Tc) & Builders<BsonDocument>.Filter.Eq("Şifre", Şifre);
            var Kullanıcılar = KullanıcıKoleksiyonu.Find(filtre).FirstOrDefault();

            return Kullanıcılar != null;
        }
        //Kullanıcı Bilgilerini kulllanıcıya göstermek için kullanıcıBilgileriGetir metodu.
        public Kullanıcı kullanıcıBilgileriGetir(long tc)
        {
            //MongoDB bağlantı cümlesi 
            var connection = new MongoBağlantı();
            var kullanıcıKoleksiyonu = connection.GetKullanıcıKoleksiyonu();
            //Her kullanıcı TC si farklı olacağından dolayı Tc ye göre filtre yaparak diğer bilgileri getirir
            var filtre = Builders<BsonDocument>.Filter.Eq("Tc", tc);
            var sonuc = kullanıcıKoleksiyonu.Find(filtre).FirstOrDefault();
            //Eğer uyuşan TC var ise kullanıcı nesnesi oluşturarak diğer bilgileri getirir
            if (sonuc != null)
            {
                return new Kullanıcı
                {
                    KulllanıcıID = sonuc["_id"].ToString(),
                    KullanıcıAdı = sonuc["KullanıcıAdı"].AsString,
                    KullanıcıSoyad = sonuc["KullanıcıSoyad"].AsString,
                    Email = sonuc["Email"].AsString,
                    Şifre = sonuc["Şifre"].AsString,
                    Tc = sonuc["Tc"].AsInt64,
                    DoğumTarihi = sonuc["DoğumTarihi"].ToUniversalTime(),
                    TelNo = sonuc["TelNo"].AsString,
                    ÖğrenimTürü = sonuc["ÖğrenimTürü"].AsString,
                    KullanıcıFoto = sonuc["KullanıcıFoto"].AsString
                };
            }

            return null;
        }
        //Kullanıcı yanlış girdiği bir bilgiyi düzeltmek isterse KullanıcıGüncelle metodu ile güncelleyecek.
        //Kullanıcı id lerine göre bilgilerini güncelleyebilir
        public void KullanıcıGüncelle(Kullanıcı kullanıcı)
        {
            //MongoDB bağlantı cümlesi 
            var connection = new MongoBağlantı();
            var KullanıcıKoleksiyon = connection.GetKullanıcıKoleksiyonu();
            //Veritabanındaki id ile sistemdeki id karşılaştırır. Eğer uyuşma varsa bilgileri set ile değiştirir.
            //UpdateOne MongoDB veri güncelleme anahtar kelimesidir.
            var filtre = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(kullanıcı.KulllanıcıID));
            var güncelle = Builders<BsonDocument>.Update
                .Set("KullanıcıAdı", kullanıcı.KullanıcıAdı)
                .Set("KullanıcıSoyad", kullanıcı.KullanıcıSoyad)
                .Set("Şifre", kullanıcı.Şifre)
                .Set("Tc", kullanıcı.Tc)
                .Set("Email", kullanıcı.Email)
                .Set("DoğumTarihi", kullanıcı.DoğumTarihi)
                .Set("TelNo", kullanıcı.TelNo)
                .Set("ÖğrenimTürü", kullanıcı.ÖğrenimTürü)
                .Set("KullanıcıFoto", kullanıcı.KullanıcıFoto);
            KullanıcıKoleksiyon.UpdateOne(filtre, güncelle);
        }
        //Kullanıcı giriş yaparken idsine göre fotoğrafını her formda sol üst köşeye getirir.
        public Kullanıcı kullanıcıFotoGetirr(long tc)
        {
            //MongoDB bağlantı cümlesi 
            var connection = new MongoBağlantı();
            var kullanıcıKoleksiyonu = connection.GetKullanıcıKoleksiyonu();
            //Tc ye göre filtreleme yapılır çünkü her kullanıcının tc si farklıdır.
            var filtre = Builders<BsonDocument>.Filter.Eq("Tc", tc);
            var sonuc = kullanıcıKoleksiyonu.Find(filtre).FirstOrDefault();

            if (sonuc != null)
            {
                return new Kullanıcı
                {
                    KullanıcıFoto = sonuc["KullanıcıFoto"].AsString
                };
            }

            return null;
        }
    }
}
