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
    public partial class KullanıcıKitapBağış : Form
    {
        public KullanıcıKitapBağış()
        {
            InitializeComponent();
        }
        //Kullanıcı işlemler ve kitapIslemler sınıflarını kullanacağımız için bu sınıfdlardan nesne oluşturuyoruz.
        KitapIslemleri KitapIslemleri = new KitapIslemleri();
        KullanıcıIslemleri kullanıcıIslemleri = new KullanıcıIslemleri();

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

        private void BtnBağışla_Click(object sender, EventArgs e)
        {
            //Kullanıcılar kütüphaneye kitap bağışında bulunabilirler. Burada kitap bilgileri alınarak bağışlanan kitap veritabanına eklenir.
            try
            {
                //Kullanıcının girdiği kitap bilgileri kontrol edilir.
                if (string.IsNullOrWhiteSpace(TxtKitapAdı.Text) ||
                    string.IsNullOrWhiteSpace(TxtYayınevi.Text) ||
                    string.IsNullOrWhiteSpace(TxtDil.Text))
                {
                    //Boş bilgi olmaması için kullanıcıya mesaj gönderilir
                    MessageBox.Show("Kitap adı, yayınevi ve dil bilgisi boş olamaz.");
                    return;
                }

                //Baskı sayısı ve sayfa sayısı sayı olarak girilmiş mi bunun kontrolü sağlanır eğer yanlış bir ifade girildiyse kullanıcıya mesaj gönderilir
                if (!int.TryParse(TxtSayfaSayısı.Text, out int sayfaSayısı) ||
                    !int.TryParse(TxtBaskıSayısı.Text, out int baskıSayısı))
                {
                    MessageBox.Show("Sayfa sayısı ve baskı sayısı geçerli bir sayı olmalıdır.");
                    return;
                }

                //Kitap nesnesi oluşturulur ve bağışlanacak kitap bilgileri alınır.
                var Kitap = new Kitap
                {
                    KitapAdı = TxtKitapAdı.Text,
                    KitapYazar = TxtYazar.Text,
                    KitapYayınevi = TxtYayınevi.Text,
                    KitapSayfaSayısı = sayfaSayısı,
                    KitapBaskıSayısı = baskıSayısı,
                    KitapDil = TxtDil.Text,
                };

                //KitapIslemleri sınıfından kitapEKle metodu çağırılır ve kitap veritabanına eklenir. Kullanıcıya kitabınız bağışlandı mesajı gönderilir.
                KitapIslemleri.KitapEkle(Kitap);
                MessageBox.Show("Kitabınız Bağışlandı");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Veritabanındaki kitapları listeler.
            List<Kitap> Kitaplar = KitapIslemleri.TümKitaplarıGetir();
            dataGridView1.DataSource = Kitaplar;
        }

        private void BtnKitapAl_Click(object sender, EventArgs e)
        {
            //KullanıcıKitapAl formunu oluşturur. KullanıcıKitapAl formunu gösterir ve kitapbağış formunu kapatır.
            KullanıcıKitapAl kitapalma = new KullanıcıKitapAl();
            kitapalma.Show();
            this.Hide();
        }

        private void BtnAyarlar_Click(object sender, EventArgs e)
        {
            //KullanıcıAyarlar formunu oluşturur. KullanıcıAyarlar formunu gösterir ve kitapbağış formunu kapatır.
            KullanıcıAyarlar kullanıcıAyar = new KullanıcıAyarlar();
            kullanıcıAyar.Show();
            this.Hide();
        }

        private void BtnÇıkışYap_Click(object sender, EventArgs e)
        {
            //KullanıcıGiriş formunu oluşturur. KullanıcıGiriş formunu gösterir ve kitapbağış formunu kapatır.
            KullanıcıGiriş kullanıcıGiriş = new KullanıcıGiriş();
            kullanıcıGiriş.Show();
            this.Hide();
        }

        private void KullanıcıKitapBağış_Load(object sender, EventArgs e)
        {
            //Form yüklenirken kullanıcı tc sine göre formun sol üst köşesinde kullanıcının fotoğrafı gözükür. Bu kod bunu sağlar.
            try
            {
                //Global tc değişkeni burada kullaniciTC değişkenine atılır
                long kullaniciTc = KullanıcıGiriş.tc;
                //MongoDB bağlanıtsı
                var mongoBaglanti = new MongoBağlantı();
                var kullanici = kullanıcıIslemleri.kullanıcıFotoGetirr(kullaniciTc);
                //Eğer kullanıcı Tc si veritabanında bulunursa fotoğrafı getirir. 
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
