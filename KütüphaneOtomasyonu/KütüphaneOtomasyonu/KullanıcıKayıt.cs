using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KütüphaneOtomasyonu.Bağlantı;
using KütüphaneOtomasyonu.Entities;

namespace KütüphaneOtomasyonu
{
    public partial class KullanıcıKayıt : Form
    {
        public KullanıcıKayıt()
        {
            InitializeComponent();
        }
        //Kullanıcı işlemler sınıfını kullanacağımız için bu sınıfdan nesne oluşturuyoruz.
        KullanıcıIslemleri KullanıcıIslemleri = new KullanıcıIslemleri();

        private void button2_Click(object sender, EventArgs e)
        {
            //Uygulamayı kapatmamızı sağlayan kod
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Uygulamayı simge haline getiren kod
            this.WindowState = FormWindowState.Minimized;
        }

        private void BtnFotoSeç_Click(object sender, EventArgs e)
        {
            //Kullanıcı kayıt olurken fotoğraf eklemesi gerekir bu kod butona işlev sağlayarak bilgisayardan fotoğraf eklememizi sağlar
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                PictureBoxFoto.Image = Image.FromFile(openFileDialog.FileName);
            }
        }
        //Girilen bilgilere göre kayıt olma. Butona işlev sağlar 
        private void BtnKayıtOl_Click(object sender, EventArgs e)
        {
            string base64Foto = ConvertImageToBase64(PictureBoxFoto.Image);
            //Kullanıcıdan bilgiler alınır ve KullanıcıEKle metodu ile kullanıcıyı veritabanımıza kaydeder.
            var Kullanıcı = new Kullanıcı
            {
                KullanıcıAdı = TxtAd.Text,
                KullanıcıSoyad = TxtSoyad.Text,
                Email = TxtEmail.Text,
                Şifre = TxtSifre.Text,
                Tc = long.Parse(MasktBoxTc.Text),
                DoğumTarihi = DateTime.Parse(MaskBoxDoğumT.Text),
                TelNo = MaskedBoxTelNo.Text,
                ÖğrenimTürü = ComboBoxOgrenimTuru.Text,
                KullanıcıFoto = base64Foto,
            };
            KullanıcıIslemleri.KullanıcıEkle(Kullanıcı);
            MessageBox.Show("Kullanıcı Eklendi");
            //Kullanıcı eklendikten sonra kayıt formu kapanır. Direk kullanıcıGiriş formu karşımıza gelir.
            KullanıcıGiriş kullanıcıGiriş = new KullanıcıGiriş();
            kullanıcıGiriş.Show();
            this.Hide();
        }
        private string ConvertImageToBase64(System.Drawing.Image image)
        {
            //Fotoğrafları veritabanına kaydetmek için Base64 formatına dönüştürmemiz gerekir bu metod bu özelliği sağlar.
            using (var ms = new System.IO.MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //KullanıcıKayıt Formunu kapatır ve KullanıcıGiriş formunu açar.
            KullanıcıGiriş kullanıcıGiriş = new KullanıcıGiriş();
            kullanıcıGiriş.Show();
            this.Hide();
        }

        private void KullanıcıKayıt_Load(object sender, EventArgs e)
        {

        }
    }
}
