# LuduStudyCase-
Proje Hakkında 
Selamlar! Projeyi geliştirirken modern yazılım prensiplerini kullanmaya ve genişletilebilir bir 
mimari oluşturmaya özen gösterdim. SOLID prensiplerine uygun ve sürdürülebilir bir kod tabanı oluşturmayı hedefledim.

Teknik Detaylar

Ana Sistemler
 
GameController ve State Management 
- Oyunun ana akışını yöneten sistem
- Modüler yapısı sayesinde yeni oyun modlarının eklenmesine uygun
- Event-based iletişim sistemi

 
Grid System
- Esnek grid yapısı
- Farklı boyut ve şekillerde level tasarımı
- Ölü hücre ve Obstacle konumlandırma.
- Object pooling ile optimize edilmiş cube yönetimi

 
PowerUp System
- Interface tabanlı modüler yapı
- Horizontal Rocket, Vertical Rocket ve Bomb powerup'ları
- Yeni güç tipleri için genişletilebilir altyapı

  
Animation System 
- DOTween ile entegre edilmiş animasyon sistemi
- Modüler ve genişletilebilir yapı
- Performans odaklı animasyon yönetimi


Genişletilebilir Yapı  
--Proje özellikle aşağıdaki alanlarda genişlemeye uygun tasarlandı:
 
-PowerUp Sistemi için: - Yeni güç tipleri kolayca eklenebilir - Mevcut güçler özelleştirilebilir - Farklı efekt ve davranışlar tanımlanabilir 
-Cube Sistemi için: - Yeni küp tipleri eklenebilir - Farklı davranış ve özellikler tanımlanabilir - Çeşitli engel tipleri oluşturulabilir 
-Grid Sistemi için: - Farklı grid şekilleri - Çeşitli level yapıları - Özel oyun kuralları 
-Level Editor - Özel level tasarım aracı - Sürükle-bırak engel yerleştirme - Grid boyutu ve oyun parametreleri ayarlama - Level test etme imkanı 
=> Scriptable Objects => Level1Data  bunu türetip GameControllerdeki level dataya atayabilirsiniz.
  
Optimizasyon 
- Object pooling sistemi 
- Event-based iletişim 
- Optimize edilmiş match-finding 
- Efektif memory yönetimi 

## Son Notlar 
Case’ de yazılım mimarisi ve game development konusundaki yaklaşımımı elimden geldiğince 
göstermeye çalıştım. 
çok esnek zaman imkanım olmadığı için elimdeki süre içerisinde elimden geldiğince temiz 
yazmaya çalıştım, iyi incelemeler , tekrardan teşekkürler      
