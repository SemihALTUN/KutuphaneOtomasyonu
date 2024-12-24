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
using KütüphaneOtomasyonu.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace KütüphaneOtomasyonu
{
    public partial class PersonelKitapIslemleri : Form
    {
        public PersonelKitapIslemleri()
        {
            InitializeComponent();
        }
        //PersonelIslemler ve KitapIslemler sınıflarındaki metotları kullanmak için PersonelIslemleri ve KitapIslemleri nesnesi oluşturuyoruz.
        KitapIslemleri KitapIslemleri = new KitapIslemleri();
        PersonelIslemleri personelIslemleri = new PersonelIslemleri();

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

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                //Personellerin sisteme kitap eklemesi için kitap bilgilerini kontrol eden kod bloğu
                if (string.IsNullOrWhiteSpace(TxtKitapAdı.Text) ||
                    string.IsNullOrWhiteSpace(TxtYayınevi.Text) ||
                    string.IsNullOrWhiteSpace(TxtDil.Text))
                {
                    //Boş alan bırakılırsa ekrana mesaj gönderilir.
                    MessageBox.Show("Kitap adı, yayınevi ve dil bilgisi boş olamaz.");
                    return;
                }

                //Kitap eklenirken sayfa syısı ve baskı sayısı gibi sayısal değerlerin kontrolü sağlanır. Eğer boş bölümler varsa ekrana mesaj gönderilir
                if (!int.TryParse(TxtSayfaSayısı.Text.Trim(), out int sayfaSayısı) ||
                    !int.TryParse(TxtBaskıSayısı.Text.Trim(), out int baskıSayısı))
                {
                    MessageBox.Show("Sayfa sayısı ve baskı sayısı geçerli bir sayı olmalıdır.");
                    return;
                }

                //Kitap nesnesi oluşturulur personelin girdiği kitap bilgilerine göre kitap veritabanına eklenir
                var Kitap = new Kitap
                {
                    KitapAdı = TxtKitapAdı.Text,
                    KitapYazar = TxtYazar.Text,
                    KitapYayınevi = TxtYayınevi.Text,
                    KitapSayfaSayısı = sayfaSayısı,
                    KitapBaskıSayısı = baskıSayısı,
                    KitapDil = TxtDil.Text,
                };

                //KitapIslemleri sınıfından kitapEkle metodu ile kitap verştabanına eklenir
                KitapIslemleri.KitapEkle(Kitap);

                //Kitap başarılı bir şekilde eklenirse personele mesaj ile bilgi verilir.
                MessageBox.Show("Kitap Eklendi");
            }
            catch (FormatException ex)
            {
                //Sayısal veri yerlerine metin yazılırsa hata mesajı ekrana verilir.
                MessageBox.Show("Veri formatı hatalı: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //PersonelAyarlar formu oluşturur ve ekrana verir PersonelKitapIslemleri formunu kapatır.
            PersonelAyarlar personelAyarlar = new PersonelAyarlar();
            personelAyarlar.Show();
            this.Hide();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //PersonelKitapBilgileriGüncelle formu oluşturur ve ekrana verir PersonelKitapIslemleri formunu kapatır.
            PersonelKitapBilgileriGüncelle personelkitapgüncel = new PersonelKitapBilgileriGüncelle();
            personelkitapgüncel.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //PersonelGiriş formu oluşturur ve ekrana verir PersonelKitapIslemleri formunu kapatır.
            PersonelGiriş personelGiriş = new PersonelGiriş();
            personelGiriş.Show();
            this.Hide();
        }

        private void BtnKitapListesiGetir_Click(object sender, EventArgs e)
        {
            //Veritabanındaki kitapları datagridviewa listeler
            List<Kitap> Kitaplar = KitapIslemleri.TümKitaplarıGetir();
            dataGridView1.DataSource = Kitaplar;
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            //Kitap id sine göre kitap silme işlemini gerçekleştirir.
            String KitapId = TxtKitapID.Text;
            KitapIslemleri.KitapSil(KitapId);
            MessageBox.Show("Silme İşlemi Gerçekleştirildi");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //PersonelPersonelEkleme formu oluşturur ve ekrana verir PersonelKitapIslemleri formunu kapatır.
            PersonelPersonelEkleme personelPersonelEkleme = new PersonelPersonelEkleme();
            personelPersonelEkleme.Show();
            this.Hide();
        }

        private void PersonelKitapIslemleri_Load(object sender, EventArgs e)
        {
            //PersonelKitapIslemleri formu yüklenirken personel kullanıcıAdlarına göre personelin resmini formun sol üst köşesine getirir.
            try
            {
                String kullanıcıAdı = PersonelGiriş.KullanıcıAdı;
                //MongoDB bağlantısı ve personelIslemleri sınıfından PersonelFotoGetir metodu kullanımı
                var mongoBaglanti = new MongoBağlantı();
                var Personel = personelIslemleri.PersonelFotoGetirr(kullanıcıAdı);
                //Eğer kullanıcıAdına göre veritabanında kullanıcı bulunursa fotoğrafını byte dizine çevirerek formun sol üst köşesine bastırır.
                if (Personel != null)
                {

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
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
