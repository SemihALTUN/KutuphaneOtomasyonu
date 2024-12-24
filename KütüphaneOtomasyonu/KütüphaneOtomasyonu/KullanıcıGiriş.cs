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
using MongoDB.Bson;
using MongoDB.Driver;

namespace KütüphaneOtomasyonu
{
    public partial class KullanıcıGiriş : Form
    {
        public KullanıcıGiriş()
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

        private void BtnPersonelGirişi_Click(object sender, EventArgs e)
        {
            //Personel girişi formuna gitmemizi sağlar.PersonelGirişi formunu gösterir ve kullanıcıGiriş formunu kapatır.
            PersonelGiriş personelGiriş = new PersonelGiriş();
            personelGiriş.Show();
            this.Hide();
        }

        private void BtnKayıtOl_Click(object sender, EventArgs e)
        {
            //Kullanıcı kayıt formuna gitmemizi sağlar.KullanıcıKayıt formunu gösterir ve kullanıcıGiriş formunu kapatır.
            KullanıcıKayıt kullanıcıKayıt = new KullanıcıKayıt();
            kullanıcıKayıt.Show();
            this.Hide();
        }
        //Ayarlar formunda tc ye göre diğer bilgileri döndüreceğimiz için tc yi globel değişken olarak tanımladık.
        public static long tc { get; set; }
        private void BtnGirişYap_Click(object sender, EventArgs e)
        {
            //Kullanıcı giriş yaparken tc ve şifre kontrolü sağlar
            try
            {                
                tc = long.Parse(TxtTc.Text);
                string password = TxtSifre.Text;

                //KullanıcıIslemleri sınıfından kullanıcıGirişi metodunu kullanır 
                bool sonuc = KullanıcıIslemleri.KullaniciGirişi(tc, password);

                if (sonuc)
                {
                    //Tc ve şifre doğru ise giriş başarılı bilgisi gönderir. KullanıcıKitapBağışı formunu açarak kullanıcıGiriş formunu kapatır.
                    MessageBox.Show("Giriş Başarılı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    KullanıcıKitapBağış kullanıcıKitapBağış = new KullanıcıKitapBağış();
                    kullanıcıKitapBağış.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Hatalı TC veya Şifre!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("TC kimlik numarası sadece rakamlardan oluşmalıdır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void KullanıcıGiriş_Load(object sender, EventArgs e)
        {

        }
    }
}
