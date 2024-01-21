![image](https://github.com/InancYucel/Expense-Payment-System/assets/48295407/d6c71a3e-6d9c-402d-9584-e217b8247603)

# Expense Payment System - Masraf Ödeme Sistemi
 Patika.dev'in hazırladığı Akbank .NET Bootcamp'inin bitirme projesi olarak Masraf Ödeme Sistemi isimli uygulamayı geliştirdim. 
 Masraf Ödeme Sistemi, şirket çalışanlarının şirket tarafından ödenecek harcamaların faturalarının sistem aracılığı ile anında şirket yetkililerine gönderebilmesine ve şirket yetkilileri eğer harcamayı onaylarsa harcama bedelinin direkt çalışana aktarılmasını sağlar. Bu sayede çalışan hem evrak fiş vb. toplamaktan kurtulmuş olacak hem de uzun süre sahada olduğu durumda gecikmeden ödemesini alabilecektir. Aynı zamanda şirketin de masraf kalemlerini takip edebilmesi mümkün olacaktır.
 ## Senaryo
 Masraf ödeme sisteminin ana mantığı şuna dayanır; Personel, şirketin ödemesini talep ettiği masrafları bilgileri ile birlikte sisteme yükler. Sisteme yüklediği anda şirketin yetkili kullanıcısı bu talepleri bütün bilgileriyle beraber görebilir. Masraf talebinde yazan bilgilere istinaden yönetici masraf talebini reddedebilir ya da onaylayabilir. 
 Onaylanan talepler ödeme emri sistemine gönderilir. Talebin ödemesinin kur cinsine göre personele Fast yada Swift ödemesi yapılır. Ödeme kuruna göre şirketin hesaplarından ilgilisinin para miktarından çıkış, personelin ilgili hesabına para girişi olur.
 Personel talepleri ile alakalı filtrelemeleri, reddedilen taleplerini işletebileceği vb. fonksiyonlara sahiptir.
 Yönetici bütün tablolara okuma ve yazma hakkının yanı sıra ödeme onaylama rapor oluşturma vb. gibi haklara sahiptir.
 ## Kullanıcı Rolleri
İki adet kullanıcı rolümüz var. Bunlar **admin** ve **staff** rolleri.
### Staff Rolü
* 🌑 **Masraf Girişi:**  Personel **Expenses** bölümü altındaki **CreateExpenseWithStaffId** ile  kendi için masraf girişi yapabilir. *
* 🌒 **Masraf Taleplerini Görme:**  Personel **Expenses** bölümü altındaki **GetExpenseWithStaffId** ile  kendi masraf taleplerini görebilir. *
*  🌓 **Masraf Taleplerini Filtreleme 1:** Personel **Expenses** bölümü altındaki **FilterExpenseWithRequestStatus** ile kendi masraf taleplerini, masraf talebinin kabul edilip edilmeme durumuna göre filtreleyebilir. *    **
*  🌔 **Masraf Taleplerini Filtreleme 2:** Personel **Expenses** bölümü altındaki **FilterExpenseWithInvoiceAmount** ile kendi masraf taleplerini, masrafın parasal değerine göre filtreleyebilir. **InvoiceAmountBegin** filtrelemenin dip değeri,         **InvoiceAmountEnd:** ise filtrelemenin tepe değeridir.  Örneğin 2 numaralı staffId'ye sahip personelin 250 ve 10000 Amount arasındaki masraf talepleri.* **
*  🌕 **Reddedilen Talepleri ve Ret Sebeplerini Görme:** Personel Expenses bölümü altındaki **GetRejectedRefundRequest** ile kendi masraf taleplerinden ret olanları görebilir. *
*  📜 **Rapor işlemleri** Personel kendi işlem hareketlerini rapor halinde **PDF** olarak çıktı alabilir.  
*  ✨*Personel sadece kendi ID'sini bileceği için işlemleri sadece kendisi için uygulayabiliyor. Sistemde kayıtlı mevcut StaffId'ler {**1**,**2**,**3**,**4**}
* ⭐** Request Status yani istek durumlarının girilebilir üç değeri var bunlar {"**approved**", "**waiting**", "**denied**"} Approved onaylanmış masraf taleplerini, waiting henüz cevap verilmemiş olanları, denied ise bir red açıklaması verilerek reddedilmişleri belirtir.

### Admin Rolü
* 🌀 **Tüm personellerin taleplerini görebilir:** Yönetici **Expenses** bölümü altındaki **GET/Expenses** ile bütün talepleri görüntüleyebilir.  
**Talepleri Değerlendirme:** Yönetici **Expenses** bölümü altındaki **ReplyApplication** ile ödemesini cevaplandırmak istediği **expenseId**'yi girer. Body kısmında ödeme tutarı **invoiceAmount**, kur tipi **invoiceCurrencyType**, masraf kategorisi **expenseCategory**,  masraf talebinin cevaplandırılacağı alan **expenseRequestStatus** ve eğer talep reddedilecekse **expensePaymentRefusal** alanlarını doldurur. * **
* 🌊 **Personel tanımlaması yapabilir:** Yönetici **Staff** bölümü altındaki **POST/Staff** alanından Personelin bilgilerini girerek personeli oluşturur. Daha sonraki masraf ödeme işlemlerinin sorunsuz bir şekilde ilerlemesi için oluşturulan Personele ait **Account** da oluşturulması gerekir.
*  ⛄ **Ödeme Kategorisi İşlemleri:** Yönetici **PaymentCategories** bölümü altındaki standart işlemler ile kategoriler ekleyebilir, güncelleyebilir, silebilir.
* ✨ *Sistemde kayıtlı default **expenseCategory**  alanlar {"**Dinner**","**Fuel**","**Material**","**Ticket**,"**Transportation**", "**Health**", "**Repair**"}
* ⭐ ** Request Status'e verilecebilecek cevaplar  {"**approved**", "**waiting**", "**denied**"} 

## Veritabanı Diyagramı 
![Databse Diagram](https://github.com/InancYucel/Expense-Payment-System/assets/48295407/aa667ab1-90e9-44a2-adbb-0778997fde64)

## 🔨 Ön Gereklilikler Ve Kurulum 
1. Microsoft Sql Server'ı bu linkten yükleyelim. [MSQL](https://www.microsoft.com/tr-tr/sql-server/sql-server-downloads)
2. Kurulum aşamasında oluşturduğunuz kullanıcı adını ve şifresini not edelim.
3. Azure Data Studio'yu bu linkten yükleyelim [Azure Data Studio](https://learn.microsoft.com/en-us/azure-data-studio/download-azure-data-studio?view=sql-server-ver16&tabs=win-install%2Cwin-user-install%2Credhat-install%2Cwindows-uninstall%2Credhat-uninstall#download-azure-data-studio)
4. IP adresimizi bulmak adına Başlat -> cmd yazıp girdikten sonra ipconfig yazıyoruz. "Ethernet adapter vEthernet (WSL (Hyper-V firewall)):" altındaki IPv4 Adress değerini not alıyoruz.
5. Azure içinde new connection diyerek yeni bir database oluşturun, sql server esnasında oluşturduğunuz kullanıcı adı şifre ve not aldığımız IP adresimizle bağlanıyoruz. 
6. Rider'ı bu linkten yükleyelim [Rider](https://www.jetbrains.com/rider/download/#section=windows)
7. Rider açılış ekranında "Get From Vcs" kısmına Github Repo'sundan aldığımız "https://github.com/InancYucel/Expense-Payment-System.git" linkini kopyalıyoruz. Proje indirilerek Rider tarafından ayağa kaldırılıyor.
8. Aşağıda yazan **Server**, **Database**, **User Id**, **Password** kısımlarını daha önce not aldığımız şekilde doldurup.  (Expense Payment System  -> Ep.Api -> **appsettings.json** içindeki **ConnectionStrings** ile değiştiriyoruz)

```"ConnectionStrings": {  
  "MsSqlConnection": "Server="Your IPv4 Adress"; Database="Your Database Name";Trusted_Connection=false;TrustServerCertificate=True; User Id="Your UserID"; Password="Your Password"; MultipleActiveResultSets=true;Min Pool Size=0;Max Pool Size=100;Pooling=true;"  
} 
```
9. Rider içinde Terminal açılır. "Expense Payment System" dizininde **"cd Ep.Data"** komutu ile Ep.Data'ya girilir.
10.  **"dotnet ef migrations add Users -s ../Ep.Api"** komutu ile veritabanı migrasyonları oluşturulur. İşlem başarılı ise Ep.Data altında Migrations klasörünün oluşması gerekir.
11. **"cd  .."** ile üst klasör Expense Payment System'e çıkılır. **"dotnet ef database update --project "./Ep.Data" --startup-project "./Ep.Api"** komutu ile migrasyonlar veritabanına yazılır. 
12. Proje ilk sefer çalıştırıldığında tanımlı iki kullanıcı admin ve staff'ın tanımlanması ve kullanıcının daha önce girilmiş verilerle çalışabilmesi adına hazır veriler Json dosyalarından veritabanına otomatik yazılır. 

## 🔧 Uygulamayı kullanmaya başlarken 
### 🔑 Kullanıcı adları ve şifreleri
> Username: admin - Password: anafartalar
> Username: staff    - Password: mengen

### 🔓 Kullanıcı girişi ve Token ile yetki almak
1. **Token** bölümü altında girmek istediğiniz hesabın bilgilerini body kısmına girdikten sonra göndeririz.
2.  Response Body'de **token** kısmını tırnaklar(") olamadan kopyalayın.
3.  Projenin en üst sağ bölümünde **Authorize** kısmına tıklayalım **Value** kısmına token'ı yapıştırıp. **Authorize** diyelim.
4. Artık Kullanıcı girişini yaptık. Yetkimiz olan methotları kullanabiliriz.

## 🔐 Metotların Yetkilendirmeleri
* **Expenses** bölümünün **"GetExpenseWithStaffId"**,  **"CreateExpenseWithStaffId"**,  **"UpdateOwnExpenseWithStaffId"**,  **"DeleteOwnExpenseWithStaffId"**,  **"FilterExpenseWithRequestStatus"**,  **"FilterExpenseWithInvoiceAmount"** ve **"GetRejectedRefundRequests"**, metotları **Staff** yetkilendirmesinde
* Geriye kalana **bütün bölümlerdeki metotlar** **Admin** yetkilendirmesindedir. Ortak yetkilendirme kullanılmamıştır.

### Kullanılan Teknolojiler
* Entity Framework
* Fluent Validation
* CQRS Pattern
* Middlewares
* RDLC Report
* JWT Token
* Mapper
* .NET Framework

## Uygulama içi Görüntüler
![image](https://github.com/InancYucel/Expense-Payment-System/assets/48295407/3cf2891a-0599-439a-95be-c10b546880a5)
![image](https://github.com/InancYucel/Expense-Payment-System/assets/48295407/6eb2da22-b50b-4bda-84b9-98c631a8f037)
![image](https://github.com/InancYucel/Expense-Payment-System/assets/48295407/9a2a5be3-6f67-4d2a-8696-0f3ab147a6be)
![image](https://github.com/InancYucel/Expense-Payment-System/assets/48295407/817a48c9-7834-400e-bf5b-a8dd2ae4ec47)

## Databese kayıt örnekleri
![image](https://github.com/InancYucel/Expense-Payment-System/assets/48295407/82d389b9-b73c-4396-af39-3cf2fb8ab306)
![image](https://github.com/InancYucel/Expense-Payment-System/assets/48295407/12093310-875e-479f-a24e-79fa2521ec75)
![image](https://github.com/InancYucel/Expense-Payment-System/assets/48295407/33e7ebf5-98ac-4137-b5b2-7fe9186ae1a4)
![image](https://github.com/InancYucel/Expense-Payment-System/assets/48295407/aaf0c1d8-70a5-417a-b883-f3c208534d9f)
![image](https://github.com/InancYucel/Expense-Payment-System/assets/48295407/7dcc9ded-f659-4931-b1a8-647fa314642b)




