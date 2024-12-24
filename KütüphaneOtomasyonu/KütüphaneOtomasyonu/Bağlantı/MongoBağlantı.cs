using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace KütüphaneOtomasyonu.Bağlantı
{
    public class MongoBağlantı
    {
        private IMongoDatabase _database;

        public MongoBağlantı()
        {
            //MongoClient MongoDB veritabanına bağlanmak için genel bir sınıftır. Bu sınıf MongoDB.driver kütüphanesinden gelir.
            //"mongodb://localhost:27017" MongoDB sunucusunun adresidir
            var client = new MongoClient("mongodb://localhost:27017");
            //MongoDb de birden fazla veritabanımız olabilir. Burada hangi veritabanı üzerinde çalışmak istiyorsak burda belirtmemiz gerekiyor.
            _database = client.GetDatabase("KütüphaneDB");
        }
        //KütüphaneDB veritabanımızdaki kitaplar koleksiyonuna erişmemizi sağlayan koleksiyon kodu. Bunları sql de tablolara benzetebiliriz.
        public IMongoCollection<BsonDocument> GetKitapKoleksiyon()
        {
            return _database.GetCollection<BsonDocument>("Kitaplar");
        }
        //KütüphaneDB veritabanımızdaki Kullanıcılar koleksiyonuna erişmemizi sağlayan koleksiyon kodu. Bunları sql de tablolara benzetebiliriz.
        public IMongoCollection<BsonDocument> GetKullanıcıKoleksiyonu()
        {
            return _database.GetCollection<BsonDocument>("Kullanıcılar");
        }
        //KütüphaneDB veritabanımızdaki Personeller koleksiyonuna erişmemizi sağlayan koleksiyon kodu. Bunları sql de tablolara benzetebiliriz.
        public IMongoCollection<BsonDocument> GetPersonelKoleksiyonu()
        {
            return _database.GetCollection<BsonDocument>("Personeller");
        }
    }
}
