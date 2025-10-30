UstaPlatform - Şehrin Uzmanlık Platformu

Ders: Nesne Yönelimli Programlama (NYP) ve İleri C# Proje Adı: UstaPlatform 

Bu proje, Arcadia şehrindeki uzmanları (usta) vatandaş talepleriyle eşleştiren, dinamik fiyatlama ve akıllı rota planlama yapabilen genişletilebilir bir platformdur. Projenin temel odak noktası, Açık/Kapalı Prensibi (OCP) kullanılarak ana uygulama kodu değişmeden yeni fiyatlandırma kurallarının (Plug-in olarak) sisteme eklenebilmesidir.



1. Kurulum ve Çalıştırma
Proje, .NET (Örn: .NET 8.0) ortamında bir Konsol Uygulaması olarak çalışır ve dinamik olarak DLL'leri yükler.

Kurulum Adımları
Proje çözüm dosyasını (UstaPlatform.sln) Visual Studio veya tercih ettiğiniz bir IDE ile açın.

Çözümü derleyin (Build Solution). Bu işlem, tüm projeleri (Domain, Infrastructure, Pricing, App ve Rules.*) derleyecektir.

Ana uygulama olan UstaPlatform.App projesinin çıktı klasörüne gidin (Genellikle UstaPlatform.App/bin/Debug/net8.0/).

Bu klasör içerisinde Rules adında yeni bir klasör oluşturun.

Derlenen kural DLL'lerini bu Rules klasörüne kopyalayın:

UstaPlatform.Rules.Default.dll dosyasını Rules klasörüne kopyalayın.

Çalıştırma
UstaPlatform.App projesini Visual Studio üzerinden başlatın (Start) veya terminalden çalıştırın.

Uygulama başladığında, Rules klasöründeki tüm DLL'leri tarayacak ve HaftasonuEkUcretiKurali kuralını otomatik olarak yükleyecektir.

İlk fiyat hesaplaması (hafta sonu ek ücreti dahil) ekranda görünecektir.

Uygulama, Dinamik Kural Yükleme Demosu  için sizden LoyaltyDiscountRule.dll dosyasını beklemenizi isteyecektir.

2. Tasarım Kararları
Proje, SOLID prensiplerine ve rehberde belirtilen İleri C# özelliklerine  bağlı kalarak tasarlanmıştır.


A. Mimari ve SOLID

Çok Projeli Mimari (.sln) : Sorumluluklar, Tek Sorumluluk Prensibi (SRP)  gereği farklı projelere ayrılmıştır:





UstaPlatform.Domain: Temel varlıklar (Usta, Talep, WorkOrder, vb.).


UstaPlatform.Pricing: Fiyatlandırma motoru (PricingEngine) ve arayüzü (IPricingRule).




UstaPlatform.Infrastructure: Yardımcı sınıflar (Guard, MoneyFormatter vb.).

UstaPlatform.App: Ana uygulamayı ve demo akışını yürüten konsol projesi.


Bağımlılıkların Tersine Çevrilmesi (DIP) : PricingEngine sınıfı, somut kural sınıflarına değil, IPricingRule arayüzüne bağımlıdır. Bu, sistemi genişletilebilir kılar.


Açık/Kapalı Prensibi (OCP): Projenin en kritik gereksinimidir. Fiyatlandırma sistemi, yeni kuralların eklenmesine açık, mevcut kodun değiştirilmesine kapalıdır. Bu, aşağıdaki Plug-in mimarisi ile sağlanmıştır.

B. Kritik Tasarım: Plug-in (Eklenti) Mimarisi 

Projenin kalbi olan dinamik fiyatlandırma, ana uygulama kodunu değiştirmeden yeni kuralların eklenmesine olanak tanır.


Bu mimari nasıl çalışır? 


Arayüz (Interface): UstaPlatform.Pricing projesinde, tüm kuralların uyması gereken bir kontrat olan IPricingRule arayüzü tanımlanmıştır. Bu arayüz, CalculatePriceImpact(WorkOrder workOrder) adında tek bir metot içerir.


Kural DLL'leri: Her yeni fiyat kuralı (Örn: HaftasonuEkUcretiKurali veya LoyaltyDiscountRule), IPricingRule arayüzünü uygulayan ve ayrı bir Class Library projesi olarak derlenen bir sınıftır.


Yükleyici (Loader) - PricingEngine: Ana uygulama başlatıldığında, PricingEngine sınıfı çalışır.


Reflection (Yansıma): PricingEngine, constructor'ında (kurucu metodunda) belirtilen Rules klasöründeki tüm .dll dosyalarını tarar.

Tip Bulma: System.Reflection kullanarak bu DLL'lerin içindeki tüm tipleri (sınıfları) gezer ve IPricingRule arayüzünü uygulayanları bulur.

Nesne Yaratma (Activation): Bulduğu her kural tipi için Activator.CreateInstance() kullanarak bir nesne yaratır ve bu nesneyi kendi içindeki List<IPricingRule> listesine ekler.


Çalıştırma: Fiyat hesaplaması istendiğinde (CalculatePrice), motor bu listedeki tüm kuralları bir döngüde çalıştırır ve her kuralın döndürdüğü etkiyi (pozitif veya negatif) toplam fiyata ekler.

Bu tasarım sayesinde, UstaPlatform.App kodunu yeniden derlemeden, sadece Rules klasörüne yeni bir .dll bırakarak sistemi genişletmek mümkündür.

C. İleri C# Özellikleri

init-only Özelliği: Talep, WorkOrder gibi varlıkların Id ve KayitZamani alanları, nesne oluşturulduktan sonra değiştirilemesin diye init olarak ayarlanmıştır.

Dizinleyici (Indexer): Schedule sınıfı, Schedule[DateOnly gun] formatını destekleyen bir dizinleyiciye sahiptir. Bu, o güne ait iş emirleri listesine kolay erişim sağlar.


Özel IEnumerable<T> Koleksiyonu: Route sınıfı, IEnumerable<(int X, int Y)> arayüzünü uygular ve Add(int X, int Y) metodu sayesinde koleksiyon başlatıcıları (new Route { {1,2}, {3,4} }) destekler.

3. Kısa Demo Akışı (Çıktı)
Aşağıdaki konsol çıktısı, sistemin çalıştığını ve Dinamik Plug-in Senaryosunu  başarıyla tamamladığını gösterir.

Plaintext

UstaPlatform Başlatılıyor...
Kural DLL'leri şu klasörden okunacak: C:\...\UstaPlatform.App\bin\Debug\net8.0\Rules
[PricingEngine] Kural yüklendi: HaftasonuEkUcretiKurali

--- Demo Verisi Oluşturuluyor ---
İş Emri Oluşturuldu: 5001 - Planlanan: 25.10.2025 (Saturday)

--- FİYAT HESAPLAMASI ---
Temel Ücret: 150,00 ₺
Hesaplanan Toplam Fiyat: 200,00 ₺

Tesisatçı Mehmet için 25.10.2025 gününe iş emri eklendi.

--- DİNAMİK KURAL YÜKLEME (PLUG-IN) DEMOSU ---
Lütfen 'LoyaltyDiscountRule.dll' adında yeni bir kural DLL'ini
'C:\...\UstaPlatform.App\bin\Debug\net8.0\Rules' klasörüne kopyalayın.
Kopyaladıktan sonra ENTER tuşuna basın...
[KULLANICI ENTER'A BASAR]

Sistem yeniden başlatılıyor ve kurallar tekrar yükleniyor...
[PricingEngine] Kural yüklendi: HaftasonuEkUcretiKurali
[PricingEngine] Kural yüklendi: LoyaltyDiscountRule

--- YENİ FİYAT HESAPLAMASI (Yeni Kural Dahil) ---
Temel Ücret: 150,00 ₺
[LoyaltyDiscountRule] Sadakat indirimi uygulandı.
Hesaplanan Yeni Toplam Fiyat: 175,00 ₺

BAŞARILI: Yeni kural (LoyaltyDiscountRule) ana uygulama değişmeden fiyat hesaplamasına o
