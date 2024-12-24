using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KütüphaneOtomasyonu.Bağlantı;
using KütüphaneOtomasyonu.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace KütüphaneOtomasyonu
{
    public partial class KullanıcıAyarlar : Form
    {
        public KullanıcıAyarlar()
        {
            InitializeComponent();
        }
        //Kullanıcı işlemler ve kullanıcı sınıfları kullanacağımız için bu sınıflardan birer nesne oluşturuyoruz.
        KullanıcıIslemleri KullanıcıIslemleri = new KullanıcıIslemleri();
        Kullanıcı kullanıcı = new Kullanıcı();

        private void button5_Click(object sender, EventArgs e)
        {
            //Uygulamayı simge haline getiren kod
            this.WindowState = FormWindowState.Minimized;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Uygulamayı kapatmamızı sağlayan kod
            Application.Exit();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Kullanıcı girişi formuna gitmemizi sağlar.KullanıcıGiriş formunu gösterir ve kullanıcıAyarlar formunu kapatır.
            KullanıcıGiriş kullanıcıGiriş = new KullanıcıGiriş();
            kullanıcıGiriş.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Kullanıcı kitapbağış formuna gitmemizi sağlar.KitapBağış formunu gösterir ve kullanıcıAyarlar formunu kapatır.
            KullanıcıKitapBağış kitapBağış = new KullanıcıKitapBağış();
            kitapBağış.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Kullanıcı kitapAl formuna gitmemizi sağlar.Kullanıcı kitap al formunu gösterir ve kullanıcıAyarlar formunu kapatır.
            KullanıcıKitapAl kullanıcıKitapAl = new KullanıcıKitapAl();
            kullanıcıKitapAl.Show();
            this.Hide();
        }

        private void KullanıcıAyarlar_Load(object sender, EventArgs e)
        {
            //KullanıcıAyarlar formu yüklenirken kullanıcı bilgilerini gösteren kod bloğu. 
            try
            {
                long kullaniciTc = KullanıcıGiriş.tc;
                //MongoDb ve kullanıcıIslemleri sınıfı ile bağlantı kurar.
                var mongoBaglanti = new MongoBağlantı();
                var kullanici = KullanıcıIslemleri.kullanıcıBilgileriGetir(kullaniciTc);
                //Eğer kullanıcı var ise bilgilerini döndürür
                if (kullanici != null)
                {
                    TxtId.Text = kullanici.KulllanıcıID ?? "";
                    TxtAd.Text = kullanici.KullanıcıAdı ?? "";
                    TxtSoyad.Text = kullanici.KullanıcıSoyad ?? "";
                    TxtEmail.Text = kullanici.Email ?? "";
                    TxtSifre.Text = kullanici.Şifre ?? "";
                    TxtMevcutSifre.Text = kullanici.Şifre ?? "";
                    MaskedTc.Text = kullanici.Tc.ToString();
                    MaskedDoğumTarihi.Text = kullanici.DoğumTarihi != null ? kullanici.DoğumTarihi.ToString("dd-MM-yyyy") : "";
                    MaskedTelNo.Text = kullanici.TelNo ?? "";
                    ComboOgrenimTürü.Text = kullanici.ÖğrenimTürü ?? "";
                    //Fotoğrafları görüntüleyebilmek için sistemde base&4 formatından byte çevirmemiz gerekiyor
                    //burda çevirir ve fotoğrafı ayarlar formunda gösterir
                    if (!string.IsNullOrEmpty(kullanici.KullanıcıFoto))
                    {
                        string base64Image = kullanici.KullanıcıFoto; 
                        byte[] imageBytes = Convert.FromBase64String(base64Image);
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            Image img = Image.FromStream(ms);
                            PictrueBoxFoto.Image = img;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı bilgileri bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            //Eğer bilgiler dönmez ise uygulama çökmez bize hata mesajı verir. Bundan kaynaklı try catch kullandık.
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try
            {
                long kullaniciTc = KullanıcıGiriş.tc;
                //MongoDb ye bağlanır kullanıcı adına göre formun sol üst köşesindeki fotoğradı döndürür.
                var mongoBaglanti = new MongoBağlantı();
                var kullanici = KullanıcıIslemleri.kullanıcıFotoGetirr(kullaniciTc);

                if (kullanici != null)
                {

                    if (!string.IsNullOrEmpty(kullanici.KullanıcıFoto))
                    {
                        string base64Image = kullanici.KullanıcıFoto;
                        byte[] imageBytes = Convert.FromBase64String(base64Image);
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            Image img = Image.FromStream(ms);
                            pictureBox1.Image = img;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı bilgileri bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBilgiGüncelle_Click(object sender, EventArgs e)
        {
            //Kullanıcılar bilgilerini değiştirmek isterse yeni bilgilerini girer ve bilgileri güncelle butonu ile bilgilerini veritabanında değiştirirler
            string kullanıcıFotoBase64 = null;
            if (PictrueBoxFoto.Image != null)
            {
                //Fotoğraf değişimini sağlayan kod bloğu
                using (MemoryStream ms = new MemoryStream())
                {
                    PictrueBoxFoto.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imageBytes = ms.ToArray();
                    kullanıcıFotoBase64 = Convert.ToBase64String(imageBytes);
                }
            }
            //kullanıcıIslemleri sınıfındaki KullanıcıGüncelle metodu kullanılarak kullanıcı bilgileri güncellenir.
            String id = TxtId.Text;
                var KullanıcıGüncelle = new Kullanıcı
                {
                    KulllanıcıID = id,
                    Email = TxtEmail.Text,
                    KullanıcıAdı = TxtAd.Text,
                    KullanıcıSoyad = TxtSoyad.Text,
                    Şifre = TxtYeniŞifre.Text,
                    Tc = long.Parse(MaskedTc.Text),
                    DoğumTarihi = DateTime.Parse(MaskedDoğumTarihi.Text),
                    TelNo = MaskedTelNo.Text,
                    ÖğrenimTürü = ComboOgrenimTürü.Text,
                    KullanıcıFoto = kullanıcıFotoBase64,
                };
                KullanıcıIslemleri.KullanıcıGüncelle(KullanıcıGüncelle);
                MessageBox.Show("Kullanıcı Bilgileri Güncellendi");
            }
        //Kullanıcıların fotoğraf seçmesi için butona tıklamaları gerekir. Burada buton işlev kazanır
        private void BtnFotoSeç_Click(object sender, EventArgs e)
        {
            //Dosya uzantılarına göre fotoğraf seçtirme işlemini sağlar.
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                PictrueBoxFoto.Image = Image.FromFile(openFileDialog.FileName);
            }
        }
    }
}
