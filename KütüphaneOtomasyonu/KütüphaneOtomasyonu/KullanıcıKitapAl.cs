using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KütüphaneOtomasyonu.Bağlantı;

namespace KütüphaneOtomasyonu
{
    public partial class KullanıcıKitapAl : Form
    {
        public KullanıcıKitapAl()
        {
            InitializeComponent();
        }
        //Kullanıcı işlemler ve kitapIslemler sınıflarını kullanacağımız için bu sınıfdlardan nesne oluşturuyoruz.
        KullanıcıIslemleri kullanıcıIslemleri = new KullanıcıIslemleri();
        KitapIslemleri kitapIslemleri = new KitapIslemleri();

        private void button6_Click(object sender, EventArgs e)
        {
            //Uygulamayı kapatmamızı sağlayan kod
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Uygulamayı simge haline getiren kod
            this.WindowState = FormWindowState.Minimized;
        }

        private void BtnKitapBağışla_Click(object sender, EventArgs e)
        {
            //KullanıcıKitapBağış formunu oluşturur KitapAl formunu kapatarak KitapBağış formuna geçiş sağlanır.
            KullanıcıKitapBağış kitapBağış = new KullanıcıKitapBağış();
            kitapBağış.Show();
            this.Hide();
        }

        private void BtnÇıkışYap_Click(object sender, EventArgs e)
        {
            //KullanıcıGiriş formunu oluşturur KitapAl formunu kapatarak KitapGiriş formuna geçiş sağlanır.
            KullanıcıGiriş kullanıcıGiriş = new KullanıcıGiriş();
            kullanıcıGiriş.Show();
            this.Hide();
        }

        private void BtnAyarlar_Click(object sender, EventArgs e)
        {
            //KullanıcıAyarlar formunu oluşturur KitapAl formunu kapatarak KulllanıcıAyarlar formuna geçiş sağlanır.
            KullanıcıAyarlar kullanıcıAyar = new KullanıcıAyarlar();
            kullanıcıAyar.Show();
            this.Hide();
        }

        private void BtnAra_Click(object sender, EventArgs e)
        {
            //Kullanıcılar kütüphaneden kitap ödünç almak için kitap bilgilerine göre kitap aratırlar bu kod kısmı bu aramayı sağlar
            string kitapAdi = TxtKitapAdı.Text;
            int? sayfaSayisi = string.IsNullOrEmpty(TxtSayfaSayısı.Text) ? null : (int?)Convert.ToInt32(TxtSayfaSayısı.Text);
            int? baskiSayisi = string.IsNullOrEmpty(TxtBaskı.Text) ? null : (int?)Convert.ToInt32(TxtBaskı.Text);
            string yayinEvi = TxtYayınevi.Text;

            // Yapılan filtrelere göre kitapAl metodu çalıştırılır. 
            var filtrelenmisKitaplar = kitapIslemleri.KitapAl(kitapAdi, sayfaSayisi, baskiSayisi, yayinEvi);

            //Aranan kitaplar datagridviewda gösterilir
            dataGridView1.DataSource = null;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = filtrelenmisKitaplar;

            //Aramalara Göre kaç kitap bulunduğı bilgisini ekrana yazdırır.
            MessageBox.Show($"Toplam {filtrelenmisKitaplar.Count} kitap bulundu.");
        }

        private void KullanıcıKitapAl_Load(object sender, EventArgs e)
        {
            try
            {
                //Kullanıcılar girişte girdiği tc ye göre her formun sol üst köşesinde fotoğrafları getirilir. Buradaki tc global değişkendir.
                long kullaniciTc = KullanıcıGiriş.tc;
                //MongoDB veritabanı bağlantı cümlesi
                var mongoBaglanti = new MongoBağlantı();
                var kullanici = kullanıcıIslemleri.kullanıcıFotoGetirr(kullaniciTc);

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
    }
}
