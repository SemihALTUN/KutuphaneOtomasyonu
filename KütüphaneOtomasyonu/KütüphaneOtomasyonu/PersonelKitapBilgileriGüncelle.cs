using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using KütüphaneOtomasyonu.Bağlantı;
using KütüphaneOtomasyonu.Entities;
using System.Text.RegularExpressions;
namespace KütüphaneOtomasyonu
{
    public partial class PersonelKitapBilgileriGüncelle : Form
    {
        public PersonelKitapBilgileriGüncelle()
        {
            InitializeComponent();
        }
        //PersonelIslemler ve KitapIslemler sınıflarındaki metotları kullanmak için PersonelIslemleri ve KitapIslemleri nesnesi oluşturuyoruz.
        KitapIslemleri KitapIslemleri = new KitapIslemleri();
        PersonelIslemleri personelIslemleri = new PersonelIslemleri();
        private void button5_Click(object sender, EventArgs e)
        {
            //Uygulamayı simge haline getiren kod
            this.WindowState = FormWindowState.Minimized;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Personel girişi formu oluştururak ekrana geritir ve PerosnelKitapBilgileriGüncelle formunu kapatır.
            PersonelGiriş personelGiriş = new PersonelGiriş();
            personelGiriş.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //PersonelAyarlar formu oluştururak ekrana geritir ve PerosnelKitapBilgileriGüncelle formunu kapatır.
            PersonelAyarlar personelAyarlar = new PersonelAyarlar();
            personelAyarlar.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //PersonelKitapİşlemleri formu oluştururak ekrana geritir ve PerosnelKitapBilgileriGüncelle formunu kapatır.
            PersonelKitapIslemleri personelKitapIslemleri = new PersonelKitapIslemleri();
            personelKitapIslemleri.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Uygulamayı kapatmamızı sağlayan kod
            Application.Exit();
        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Veritabanındaki tüm kitapları datagridviewa listeler
            List<Kitap> Kitaplar = KitapIslemleri.TümKitaplarıGetir();
            dataGridView1.DataSource = Kitaplar;
        }

        private void BtnGüncelle_Click(object sender, EventArgs e)
        {

            try
            {
                //Kitap id sine göre işlem yapacağımız için kitabın idsini alıyoruz
                String id = TxtKitapId.Text;

                //Regex desen ile textboxa girilen değer metin mi değil mi onun kontrolünü gerçekleştiriyoruz
                string harfDeseni = @"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$";

                //String değer alan textbox kontrolleri
                if (string.IsNullOrWhiteSpace(TxtKitapAdı.Text) || !Regex.IsMatch(TxtKitapAdı.Text.Trim(), harfDeseni))
                {
                    MessageBox.Show("Kitap adı sadece harflerden oluşmalıdır ve boş bırakılamaz.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtYazar.Text) || !Regex.IsMatch(TxtYazar.Text.Trim(), harfDeseni))
                {
                    MessageBox.Show("Yazar adı sadece harflerden oluşmalıdır ve boş bırakılamaz.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtYayınevi.Text) || !Regex.IsMatch(TxtYayınevi.Text.Trim(), harfDeseni))
                {
                    MessageBox.Show("Yayınevi adı sadece harflerden oluşmalıdır ve boş bırakılamaz.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtDil.Text) || !Regex.IsMatch(TxtDil.Text.Trim(), harfDeseni))
                {
                    MessageBox.Show("Kitap dili sadece harflerden oluşmalıdır ve boş bırakılamaz.");
                    return;
                }

                //Sayısal değer alan textbox kontolleri
                if (!int.TryParse(TxtSayfaSayısı.Text.Trim(), out int sayfaSayısı))
                {
                    MessageBox.Show("Sayfa sayısı geçerli bir sayı olmalıdır.");
                    return;
                }

                if (!int.TryParse(TxtBaskıSayısı.Text.Trim(), out int baskıSayısı))
                {
                    MessageBox.Show("Baskı sayısı geçerli bir sayı olmalıdır.");
                    return;
                }

                //Kitap güncelleme nesnesi oluşturulur ve bilgiler alınarak KitapGüncelle metodu ile kitap bilgileri güncellenir
                var KitapGüncelle = new Kitap
                {
                    KitapId = id,
                    KitapAdı = TxtKitapAdı.Text.Trim(),
                    KitapYazar = TxtYazar.Text.Trim(),
                    KitapYayınevi = TxtYayınevi.Text.Trim(),
                    KitapSayfaSayısı = sayfaSayısı,
                    KitapBaskıSayısı = baskıSayısı,
                    KitapDil = TxtDil.Text.Trim(),
                };
                KitapIslemleri.KitapGüncelle(KitapGüncelle);

                //Kitap Güncelleme başarılı olursa ekrana gönderilen mesaj
                MessageBox.Show("Kitap Bilgileri Güncellendi");
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Geçersiz veri formatı: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //PersonelPersonelEkleme formu oluştururak ekrana geritir ve PerosnelKitapBilgileriGüncelle formunu kapatır.
            PersonelPersonelEkleme personelPersonelEkleme = new PersonelPersonelEkleme();
            personelPersonelEkleme.Show();
            this.Hide();
        }

        private void PersonelKitapBilgileriGüncelle_Load(object sender, EventArgs e)
        {
            //PersonelKitapBilgileriGüncelle formu ekrana yüklenirken personel kullanıcı adına göre formun sol üst köşesinde personelin fotoğrafını getirir
            try
            {
                //PersonelGiriş formundan kullanıcıAdı globel değişkeni kullanıcıAdı değişkenine atanır
                String kullanıcıAdı = PersonelGiriş.KullanıcıAdı;
                //MongoDB veritabanı bağlanılır ve PersonelIslemleri sınıfındaki PersonelFOtoGetir metodu ile personelin fotoğrafı ekrana gönderilir.
                var mongoBaglanti = new MongoBağlantı();
                var Personel = personelIslemleri.PersonelFotoGetirr(kullanıcıAdı);

                if (Personel != null)
                {
                    //Fotoğrafın ekrana gönderilmesi için veritabanında Base64 türünde saklanan fotoğraf byte dizine çevirilir.
                    if (!string.IsNullOrEmpty(Personel.PersonelFoto))
                    {
                        string base64Image = Personel.PersonelFoto;
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
            //Uygulamanın çökmemesi için try catch kullanıldı. olası bir hatada ekrana hata mesajı gönderilir.
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
