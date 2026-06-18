# TechRegression

> Najnowsze wieści ze świata technologii, które sprawiają, że chcesz powrócić do epoki brązu

TechRegression to aplikacja webowa typu CMS/blog stworzona w architekturze MVC.  
System pozwala na publikację artykułów, kategoryzację treści, dynamiczne filtrowanie, wyszukiwanie oraz moderację treści.

## Architektura

### Backend
* **Platforma:** .NET 8.0 (ASP.NET Core MVC)
* **Zarządzanie stanem:** ASP.NET Core Session State (autoryzacja sesyjna)

### Baza danych
* **ORM:** Entity Framework Core (Code First)
* **Silnik bazy danych:** Microsoft SQL Server (LocalDB)

### Frontend
* **Szkielet:** HTML5, Razor Views (`.cshtml`)
* **Style:** CSS3 (flexbox, grid, RWD)
* **Skrypty:** vanilla JavaScript (filtrowanie, przycisk back-to-top)

## Funkcjonalność
* **Publiczny feed wiadomości:** Dynamiczna siatka artykułów z filtrowaniem oraz wyszukiwaniem.
* **System komentarzy:** Możliwość dyskutowania pod artykułami przez użytkowników.
* **Panel administratora:** Autoryzacja sesyjna.
* **CRUD artykułów:** Tworzenie, edytowanie, usuwanie wpisów wraz z uploadem obrazów.
* **Zarządzanie kategoriami:** Dodawanie, edytowanie, usuwanie kategorii artykułów.
* **Moderacja komentarzy:** Możliwość usuwania komentarzy użytkowników przez administratorów.

## Instrukcja uruchomienia lokalnego

### 1. Wymagania wstępne
* Zalecane Visual Studio
* .NET 8.0
* Microsoft SQL Server / LocalDB (instalowany automatycznie z Visual Studio)

### 2. Klonowanie repozytorium

```bash
git clone https://github.com/SzymonMoskwa/TechRegression.git
cd TechRegression
```

### 3. Konfiguracja połączenia i konta administratora
Otwórz plik `appsettings.json` i zweryfikuj parametry konfiguracyjne

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TechRegressionDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "AdminSettings": {
    "Username": "admin",
    "PasswordHash": "AQAAAAIAAYagAAAAEGEl5W3MWpLqvCImCHaHIkTZUc7ZujCo7VFOqKmSBIeToarD7c12N3h9xuQ/cNwSuA=="
  }
}
```

Domyślnymi danymi logowania są login: `admin` i hasło: `password`.  
Aby je zmienić, podmień w `appsettings.json` wartość w polu "Username" na wybrany login.  
Następnie, aby wygenerować hash hasła, użyj następującego kodu, np. w aplikacji konsolowej lub narzędziu online do hashowania .NET Identity:

```csharp
var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<object>();
string hash = hasher.HashPassword(null, "TwojeNoweHaslo");
```

Skopiuj wygenerowany ciąg znaków i podmień nim wartość w polu "PasswordHash" w `appsettings.json`.

### 4. Budowanie projektu i migracja bazy danych
Aplikacja posiada zaimplementowany mechanizm auto-seeding, co oznacza, że nie musisz ręcznie uruchamiać migracji w konsoli &ndash; w przypadku braku bazy danych, system sam ją utworzy oraz doda startową kategorię i artykuł przy pierwszym uruchomieniu.

Jeśli jednak wolisz zrobić to ręcznie przez terminal:

```bash
dotnet tool install --global dotnet-ef
dotnet ef database update
```

Lub w Visual Studio 2022:  
Wybierz Narzędzia -> Menedżer pakietów NuGet -> Konsola menedżera pakietów (Tools -> NuGet Package Manager -> Package Manager Console).  
Następnie w otwartej konsoli wpisz:

```powershell
Update-Database
```

### 5. Uruchomienie aplikacji
Terminal:

```bash
dotnet run
```

i otwórz wyświetlony lokalny URL w przeglądarce.

Lub w Visual Studio 2022:  
1. Na górnym pasku narzędzi upewnij się, że profil uruchamiania jest ustawiony na nazwę projektu lub `https` / `IIS Express`.
2. Kliknij zielony trójkąt Uruchom / Rozpocznij debugowanie (lub F5 na klawiaturze).
3. Visual Studio skompiluje kod, uruchomi serwer i otworzy twoją domyślną przeglądarkę pod właściwym adresem lokalnym.

---
