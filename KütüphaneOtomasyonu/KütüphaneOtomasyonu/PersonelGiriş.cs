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

namespace KütüphaneOtomasyonu
{
    public partial class PersonelGiriş : Form
    {
        public PersonelGiriş()
        {
            InitializeComponent();
        }
        //PersonelIslemler sınıfındaki metotları kullanmak için PersonelIslemleri nesnesi oluşturuyoruz.
        PersonelIslemleri PersonelIslemleri = new PersonelIslemleri();

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

        private void BtnKullanıcıGiriş_Click(object sender, EventArgs e)
        {
            //KullanıcıGiriş formunu oluşturur ve Personel Giriş formunu kapatarak KullanıcıGiriş formunu gösterir
            KullanıcıGiriş kullanıcıGiriş = new KullanıcıGiriş();
            kullanıcıGiriş.Show();
            this.Hide();
        }
        //Diğer formlarda kullanıcıAdına göre bilgiler döndüreceğimiz için personel kullanıcı adlarını globel değişken olarak tanımlıyoruz
        public static string KullanıcıAdı { get; set; }
        private void BtnGirişYap_Click(object sender, EventArgs e)
        {
            //Personel kullanıcıAdı ve şifresine göre giriş yapmasını sağlayan kod bloğu
            try
            {
                //Personelden kullanıcıAdı ve şifresini alır
                KullanıcıAdı = TxtKullaniciAdi.Text;
                string Şifre = TxtSifre.Text;

                //PersonelIslemleri sınıfından PersonelGiriş Metodunu kullanarak işlem yapar.
                bool sonuc = PersonelIslemleri.PersonelGirişi(KullanıcıAdı, Şifre);
                //Eğer kullanıcıAdı ve şifre veritabanı ile eşleşiyorsa girişi sağlar ve personelKitapIslemleri formu oluşturarak bu forma geçiş sağlar.
                if (sonuc)
                {
                    MessageBox.Show("Giriş Başarılı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PersonelKitapIslemleri personelKitapIslemleri = new PersonelKitapIslemleri();
                    personelKitapIslemleri.Show();
                    this.Hide();
                }
                else
                {
                    //Personel hatalı kullanıcıAdı ve şifre girerse ekrana uyarı mesajı gönderilir.
                    MessageBox.Show("Hatalı Kullanıcı Adı veya Şifre!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PersonelGiriş_Load(object sender, EventArgs e)
        {

        }
    }
}
