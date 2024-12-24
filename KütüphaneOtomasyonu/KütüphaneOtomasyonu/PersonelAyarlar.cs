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

namespace KütüphaneOtomasyonu
{
    public partial class PersonelAyarlar : Form
    {
        public PersonelAyarlar()
        {
            InitializeComponent();
        }
        //PersonelIslemler sınıfındaki metotları kullanmak için PersonelIslemleri nesnesi oluşturuyoruz.
        PersonelIslemleri PersonelIslemleri = new PersonelIslemleri();

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

        private void button7_Click(object sender, EventArgs e)
        {
            //PersonelGiriş formu oluşturur. PersonelAyarlar formunu kapatarak PersonelGİriş formunu gösterir.
            PersonelGiriş personelGiriş = new PersonelGiriş();
            personelGiriş.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //PersonelPersonelEKleme formu oluşturur. PersonelAyarlar formunu kapatarak PersonelPersonelEkleme formunu gösterir.
            PersonelPersonelEkleme personelPersonelEkleme = new PersonelPersonelEkleme();
            personelPersonelEkleme.Show();
            this.Hide();
        }

        private void PersonelAyarlar_Load(object sender, EventArgs e)
        {
            //Personeller uygulamaya giriş yaparken kullanıcıAdlarını kullanırlar. KullanıcıAdlarına göre kullanıcıAyarlar formunda bilgileri gösterilir
            try
            {
                //Globel kullanıcıAdı değişkeni PersonelKullanıcıAdı değişkenine atılır.
                String PersonelKullanıcıAdı = PersonelGiriş.KullanıcıAdı;
                //MongoDB bağlantısı
                var mongoBaglanti = new MongoBağlantı();
                var personel = PersonelIslemleri.PersonelBilgileriGetir(PersonelKullanıcıAdı);
                //KullanıcıAdına göre eğer o personel varsa diğer bilgileri ekranda gösterilir.
                if (personel != null)
                {
                    TxtId.Text = personel.PersonelID ?? "";
                    TxtAd.Text = personel.PersonelAdı ?? "";
                    TxtSoyad.Text = personel.PersonelSoyad ?? "";
                    TxtEmail.Text = personel.Email ?? "";
                    TxtSifre.Text = personel.Şifre ?? "";
                    TxtMevcutSifre.Text = personel.Şifre ?? "";
                    MaskTc.Text = personel.Tc.ToString();
                    MaskDoğumTarihi.Text = personel.DoğumTarihi != null ? personel.DoğumTarihi.ToString("dd-MM-yyyy") : "";
                    MaskTelNo.Text = personel.TelNo ?? "";
                    TxtKullanıcıAdı.Text = personel.KullanıcıAdı ?? "";
                    //Veritabanında Base64 formatında saklanan fotoğraflar byte dizine çevirilerek ekranda gösterilir bu kod bloğu bu işlemi sağlar.
                    if (!string.IsNullOrEmpty(personel.PersonelFoto))
                    {
                        string base64Image = personel.PersonelFoto;
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
            //Uygulamanın çökmemsi için try catch kullanılmıştır. Herhangi bir hatada uygulama çökmez sadece hata mesajı gönderir.
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try
            {
                //Globel kullanıcıAdı değişkeni PersonelKullanıcıAdı değişkenine atılır.
                String kullanıcıAdı = PersonelGiriş.KullanıcıAdı;
                //MongoDB bağlantısı
                var mongoBaglanti = new MongoBağlantı();
                var Personel = PersonelIslemleri.PersonelFotoGetirr(kullanıcıAdı);
                //KullanıcıAdına göre veritabanında personel bulunursa personelin fotoğrafını formun sol üst kçşesinde gösterir.
                if (Personel != null)
                {
                    //Fotoğrafı Base64 den byte dizine çeviren kod
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
            //Hata mesajı yakalamak için try catch kullanılmıştır.
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBilgiGüncelle_Click(object sender, EventArgs e)
        {
            //Personeller bilgilerini değiştirmek isteyebilir. PersonelAyarlar formu bu işlebi sağlar.
            string PersonelFotoBase64 = null;
            if (PictrueBoxFoto.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    PictrueBoxFoto.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imageBytes = ms.ToArray();
                    PersonelFotoBase64 = Convert.ToBase64String(imageBytes);
                }
            }
            //Personel idlerine göre diğer bilgilerin değişimi sağlanır
            String id = TxtId.Text;
            var PerosnelGüncelle = new Personeller
            {
                PersonelID = id,
                PersonelAdı = TxtAd.Text,
                PersonelSoyad = TxtSoyad.Text,
                Email = TxtEmail.Text,
                KullanıcıAdı = TxtKullanıcıAdı.Text,
                Şifre = TxtYeniSifre.Text,
                Tc = long.Parse(MaskTc.Text),
                DoğumTarihi = DateTime.Parse(MaskDoğumTarihi.Text),
                TelNo = MaskTelNo.Text,
                PersonelFoto = PersonelFotoBase64,
            };
            //PersonelIslemleri sınıfından PersonelGüncelle metodu ile personel bilgileri güncellenir ve bilgilerin güncellendiğine dair mesaj gönderilir.
            PersonelIslemleri.PersonelGüncelle(PerosnelGüncelle);
            MessageBox.Show("Kullanıcı Bilgileri Güncellendi");
        }
        //Fotoğraf değiştirirken bilgisaayrımızdan footğraf seçmemizi sağlayan kod bloğu 
        private void BtnFotoSeç_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                PictrueBoxFoto.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //PersonelKitapIslemleri formunu oluşturur PersonelAyarlar formunu kapatarak PersonelKitapIslemleri formunu gösterir.
            PersonelKitapIslemleri personelKitapIslemleri = new PersonelKitapIslemleri();
            personelKitapIslemleri.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //PersonelKitapBilgileriGÜncelle formunu oluşturur PersonelAyarlar formunu kapatarak PersonelKitapBilgileriGÜncelle formunu gösterir.
            PersonelKitapBilgileriGüncelle personelKitapBilgileriGüncelle = new PersonelKitapBilgileriGüncelle();
            personelKitapBilgileriGüncelle.Show();
            this.Hide();
        }
    }
}
