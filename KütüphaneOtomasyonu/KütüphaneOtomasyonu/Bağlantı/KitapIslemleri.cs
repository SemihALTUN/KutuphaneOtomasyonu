using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KütüphaneOtomasyonu.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace KütüphaneOtomasyonu.Bağlantı
{
    //Projedeki kitap bağışlama kitap alma kitapları listeleme gibi olayları ayrı bir kitapIslemleri sınıfında oluşturdum
    public class KitapIslemleri
    {
        //MongoDB veritabanımıza bağlanarak kitap bağışlama işlemini gerçekleştiren metodumuz
        public void KitapEkle(Kitap kitap)
        {
            //MongoDB bağlantı cümlesi
            var connection = new MongoBağlantı();
            var KitapKoleksiyon = connection.GetKitapKoleksiyon();
            //Veritabanımız Dökümanları Bson türünde aldığı için burada kitap özelliklerimizi bson formatı şeklinde tanımlıyoruz.
            //InsertOne MongoDB ekleme yapan anahtar kelime
            BsonDocument document = new BsonDocument
            {
                {"KitapAdı",kitap.KitapAdı },
                {"KitapYazar",kitap.KitapYazar },
                {"KitapYayınevi",kitap.KitapYayınevi },
                {"KitapSayfaSayısı",kitap.KitapSayfaSayısı },
                {"KitapBaskıSayısı",kitap.KitapBaskıSayısı },
                {"KitapDil",kitap.KitapDil }
            };
            KitapKoleksiyon.InsertOne(document);

        }
        //MongoDB veritabanımıza bağlanarak kitapları listeleyen liste
        public List<Kitap> TümKitaplarıGetir()
        {
            //MongoDB bağlantı cümlesi
            var connection = new MongoBağlantı();
            var KitapKoleksiyon = connection.GetKitapKoleksiyon();
            //Find MongoDB veritabanında arama yapmamızı sağlar. Tolist metodu ise veritabanındaki bilgileri liste halinde döndürür.
            var Kitaplar = KitapKoleksiyon.Find(new BsonDocument()).ToList();

            List<Kitap> KitapListesi = new List<Kitap>();

            foreach (var kitap in Kitaplar)
            {
                KitapListesi.Add(new Kitap
                {
                    KitapAdı = kitap["KitapAdı"].ToString(),
                    KitapYazar = kitap["KitapYazar"].ToString(),
                    KitapYayınevi = kitap["KitapYayınevi"].ToString(),
                    KitapSayfaSayısı = kitap["KitapSayfaSayısı"].ToInt32(),
                    KitapBaskıSayısı = kitap["KitapBaskıSayısı"].ToInt32(),
                    KitapDil = kitap["KitapDil"].ToString(),
                    KitapId = kitap["_id"].ToString(),
                });
            }
            return KitapListesi;
        }
        //Kitap alma işlemini gerçekleştiren KitapAl listesi 
        public List<Kitap> KitapAl(string kitapAdi, int? sayfaSayisi, int? baskiSayisi, string yayinEvi)
        {
            //MongoDB bağlantı cümlesi
            var connection = new MongoBağlantı();
            var kitapKoleksiyon = connection.GetKitapKoleksiyon();
            //Özelliklere göre filtreleme yaparak kitap arammımızı sağlayan bölüm
            var filterBuilder = Builders<BsonDocument>.Filter;
            var filters = new List<FilterDefinition<BsonDocument>>();

            //Filtreleme Kriterleri
            if (!string.IsNullOrEmpty(kitapAdi))
                filters.Add(filterBuilder.Regex("KitapAdı", new BsonRegularExpression(kitapAdi, "i")));

            if (sayfaSayisi.HasValue)
                filters.Add(filterBuilder.Eq("KitapSayfaSayısı", sayfaSayisi.Value));

            if (baskiSayisi.HasValue)
                filters.Add(filterBuilder.Eq("KitapBaskıSayısı", baskiSayisi.Value));

            if (!string.IsNullOrEmpty(yayinEvi))
                filters.Add(filterBuilder.Eq("KitapYayınevi", yayinEvi));

            //Fİltreleri birleştirdiğimiz kısım
            var combinedFilter = filters.Count > 0
                ? filterBuilder.And(filters)
                : FilterDefinition<BsonDocument>.Empty;

            //MongoDB sorgusu Find komutu arama yapar ToList ise arama yapılan bilgileri liste halinde döndürür
            var kitaplar = kitapKoleksiyon.Find(combinedFilter).ToList();

            List<Kitap> kitapListesi = new List<Kitap>();
            foreach (var kitap in kitaplar)
            {
                kitapListesi.Add(new Kitap
                {
                    KitapAdı = kitap["KitapAdı"].ToString(),
                    KitapYazar = kitap["KitapYazar"].ToString(),
                    KitapYayınevi = kitap["KitapYayınevi"].ToString(),
                    KitapSayfaSayısı = kitap["KitapSayfaSayısı"].ToInt32(),
                    KitapBaskıSayısı = kitap["KitapBaskıSayısı"].ToInt32(),
                    KitapDil = kitap["KitapDil"].ToString(),
                    KitapId = kitap["_id"].ToString()
                });
            }
            return kitapListesi;
        }
        //Personellerin kitap silmesini sağlayan metot.
        public void KitapSil(String KitapId)
        {
            //MongoDB bağlantı cümlesi
            var connection = new MongoBağlantı();
            var KitapKoleksiyon = connection.GetKitapKoleksiyon();
            //Burada kitapları id lerine göre sileceğimiz için id filtreliyoruz. DeleteOne ise MongoDB silme işlemini gerçekleştirir
            var filtre = Builders<BsonDocument>.Filter.Eq("_id",ObjectId.Parse(KitapId));
            KitapKoleksiyon.DeleteOne(filtre);
        }
        //Personellerin Kitap bilgilerini güncellemesi için KitapGüncelle metodu Kitapaları id göre filtreler ve özellikleri set ile değiştirir
        public void KitapGüncelle(Kitap kitap)
        {
            var connection = new MongoBağlantı();
            var KitapKoleksiyon = connection.GetKitapKoleksiyon();
            var filtre = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(kitap.KitapId));
            var güncelle = Builders<BsonDocument>.Update
                .Set("KitapAdı", kitap.KitapAdı)
                .Set("KitapYazar", kitap.KitapYazar)
                .Set("KitapBaskıSayısı", kitap.KitapBaskıSayısı)
                .Set("KitapSayfaSayısı", kitap.KitapSayfaSayısı)
                .Set("KitapYayınevi", kitap.KitapYayınevi)
                .Set("KitapDil", kitap.KitapDil);
            KitapKoleksiyon.UpdateOne(filtre, güncelle);

        }
    }
}
