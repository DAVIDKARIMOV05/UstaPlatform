using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection; // DLL taraması için
using UstaPlatform.Domain.Entities;
using UstaPlatform.Domain.Interfaces;
using UstaPlatform.Infrastructure.Utils;

namespace UstaPlatform.Infrastructure.Pricing
{

    public class PricingEngine
    {
        
        private readonly List<IPricingRule> _rules = new();

        public PricingEngine(string pluginFolderPath)
        {
            LoadRules(pluginFolderPath);
        }

        private void LoadRules(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine($"[Motor HATA] Eklenti klasörü bulunamadı: {folderPath}");
                return;
            }

           
            var dllFiles = Directory.GetFiles(folderPath, "*.dll");

            foreach (var file in dllFiles)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    foreach (var type in assembly.GetTypes())
                    {
                        
                        if (typeof(IPricingRule).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                        {
                            var ruleInstance = (IPricingRule)Activator.CreateInstance(type);
                            if (ruleInstance != null)
                            {
                                _rules.Add(ruleInstance);
                                Console.WriteLine($"[Motor] Kural Yüklendi: {ruleInstance.RuleName} ({Path.GetFileName(file)})");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Motor HATA] Eklenti yüklenemedi: {Path.GetFileName(file)}. Hata: {ex.Message}");
                }
            }
        }

       
        public decimal CalculatePrice(WorkOrder workOrder)
        {
            decimal sonFiyat = workOrder.TemelUcret;
            Console.WriteLine($"--- Fiyat Hesaplanıyor (Temel: {MoneyFormatter.Format(sonFiyat)}) ---");

            foreach (var rule in _rules)
            {
                var adjustment = rule.CalculatePriceAdjustment(workOrder);
                if (adjustment != 0)
                {
                    Console.WriteLine($"  -> Kural Uygulandı: {rule.RuleName} (Etki: {MoneyFormatter.Format(adjustment)})");
                    sonFiyat += adjustment;
                }
            }

            workOrder.SonFiyat = sonFiyat;
            Console.WriteLine($"--- Nihai Fiyat: {MoneyFormatter.Format(sonFiyat)} ---");
            return sonFiyat;
        }
    }
}
