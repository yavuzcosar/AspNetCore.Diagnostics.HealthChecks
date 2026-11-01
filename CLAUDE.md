# Toltek.Diagnostics.HealthChecks
> Xabaril `AspNetCore.Diagnostics.HealthChecks` çatalı: ASP.NET Core sağlık kontrolleri, yayıncıları ve HealthChecks.UI; Toltek paket önekiyle (`Toltek.HealthChecks.*`) yayımlanır.
> Doğrulandığı commit: `d331f08e` · 2026-11-01 · Modül belgesi: yok (üst depo çatalı; yalnız bu kök belge yazıldı)

## Belge sözleşmesi
- Bu depoda iş yapmadan önce depo kökünün ve dokunacağın projenin `CLAUDE.md`'sini, akış değişikliğinde modül belgesini oku. Kodu
  baştan taramadan önce bu harita kullanılır.
- Belgenin başındaki "Doğrulandığı commit" damgasından sonra değişenleri görmek için `git log --oneline <hash>..HEAD -- <yol>`;
  yalnız değişen dosyaları oku.
- Public API'yi, ucu, izni, varlığı, akışı, hostu ya da konvansiyonu değiştiren her değişiklik **aynı commit'te** ilgili
  `CLAUDE.md`'yi ve gerekiyorsa modül belgesindeki diyagramı günceller, damgayı yeniler.
- Belge ile kod çelişirse kod esastır; belge düzeltilir.

## Ne olduğu ve üst depo ilişkisi
- Bir Toltek modülü DEĞİL: `ToltekModule`, `[DependsOn]`, EF, izin, yerelleştirme yok; Toltek kodlama rehberi bu depoya uygulanmaz.
  Kod Xabaril'indir; Toltek yalnız paketleri kendi önekiyle üretir ve Framework üzerinden tüketir.
- Uzaklar (`git remote -v`): `origin` = `yavuzcosar/AspNetCore.Diagnostics.HealthChecks` (Toltek çatalı), `upstream` =
  `Xabaril/AspNetCore.Diagnostics.HealthChecks`. `origin/master...upstream/master` = 10 | 1: çatal 10 commit önde, upstream 1 commit önde
  (son birleştirme `87b4f941`). Güncelleme upstream'den `master`'a birleştirmeyle yapılır.
- Toltek'e özgü değişiklikler (`git log upstream/master..origin/master`):
  - `src/Directory.Build.props`: `PackageId` = `Toltek.$(MSBuildProjectName)`; yazar, şirket ve URL `yavuzcosar` (`2b373412`).
  - `.github/workflows/healthchecks_*_cd*.yml`: `PACKAGE_NAME` `Toltek.HealthChecks.*` (`abcd2ff3`); yayın GitHub Packages'a
    (`https://nuget.pkg.github.com/yavuzcosar/index.json`, `reusable_cd_workflow.yml`).
  - `build/versions.props`: tüm paket sürümleri `9.0.2` (`33b90d86`).
  - `build.bat`, `test.txt`, `DataColumn` eklemesi (`64f3a3ca`), UI yayın iş akışı düzeltmeleri (`d2a696a5`, `6e48b530`).
- Bu belge `origin/master`'ı anlatır. Ana checkout farklı daldadır (aşağıda "Açık dallar").

## Projeler (gruplar; 145 csproj)
- `src/` (71 klasör): **kontroller** `HealthChecks.<Hedef>` (SqlServer, Redis, MongoDb, Rabbitmq, Rabbitmq.v6, Network, Uris, System, SignalR,
  OpenIdConnectServer, Kafka, Elasticsearch, Hangfire, Kubernetes, Aws.*, Azure.*, Gcp.CloudFirestore …); **yayıncılar**
  `HealthChecks.Publisher.*` (ApplicationInsights, CloudWatch, Datadog, Prometheus, Seq); `HealthChecks.Prometheus.Metrics`;
  **UI**: `HealthChecks.UI`, `.UI.Client`, `.UI.Core`, `.UI.Data`,
  `.UI.{InMemory,SqlServer,PostgreSQL,MySql,SQLite}.Storage`, `.UI.K8s.Operator`.
- `src/` köküne ortak kaynaklar: `ApiMarker.cs`, `CallerArgumentExpressionAttribute.cs`, `DisposalHostedService.cs`,
  `HealthCheckResultTask.cs`; kökteki `Directory.Build.targets` ilk ikisini paketlenebilir her projeye bağlar.
- `test/` (kontrol başına `HealthChecks.<X>.Tests`, `FunctionalTests`; bir kısmı Docker/Testcontainers ister), `samples/`,
  `extensions/` (Azure DevOps release gate vsix), `deploy/`, `doc/`, `build/docker-*`.
- Çözüm: `AspNetCore.Diagnostics.HealthChecks.sln` (`.slnx` yok).

## Paket adlandırma, sürüm ve yayın
- `PackageId` = `Toltek.` + proje adı → `Toltek.HealthChecks.Redis`, `Toltek.HealthChecks.UI.Client` (`src/Directory.Build.props`).
- Sürüm: her proje `<VersionPrefix>$(HealthCheckX)</VersionPrefix>`; değerler `build/versions.props` (şu an hepsi `9.0.2`).
  Framework aynı numarayı `ToltekHealthChecks` ile sabitler (`Toltek.Framework/.build/target-common.targets`: `9.0.2`):
  **iki yerdeki sürüm elle eşit tutulur**.
- Merkezi paket sürümleri `Directory.Packages.props`; `TreatWarningsAsErrors=true`, nullable açık, Release'te
  `build/strongNameKey.snk` ile imza (`HealthChecks.UI*` hariç), Roslynator analizörleri.
- TFM (origin/master): `DefaultNetCoreApp` = `net8.0`, kütüphaneler `net8.0;netstandard2.0` (bazıları `netstandard2.1`/`net472`).
  Toltek net10 kullanır; net10 geçişi yalnız ana checkout'taki dalda (aşağıda).
- Yerel yayın: `build.bat` → `C:\Toltek\Build\BuildWithoutPrefix.ps1 -Action build -SolutionPath AspNetCore.Diagnostics.HealthChecks.sln
  -PackageFolder C:\Toltek\TLibrary\Framework` (betik bu depo dışındadır, içeriği doğrulanmadı). Paketler Toltek `pack.bat`
  hattından değil, bu betikten yerel `TLibrary\Framework` klasörüne basılır. CI: `.github/workflows/healthchecks_<ad>_{ci,cd,cd_preview}.yml`;
  `release-<ad>-*` / `release-all-*` etiketi GitHub Packages'a yayımlar.
- `Nuget.config` yalnız nuget.org'u açar (`<clear />`).

## Toltek'in kullandığı paketler
Yalnız Framework doğrudan tüketir; Platform depolarının (`*-docs` worktree'leri) hiçbir csproj/props/targets dosyası `Toltek.HealthChecks.*`
paketine doğrudan başvurmaz. Platform modülleri `Toltek.AspNetCore.HealthChecks.UI` (Framework paketi) ile dolaylı tüketir.

| Paket | Kullanan depo | Proje |
|---|---|---|
| `Toltek.HealthChecks.MongoDb`, `.Network`, `.OpenIdConnectServer`, `.Rabbitmq`, `.Redis`, `.SignalR`, `.SqlServer`, `.System`, `.Uris` | Toltek.Framework | `src/Modules/HealthChecks/Toltek.AspNetCore.HealthChecks` |
| `Toltek.HealthChecks.UI`, `.UI.Client`, `.UI.InMemory.Storage`, `.UI.SqlServer.Storage` | Toltek.Framework | `src/Modules/HealthChecks/Toltek.AspNetCore.HealthChecks.UI` |
| yukarıdaki 13 paketin tümü | Toltek.Framework | `src/Helpers/Toltek.NugetSampler` |
| `Toltek.HealthChecks.*` (sürüm sabiti) | Toltek.Framework | `.build/target-packages.targets` (`Update` satırları), `.build/target-common.targets` |
| `Toltek.AspNetCore.HealthChecks.UI` (dolaylı) | Toltek.Status | `Toltek.Status`, `Toltek.Status.Components`, `Toltek.Status.HttpApi` |
| `Toltek.AspNetCore.HealthChecks.UI` (yorum satırı) | Toltek.Platform | `src/Apps/Toltek.Platform.Pages` |

- Yeni kontrol kullanılacaksa önce Framework'teki sarmalayıcı csproj'a, sonra `target-packages.targets`'a `Update` satırı eklenir.
- Framework'te güvenlik yorumları var: `SSH.NET` (`Network`, NU1903) ve `SignalR.Client` zinciri (`target-packages.targets`, `target-microsoft.targets`).

## Hostlar ve çalıştırma
- Çalışan host yok (kütüphane + `samples/`). `H:\Toltek\Env\dev-ports.json`: `products.Platform.modules.HealthChecks` blok 21,
  web 5210, api 5211 ayrılmış; bu depoda bu portları kullanan host bulunmadı.

## İzinler ve kimlik
- Yok. `samples/HealthChecks.UI.Oidc` yalnız örnektir.

## Konvansiyonlar ve tuzaklar
- Yeni kontrol: `src/HealthChecks.<Hedef>` + `test/HealthChecks.<Hedef>.Tests` + `healthchecks_<hedef>_{ci,cd,cd_preview}.yml`
  + `versions.props` girdisi + `<VersionPrefix>`; `PackageId` önekini elle yazma (props verir).
- `Rabbitmq` ve `Rabbitmq.v6` ayrı projelerdir; Framework yalnız `Toltek.HealthChecks.Rabbitmq`'yu alır.
- Upstream birleştirirken `Toltek.`/`yavuzcosar` yerine `Xabaril`'in geri gelmediğini `Directory.Build.props`, iş akışları, `versions.props` için denetle.
- Windows büyük-küçük harf duyarsızdır: aynı adın farklı yazımıyla ikinci klasör/dosya yaratma.

## Açık dallar
- `git worktree list`: ana checkout `Toltek.Diagnostics.HealthChecks` → `copilot/update-package-versions-central-management` (`45f61b8f`);
  belge worktree'si `claude/proje-belgeleri` (`d331f08e`).
- `copilot/update-package-versions-central-management`: `origin/master`'a birleşmemiş **8 commit** (net10 geçişi, paket sürümü
  güncellemeleri: `7eb05f00`, `c02d2237`, `0e653ec4`, `4abaf66e` …). Bu dalda `DefaultNetCoreApp` = `net10.0`, UI ve Uris sürümleri `9.0.3`.
  Birleşirse Framework `ToltekHealthChecks` da `9.0.3` olmalıdır. Uzakta ayrıca `origin/v10` dalı var.
