---
title:  "Unity 2D Oyun - Tetris"
date:   2020-10-31 19:57:56 +0300
tages: [unity c# android game tetris]
---
Unity ve game mekaniklerini öğrenmek, Facebook Entegrasyonu sağlamak için geliştirdiğim bu projenin Android için APK'sını aşağıda bulabilirsiniz. Ayrıca tüm kaynak kodları, res dosyaları mevcuttur.

# Tetris Unity Game
![genel_pano](https://i.imgur.com/wZIumap.png)


### Oyun Ana Menüsü
![ana_menu](https://i.imgur.com/yE0tktp.png)
<br>Facebook butonu ile Facebook hesabınızla giriş yapabilirsiniz (bu özellik sadece leaderboarda score kaydedilirken Ad - Soyad yazdırmak için kullanılır), Levelları seçebileceğiniz bir bar, Leaderboard ve Oyun sesi ayarı.

### Leaderboard
![leaderboard](https://i.imgur.com/SZ9lnEF.png)
<br>
Buradaki puanlar **Firebase Real Database** 'de kaydedilmektedir. İnterneti olan herkesde anlık güncelendiği için oyunu oynayan herkesin puanını burada görebilir ve onlarla mücadele edebilirsiniz.

### Gameplay Örneği
![gameplay](https://i.imgur.com/iRkI7Y6.png)
<br>
Bir seviye örneği, aşağıda bulunan butonlar Tetris parcasını sol, sağ, direkt indirme ve rotate, Yukardaki bildirimler bir sonraki Tetris parcasının ne olacağı, toplamda patlatılan dize sayısı, score ve level. Level ilerledikçe tetris parçaları daha hızlı hareket etmekte ve puan daha fazla kazanılmakta.

### Gameplay Pause
![gameplaypause](https://i.imgur.com/3jRhKxZ.png)
<br>
Oyun Durdurma/Devam Etme.

### Facebook Login
![facebook](https://i.imgur.com/UyA1akZ.png)
<br>
Facebook'a giriş yaptıktan sonra alınan isim Ana ekranda gösterilmekte aynı zamanda leaderboarda bu şekilde kaydedilmekte.

# Oyun APK'sı
Oyun Google Playde yüklü olmadığı için Android kimlik hırsızlığı tehdit scam vsvs uyarılarda bulunabilir klasik bir şekilde gönderme yoksay izin ver diyoruz ve oyunu bu şekilde yükleyebilirsiniz. <br> [Unity 2D Oyun - Tetris](https://drive.google.com/file/d/12XuGGFhZttoxPj5fp9BHcQpw_0Ag7JsI/view?usp=sharing)