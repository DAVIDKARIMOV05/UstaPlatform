using UstaPlatform.Domain.Entities;
using UstaPlatform.Infrastructure.Pricing;
using UstaPlatform.Infrastructure.Utils;

namespace UstaPlatform.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "UstaPlatform - Arcadia Şehir Yönetimi";
            Console.WriteLine("UstaPlatform Simülatörü Başlatıldı");
            Console.WriteLine("=============================================");

            // 1. Eklenti (Plugin) Klasörünü Ayarla
            // Çözüm (Solution) ana dizininde 'PLUGIN_DLLs' adında bir klasör oluşturur.
            string solutionDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string pluginDir = Path.Combine(solutionDir, "PLUGIN_DLLs");
            Directory.CreateDirectory(pluginDir);

            Console.WriteLine($"Eklenti klasörü hazırlandı: {pluginDir}");
            Console.WriteLine("\nLÜTFEN DİKKAT:");
            Console.WriteLine("1. 'UstaPlatform.Rules.Default' projesini derleyin (Build).");
            Console.WriteLine($"2. Oluşan 'UstaPlatform.Rules.Default.dll' dosyasını");
            Console.WriteLine($"   '{pluginDir}' klasörüne MANUEL olarak kopyalayın.");
            Console.WriteLine("\nKopyaladıktan sonra devam etmek için Enter'a basın...");
            Console.ReadLine();

            // 2. Senaryo 1: Sadece 'Default' kuralı yüklü
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("--- SENARYO 1: Sadece Varsayılan Kurallar Yüklü ---");
            Console.ResetColor();

            // Motoru başlat. Motor klasörü tarayacak ve 'Default.dll'yi bulacak.
            PricingEngine engine1 = new PricingEngine(pluginDir);

            // Test Verisi Oluşturma (Object Initializers kullanılır [cite: 23])
            var usta = new Usta { AdSoyad = "Ali Usta", UzmanlikAlani = "Tesisatçı" };
            var vatandas = new Vatandas { AdSoyad = "Ayşe Hanım" };

            var workOrder = new WorkOrder
            {
                IsTanimi = "Mutfak Sızıntısı",
                TemelUcret = 300.0m,
                AtanmisUsta = usta,
                TalepEden = vatandas,
                PlanlananTarih = new DateOnly(2025, 10, 25) // Cumartesi
            };

            Console.WriteLine($"İş Emri: {workOrder.IsTanimi} ({workOrder.PlanlananTarih} - {workOrder.PlanlananTarih.DayOfWeek})");
            engine1.CalculatePrice(workOrder);

            // 3. Diğer C# Özelliklerini Demo
            DemoAdvancedFeatures(workOrder);

            // 4. Senaryo 2: Yeni Kural Ekleme (OCP Testi) [cite: 41, 42, 43]
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n--- SENARYO 2: Yeni Kural Eklentisi (OCP Testi) ---");
            Console.ResetColor();
            Console.WriteLine("ŞİMDİ:");
            Console.WriteLine("1. 'UstaPlatform.Rules.Loyalty' projesini derleyin (Build).");
            Console.WriteLine($"2. Oluşan 'UstaPlatform.Rules.Loyalty.dll' dosyasını");
            Console.WriteLine($"   YİNE '{pluginDir}' klasörüne MANUEL olarak kopyalayın.");
            Console.WriteLine("\nAna uygulama (UstaPlatform.App) DEĞİŞMEDEN ve YENİDEN DERLENMEDEN,");
            Console.WriteLine("yeni kuralın nasıl yüklendiğini göreceksiniz. [cite: 17, 29]");
            Console.WriteLine("\nKopyaladıktan sonra devam etmek için Enter'a basın...");
            Console.ReadLine();

            // Motoru YENİDEN başlat
            Console.WriteLine("Motor yeniden başlatılıyor ve klasör taranıyor...");
            PricingEngine engine2 = new PricingEngine(pluginDir);

            Console.WriteLine($"Aynı İş Emri: {workOrder.IsTanimi} ({workOrder.PlanlananTarih} - {workOrder.PlanlananTarih.DayOfWeek})");
            // Fiyatı yeni motorla (içinde 2 kural var) tekrar hesapla
            engine2.CalculatePrice(workOrder);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[SONUÇ] Gördüğünüz gibi, ikinci hesaplamada 'Sadakat İndirimi' kuralı");
             Console.WriteLine("otomatik olarak dahil oldu ve nihai fiyat değişti. ");
            Console.ResetColor();
            Console.WriteLine("=============================================");
            Console.WriteLine("Demo tamamlandı. Çıkmak için Enter'a basın.");
            Console.ReadLine();
        }

        static void DemoAdvancedFeatures(WorkOrder order)
        {
            Console.WriteLine("\n--- İleri C# Özellikleri Demosu ---");

            // 1. init-only [cite: 21]
            Console.WriteLine($"İş Emri ID (init-only): {order.Id}");
            // order.Id = Guid.NewGuid(); // <-- Bu satır CS8852 hatası verir (Derlenmez)

            // 2. Indexer [cite: 24]
            Schedule schedule = new Schedule();
            schedule.AddWorkOrder(order);
            var today = new DateOnly(2025, 10, 25);
            var todaysJobs = schedule[today]; // Indexer kullanımı
            Console.WriteLine($"Schedule Indexer [{today}]: {todaysJobs.Count} iş bulundu.");

            
            Route route = new Route { { 10, 20 }, { 50, 60 } }; // Koleksiyon Başlatıcı
            Console.WriteLine("Route (Özel IEnumerable):");
            foreach (var (X, Y) in route)
            {
                Console.WriteLine($"  - Durak: (X={X}, Y={Y})");
            }
        }
    }
}
