# گزارش کارهای انجام‌شده و باقی‌مانده — Base و BaseTemplate

تاریخ آخرین بررسی: ۲۰۲۶-۱۰-۱۷

این فایل وضعیت کارهای همین دوره را ثبت می‌کند؛ موفقیت Build/Pack به معنی انتشار روی NuGet یا تأیید کامل محیط Production نیست.

## آخرین تغییر وضعیت بسته‌بندی

- طبق توضیح نهایی کاربر، قابلیت‌های Extension در Base پیاده‌سازی شده‌اند و کار روی پوشهٔ قدیمی کنار گذاشته شد؛ تغییرات نیمه‌تمام در سورس حفظ شده‌اند و انتشار کامل ۵۱ بسته تأیید نشده است. بخش‌های تاریخی پایین‌تر دربارهٔ شروع‌نشدن ارتقا، وضعیت قبل از این تلاش را بیان می‌کنند.
- اجرای پیش‌فرض build-package.bat دوباره فقط ۲۳ پروژهٔ راهکار فعال و ۴ Utility را انتخاب می‌کند. پوشهٔ قدیمی Extentsions طبق تصمیم نهایی کاربر منبع انتشار نیست و گزینه‌ای برای افزودن آن وجود ندارد.
- وابستگی Serilog Utility به UsersManagement.Abstractions به نسخهٔ منتشرشدهٔ 9.0.0 بازگردانده شد تا به Extension نیمه‌تمام نسخهٔ ۱۰ وابسته نباشد؛ خود Utility همچنان net10.0 و نسخهٔ 10.0.0 است.
- خطای NU1301 ناشی از نبودن external-cache اصلاح شد: پوشه و منبع کش خارجی پیش از اولین Restore ساخته می‌شوند.
- دو خطای تولید کد OpenAPI با استفاده از وابستگی سازگار OpenAPI.NET 2.x رفع شدند؛ تست HTTP شامل ۲۴ بررسی موفق بود.
- خطاهای SkiaSharp با جایگزینی FilterQuality توسط SKSamplingOptions رفع شدند؛ Build راهکار فعال با صفر خطا موفق شد.

- پاک‌سازی artifacts به درخواست کاربر انجام شد: بسته‌های نهایی، manifest و NuGet.Config نگه داشته شدند؛ کش‌ها و لاگ‌های تاریخی حذف شدند. ارجاع‌های قدیمی به فایل‌های log در گزارش‌ها، سابقهٔ اعتبارسنجی هستند و فایل‌های مربوط دیگر نگهداری نمی‌شوند. اسکریپت در ساخت موفق کش‌ها و لاگ‌های موقت را حذف می‌کند؛ KeepBuildCache برای نگهداری آن‌هاست.

## مخازن و مبنای کار

- Base: `C:\Base`، شاخهٔ `BaseDotNet10`، مبنای بررسی `b089a5f`؛ راهکار فعال `Base.sln` و سورس اصلی در `sec` است.
- Template: `A:\Project\Collections\MassoudKargar\BaseTemplatePakage`.
- پوشهٔ Traders محل شروع گفتگو بود؛ تغییرات اصلی در دو مخزن بالا انجام شده‌اند.
- تغییرات قدیمی شاخهٔ `upgrade/dotnet-10` مبنای شاخهٔ فعلی نیستند؛ شاخهٔ فعلی جداگانه بررسی و اصلاح شد.

## کارهای انجام‌شده

### Base و .NET 10

- [x] بررسی ۲۳ پروژهٔ راهکار فعال؛ هدف `net10.0` و نسخهٔ بسته‌ها `10.0.0` است.
- [x] رفع خطاهای Build مربوط به APIهای قدیمی OpenAPI و Swashbuckle.
- [x] Build نسخهٔ Release و بسته‌بندی موفق راهکار.
- [x] حفظ نام بسته‌ها، از جمله املای موجود در نام‌هایی مانند `Base.Extentsions.Queries.Abstractions` و `Base.Infrastructure.Files.MiniIo`.

### OpenAPI و Scalar

- [x] حذف وابستگی‌ها و مسیرهای تولید مستندات Swashbuckle/Swagger از پروژه‌های Endpoint فعال.
- [x] استفاده از `Microsoft.AspNetCore.OpenApi` و رابط `Scalar.AspNetCore`.
- [x] پشتیبانی از تنظیمات فعال/غیرفعال‌بودن مستندات، مسیرها، عنوان و نسخهٔ سند.
- [x] پشتیبانی از Bearer و OAuth2 Authorization Code/PKCE.
- [x] اعمال متادیتای صریح Authorize/AllowAnonymous و پاسخ‌های 401/403 در مستندات؛ سیاست عمومی fallback به‌صورت خودکار استنباط نمی‌شود.
- [x] حفظ منطق runtime مربوط به API Consumer در فضای نام `Consumers`.
- [x] تست HTTP مصرف‌کنندهٔ بستهٔ OpenAPI/Scalar: ۲۴ assertion موفق.

### ساختار و اجرای Template

- [x] انتقال لایهٔ `2.Infra` به `2.Infrastructure`.
- [x] اصلاح نام پروژه‌ها، namespaceها، migrationها، referenceها و پوشه‌های Solution متناسب با Infrastructure.
- [x] هماهنگ‌سازی وابستگی‌های Template با API و بسته‌های شاخهٔ فعلی BaseDotNet10.
- [x] تنظیم MediatR، اسکن اسمبلی ApplicationService و RequestLoggingBehavior.
- [x] تنظیم DbContext فرمان، DbContext فقط‌خواندنی پرس‌وجو و اشتراک یک UnitOfWork در scope بین رابط‌های مربوط.
- [x] اتصال ثبت سرویس‌ها، middleware، مدیریت خطا، OpenAPI/Scalar و JWT اختیاری.
- [x] حذف تنظیمات و decoratorهای قدیمیِ بدون مصرف.
- [x] اجرای migration فقط به‌صورت opt-in؛ در این کار migration واقعی اجرا نشده است.
- [x] Pack، نصب در hive مستقل، تولید TradingSmoke و Restore/Build با بسته‌های محلی شاخهٔ فعلی.
- [x] تست startup/DI/HTTP پروژهٔ تولیدشده: ۹ assertion موفق؛ مسیر قدیمی Swagger نیز بررسی شد و در دسترس نیست.

### Utility

چهار کتابخانهٔ زیر با حفظ نام بسته به `net10.0` و نسخهٔ `10.0.0` ارتقا یافتند و Build/Pack آن‌ها موفق شد:

- [x] `Base.Utility.Auth.ApiAuthentication`
- [x] `Base.Utility.OpenTelemetryRegistration`
- [x] `Base.Utility.SerilogRegistration`
- [x] `Base.Utility.SoftwarePartsDetector`

بستهٔ `Base.Utility` از قبل جزو ۲۳ پروژهٔ راهکار فعال بود. وابستگی‌های Microsoft نسخهٔ ۹ در چهار Utility به ۱۰ ارتقا یافتند. SoftwarePartsDetector از FrameworkReference مربوط به ASP.NET Core به‌جای MVC Core 2.2.5 استفاده می‌کند و using بدون مصرف Newtonsoft حذف شد.

وابستگی‌های منتشرشدهٔ قدیمی `Base.Extensions.* 9.0.0` برای سازگاری API این Utilityها همچنان از NuGet مصرف می‌شوند؛ سورس آن کتابخانه‌ها مجدداً بسته‌بندی نشده است. این وابستگی‌ها با هدف `net10.0` خود بسته‌های Utility متفاوت‌اند.

### اسکریپت بسته‌بندی

- [x] اصلاح `build-package.bat` در هر دو مخزن: اجرا از پوشه‌های دیگر، پشتیبانی مسیر دارای فاصله، انتقال آرگومان‌ها و برگرداندن exit code خطا.
- [x] ترجیح PowerShell 7 و fallback به Windows PowerShell؛ اصلاح مقدار پیش‌فرض پارامترها برای سازگاری PowerShell 5.
- [x] بسته‌بندی Base با ترتیب وابستگی، ثبت log و تولید `publish-manifest.csv` همراه SHA256.
- [x] استفاده از NuGet.Config محدود به ساخت برای دورزدن feed محلی خراب سیستم؛ تنظیمات عمومی NuGet تغییر نکردند.
- [x] افزودن چهار Utility ارتقایافته به اسکریپت و حفظ فیلتر net10.
- [x] خروج پروژه‌های Sample/Test، میزبان‌های اجرایی، Swagger Utility و کتابخانه‌های net8/net9 از انتخاب بسته‌بندی.
- [x] حذف خروجی‌های قدیمی net8/net9 تولیدشده در این دوره؛ سورس آن‌ها حفظ شده است. اسکریپت فعلی کل پوشهٔ خروجی را پاک نمی‌کند.
- [x] جداکردن Pack معمول Template از اعتبارسنجی اختیاری تولید پروژه با `-Verify`.
- [x] بررسی محتوای چهار بستهٔ Utility و تأیید DLL در `lib/net10.0`.

## تعداد بسته‌ها و اختلاف ۲۷، ۴۹ و ۵۱

| گروه | تعداد | وضعیت |
| --- | ---: | --- |
| پروژه‌های راهکار فعال Base | ۲۳ | net10؛ ساخته و بسته‌بندی شده |
| Utilityهای اضافه‌شده | ۴ | net10؛ ساخته و بسته‌بندی شده |
| جمع خروجی فعلی Base | **۲۷** | بستهٔ نسخهٔ 10.0.0 |
| بستهٔ مستقل Template | ۱ | خروجی اسکریپت مخزن Template |
| جمع فهرست انتشار فعلی با Template | **۲۸** | انتشار عمومی انجام نشده |
| Extensionهای قدیمی با نام غیرتکراری نسبت به راهکار فعال | ۲۴ | هنوز net8/net9؛ خارج از خروجی |
| شمارش تاریخی با افزودن پوشهٔ قدیمی | **۵۱** | کنار گذاشته شد؛ هدف انتشار فعلی نیست |

در پوشهٔ قدیمی `Extentsions` تعداد ۳۱ کتابخانهٔ غیر Sample/Test وجود دارد: ۷ نام با پروژه‌های راهکار فعال هم‌پوشانی دارند و ۲۴ نام دیگر یکتا هستند. بنابراین با شمارش فعلی، عدد ۴۹ تأیید نشده است؛ عدد ۵۱ نیز به معنی آماده‌بودن آن بسته‌ها نیست.

## کارهای باقی‌مانده یا انجام‌نشده

- [x] کنارگذاشتن ارتقای پوشهٔ قدیمی Extentsions طبق تصمیم نهایی کاربر؛ قابلیت‌ها در Base پیاده شده‌اند و این پوشه دیگر جزو کارهای باقی‌مانده نیست.
- [ ] بررسی جایگزینی وابستگی‌های Base.Extensions نسخهٔ ۹ در Utilityها با APIهای نسخهٔ ۱۰؛ احتمال نیاز به تغییر قراردادها وجود دارد.
- [ ] رفع هشدارهای امنیتی وابستگی‌های موجود، از جمله Magick.NET، Pkcs و OpenTelemetry؛ این کار به‌طور کامل انجام نشده است.
- [ ] ارتقا و هماهنگ‌سازی خانوادهٔ وابستگی‌های OpenTelemetry و آزمون collector واقعی.
- [ ] بررسی و رفع همهٔ nullable/analyzer warningها؛ Build موفق به معنی صفر بودن warningها نیست.
- [ ] آزمون یکپارچه با SQL Server، Redis، Kafka، MinIO، OAuth، logging sinkها و telemetry واقعی.
- [ ] اجرای migration پایگاه دادهٔ واقعی؛ عمداً اجرا نشده است.
- [ ] بررسی مهاجرت مصرف‌کنندگان APIهای حذف‌شدهٔ Swagger به transformerهای OpenAPI بومی.
- [ ] اعمال کنترل‌های CI برای ساخت، تست، بررسی بسته‌ها و امنیت.
- [ ] Commit تغییرات فعلی در هر دو مخزن پس از بازبینی؛ تغییرات فعلی هنوز در working tree هستند.
- [ ] Push و انتشار بسته‌ها روی NuGet.org؛ هیچ انتشار عمومی انجام نشده است.

### مواردی که طبق درخواست خارج از دامنه می‌مانند

- Swagger/Swashbuckle و بستهٔ `Base.Utility.Swagger.Registration`.
- بسته‌بندی Sample endpointها و پروژه‌های اجرایی نمونه.
- انتشار مجدد نسخه‌های قدیمی ۸/۹ یا حذف سورس آن‌ها.

## فهرست تاریخی Extensionهای قدیمی — خارج از دامنهٔ فعلی
- [ ] `Base.Extensions.BackgroundWorker.KafkaConsumer` — `net8.0`
- [ ] `Base.Extensions.BackgroundWorker.KafkaProducer` — `net8.0`
- [ ] `Base.Extensions.Caching.Distributed.Redis` — `net9.0`
- [ ] `Base.Extensions.Caching.Distributed.Sql` — `net9.0`
- [ ] `Base.Extensions.Caching.InMemory` — `net9.0`
- [ ] `Base.Extensions.ChangeDataLog` — `net9.0`
- [ ] `Base.Extensions.ChangeDataLog.Sql` — `net9.0`
- [ ] `Base.Extensions.DependencyInjection` — `net9.0`
- [ ] `Base.Extensions.Events.Abstractions` — `net9.0`
- [ ] `Base.Extensions.Events.Outbox.Dal.EF` — `net9.0`
- [ ] `Base.Extensions.Events.PollingPublisher` — `net9.0`
- [ ] `Base.Extensions.Events.PollingPublisher.Dal.Dapper` — `net9.0`
- [ ] `Base.Extensions.MessageBus.Abstractions` — `net9.0`
- [ ] `Base.Extensions.MessageBus.MessageInbox` — `net9.0`
- [ ] `Base.Extensions.MessageBus.MessageInbox.Dal.Dapper` — `net9.0`
- [ ] `Base.Extensions.MessageBus.RabbitMQ` — `net9.0`
- [ ] `Base.Extensions.ObjectMappers.Abstractions` — `net9.0`
- [ ] `Base.Extensions.ObjectMappers.AutoMapper` — `net8.0`
- [ ] `Base.Extensions.Serializers.EPPlus` — `net9.0`
- [ ] `Base.Extensions.Serializers.Microsoft` — `net9.0`
- [ ] `Base.Extensions.Serializers.NewtonSoft` — `net9.0`
- [ ] `Base.Extensions.Translations.Parrot` — `net9.0`
- [ ] `Base.Extensions.UsersManagement` — `net9.0`
- [ ] `Base.Extensions.UsersManagement.Abstractions` — `net9.0`

## مسیر خروجی و دستورات تکرار

خروجی Base: `C:\Base\artifacts\branch-nuget`.

```powershell
# در C:\Base
.\build-package.bat -ListOnly
.\build-package.bat
```

خروجی پیش‌فرض Template: `A:\Project\Collections\MassoudKargar\BaseTemplatePakage\artifacts\nuget`.

```powershell
# در مخزن BaseTemplatePakage
.\build-package.bat
.\build-package.bat -Verify -BasePackageDirectory C:\Base\artifacts\branch-nuget
```

برای انتشار از manifest استفاده شود؛ وجود فایل قدیمی در پوشهٔ خروجی به معنی انتخاب آن برای انتشار نیست. اسکریپت‌ها خودشان NuGet push انجام نمی‌دهند.

## مستندات تکمیلی

- [گزارش فنی و محدودیت‌های اعتبارسنجی](docs/BASEDOTNET10-AUDIT.md)
- [روش استفاده از اسکریپت‌های بسته‌بندی](docs/BUILD-PACKAGES.md)
- [فهرست بسته‌های انتشار](docs/NUGET-PUBLISH-LIST.md)
- [Manifest انتشار و SHA256](docs/NUGET-PUBLISH-MANIFEST.csv)

Commitهای قبلی این دوره: `166b08d` در شاخهٔ قدیمی Base و `e4a85e1` در Template. این‌ها شامل تمام تغییرات بعدی شاخهٔ فعلی و Utilityها نیستند.