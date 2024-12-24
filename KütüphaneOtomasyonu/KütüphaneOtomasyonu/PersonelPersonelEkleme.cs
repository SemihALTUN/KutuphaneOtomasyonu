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
    public partial class PersonelPersonelEkleme : Form
    {
        public PersonelPersonelEkleme()
        {
            InitializeComponent();
        }
        //PersonelIslemler sınıfındaki metotları kullanmak için PersonelIslemleri nesnesi oluşturuyoruz.
        PersonelIslemleri PersonelIslemleri = new PersonelIslemleri();

        private void button8_Click(object sender, EventArgs e)
        {
            string base64Foto = ConvertImageToBase64(PictureBoxFoto.Image);
            //Girilen Personel bilgilerine göre personel ekleme işlemi yapılır. Personel eklerken PersonelIslemleri sınıfından PersonelEkle metodu kullanılır.
            //Personel eklenildiği taktirde ekrana personel eklendi mesajı verilir
            var Personel = new Personeller
            {
                PersonelAdı = TxtPersonelAd.Text,
                PersonelSoyad = TxtPersonelSoyad.Text,
                Email = TxtPersonelEmail.Text,
                Tc = long.Parse(MaskedBoxTc.Text),
                DoğumTarihi = DateTime.Parse(MaskedBoxDoğumTarihi.Text),
                TelNo = MaskedBoxTelNo.Text,
                KullanıcıAdı = TxtPersonelKullanıcıAdı.Text,
                PersonelFoto = base64Foto,
                Şifre = TxtPersonelŞifre.Text,
            };
            PersonelIslemleri.PersonelEkle(Personel);
            MessageBox.Show("Personel Eklendi");
        }
        //Personel eklenirken personelin fotoğrafı da olmalıdır. bu kod fotoğrafı byte dizisinden Base64 çevirir ve veritabanında saklanmasını sağlar.
        private string ConvertImageToBase64(System.Drawing.Image image)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        private void BtnFotoSeç_Click(object sender, EventArgs e)
        {
            //Personel eklerken bilgisayardan fotoğraf seçmemizi sağlayan kod bloğu
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                PictureBoxFoto.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

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
            //PersonelGiriş formu oluşturur ve ekrana getirir. PersonelPersonelEKleme formunu kapatır.
            PersonelGiriş personelGiriş = new PersonelGiriş();
            personelGiriş.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //PersonelAyarlar formu oluşturur ve ekrana getirir. PersonelPersonelEkleme formunu kapatır.
            PersonelAyarlar personelAyarlar = new PersonelAyarlar();
            personelAyarlar.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //PersonelKitapBilgileriGüncelle formu oluşturur ve ekrana getirir. PersonelPersonelEKleme formunu kapatır.
            PersonelKitapBilgileriGüncelle personelKitapBilgileriGüncelle = new PersonelKitapBilgileriGüncelle();
            personelKitapBilgileriGüncelle.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //PersonelKitapIslemleri formu oluşturur ve ekrana getirir. PersonelPersonelEKleme formunu kapatır.
            PersonelKitapIslemleri personelKitapIslemleri = new PersonelKitapIslemleri();
            personelKitapIslemleri.Show();
            this.Hide();
        }

        private void PersonelPersonelEkleme_Load(object sender, EventArgs e)
        {
            //PersonelPersonelEkleme formu ekrana yüklenirken personelin kullanıcıAdına göre formun sol üst kçşesinde personelin fotoğrafı gösterilir.
            try
            {
                //Globel KullanıcıAdı değişkeni kullanıcıAdı değişkenine atıldı
                String kullanıcıAdı = PersonelGiriş.KullanıcıAdı;
                //MongoDB bağlanıtsı sağlandı ve personelIslemler sınıfından PersonelFotoGetir metodu kullanılarak fotoğraf ekrana getirildi
                var mongoBaglanti = new MongoBağlantı();
                var Personel = PersonelIslemleri.PersonelFotoGetirr(kullanıcıAdı);

                if (Personel != null)
                {
                    //Fotoğrafı formda göstermek için veritabanından Base64 formatında aldığımız fotoğraf byte dizine dönüştürüldü
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
            //Uygulama çökmemesi için try catch kullanıldı olası hatada ekrana hata mesajı yazdırıldı.
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
