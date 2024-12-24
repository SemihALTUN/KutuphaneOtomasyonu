using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KütüphaneOtomasyonu.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace KütüphaneOtomasyonu.Bağlantı
{
    //Personellerin işlemlerini gerçekleştireceğimiz PersonelIslemler sınıfı
    public class PersonelIslemleri
    {
        //Uygulamada çalışan personeller yeni işe giren personeli sisteme ekleyebilir. PersonelEkle metodu bu işe yarar.
        public void PersonelEkle(Personeller Personel)
        {
            //MongoDB veritabanına bağlantı cümlesi
            var connection = new MongoBağlantı();
            var PersonelKoleksiyonu = connection.GetPersonelKoleksiyonu();
            //Ekleme işlemi için girilen personel bilgilerini bson formatında dökğman olarak veritabanına gönderir.
            BsonDocument document = new BsonDocument
            {
                {"PersonelAdı",Personel.PersonelAdı},
                {"PersonelSoyad",Personel.PersonelSoyad },
                {"Email",Personel.Email },
                {"Tc",Personel.Tc },
                {"DoğumTarihi",Personel.DoğumTarihi },
                {"TelNo",Personel.TelNo },
                {"KullanıcıAdı",Personel.KullanıcıAdı },
                {"PersonelFoto",Personel.PersonelFoto },
                {"Şifre",Personel.Şifre }
            };
            //InserOne veritabanına ekleme yapmamamızı sağlayan anahtar kelimedir.
            PersonelKoleksiyonu.InsertOne(document);

        }
        //Personeller uygulamaya giriş yaparken kullanıcı adı ve şifrelerini kullanırlar.
        //Burada veritabanındaki kullanıcıadı ve şifre ile sistemde kayıt olan kullanıcı adı ve şifreyi karşılaştırarak uygulamaya
        //girişi sağlar burada Eg bu karşılaştırmayı gerçekleştirir
        public bool PersonelGirişi(string KullanıcıAdı, string Şifre)
        {
            //MongoDB veritabanına bağlantı cümlesi
            var connection = new MongoBağlantı();
            var PersonelKoleksiyonu = connection.GetPersonelKoleksiyonu();
            //kullanıcıadı ve şifrenin filtrelendiği kısım
            var filtre = Builders<BsonDocument>.Filter.Eq("KullanıcıAdı", KullanıcıAdı) & Builders<BsonDocument>.Filter.Eq("Şifre", Şifre);
            var Kullanıcılar = PersonelKoleksiyonu.Find(filtre).FirstOrDefault();

            return Kullanıcılar != null;
        }
        //Personeller bilgilerini getiren metod.
        public Personeller PersonelBilgileriGetir(String KullanıcıAdı)
        {
            //MongoDB veritabanına bağlantı cümlesi
            var connection = new MongoBağlantı();
            var PersonelKoleksiyonu = connection.GetPersonelKoleksiyonu();
            //Filtrelemeyi kullanıcı adına göre yapar diğer blilgileride kullanıcıadına bakarak döndürür
            var filtre = Builders<BsonDocument>.Filter.Eq("KullanıcıAdı", KullanıcıAdı);
            var sonuc = PersonelKoleksiyonu.Find(filtre).FirstOrDefault();
            //Kullanıcı adı veritabanında var ise diğer bilgileri döndürür yoksa metod boş döner.
            if (sonuc != null)
            {
                return new Personeller
                {
                    PersonelID = sonuc["_id"].ToString(),
                    PersonelAdı = sonuc["PersonelAdı"].AsString,
                    PersonelSoyad = sonuc["PersonelSoyad"].AsString,
                    Email = sonuc["Email"].AsString,
                    Şifre = sonuc["Şifre"].AsString,
                    Tc = sonuc["Tc"].AsInt64,
                    DoğumTarihi = sonuc["DoğumTarihi"].ToUniversalTime(),
                    TelNo = sonuc["TelNo"].AsString,
                    KullanıcıAdı = sonuc["KullanıcıAdı"].AsString,
                    PersonelFoto = sonuc["PersonelFoto"].AsString
                };
            }

            return null;
        }
        //Personeller bilgilerini değiştirmek isterse PersonelGüncelle metodu çalışır.
        public void PersonelGüncelle(Personeller Personel)
        {
            //MongoDB veritabanına bağlantı cümlesi
            var connection = new MongoBağlantı();
            var PersonelKoleksiyon = connection.GetPersonelKoleksiyonu();
            //Personeller bilgilerini id göre filtreleyerek yapar.
            //Her personelin id farklı olacağından dolayı veritabanında id filtrelenir ve buna göre diğer bilgiler değiştirilebilir
            //Eq burada veritabanı ile sistemdeki id karşılaştırmasını yapar. Set güncel bilgileri alır. UpdateOne ise bilgileri veritabanından değiştirir.
            var filtre = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(Personel.PersonelID));
            var güncelle = Builders<BsonDocument>.Update
                .Set("PersonelAdı", Personel.PersonelAdı)
                .Set("PersonelSoyad", Personel.PersonelSoyad)
                .Set("Şifre", Personel.Şifre)
                .Set("Tc", Personel.Tc)
                .Set("Email", Personel.Email)
                .Set("DoğumTarihi", Personel.DoğumTarihi)
                .Set("TelNo", Personel.TelNo)
                .Set("KullanıcıAdı", Personel.KullanıcıAdı)
                .Set("PersonelFoto", Personel.PersonelFoto);
            PersonelKoleksiyon.UpdateOne(filtre, güncelle);
        }
        //Personel girişinden sonra her formun sol üst köşesinde o personelin fotoğrafı gelir. PersonelFotoGetir metodu bu işe yarar.
        public Personeller PersonelFotoGetirr(String KullanıcıAdı)
        {
            //MongoDB veritabanına bağlantı cümlesi
            var connection = new MongoBağlantı();
            var PersonelKoleksiyonu = connection.GetPersonelKoleksiyonu();
            //Personeller kullanıcıAdı eşsiz olduğundan dolayı fotoğrafı kullanıcıAdına göre getirir.
            var filtre = Builders<BsonDocument>.Filter.Eq("KullanıcıAdı", KullanıcıAdı);
            var sonuc = PersonelKoleksiyonu.Find(filtre).FirstOrDefault();
            //KullanıcıAdı veritabanında var ise fotoğrafı döndürür yoksa metod boş döner.
            if (sonuc != null)
            {
                return new Personeller
                {
                    PersonelFoto = sonuc["PersonelFoto"].AsString
                };
            }

            return null;
        }
    }
}
