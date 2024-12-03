# Flight Booking System

## Opis projektu
**FlightBookingSystem** to implementacja modelu domenowego systemu sprzedaży biletów lotniczych zgodnego z zasadami **SOLID**. Projekt został przygotowany z myślą o umożliwieniu łatwej rozbudowy oraz testowaniu z użyciem podejścia **TDD**. Nie zawiera warstw API ani infrastruktury, zgodnie z wymaganiami zadania.

System obsługuje takie funkcjonalności, jak:
- Dodawanie lotów.
- Zakup biletów na loty z dynamicznym obliczaniem ceny.
- Zniżki na podstawie elastycznych kryteriów.

---

## Struktura projektu
Projekt został podzielony na dwa główne komponenty:
1. **FlightBooking.Domain** – implementacja logiki domenowej.
   - **Models/** – klasy reprezentujące model domenowy (np. `Flight`, `Tenant`).
   - **Services/** – implementacje logiki biznesowej, np. `DiscountService`.
   - **Interfaces/** – interfejsy umożliwiające rozbudowę systemu (np. `IDiscountCriteria`).
   - **Enums/** – definicje typów wyliczeniowych, np. `TenantGroup`.

2. **FlightBooking.Tests** – projekt testów jednostkowych oparty na **NUnit**.
   - Testy są zgodne z podejściem **TDD** i pokrywają kluczowe funkcjonalności systemu.

---

## Wymagania systemowe
Aby uruchomić projekt, wymagane są:
- .NET 8.0
- Visual Studio 2022 (lub dowolne IDE obsługujące .NET)

---

## Uruchomienie lokalne
1. **Sklonuj repozytorium**:
```bash
git clone https://github.com/kademat/FlightBookingSystem.git
```
2. **Otwórz projekt w Visual Studio.**
3. **Zbuduj projekt (Build > Build Solution).**
4. **Uruchom testy jednostkowe:**
	- **Otwórz Test Explorer.**
	- **Kliknij Run All.**

---

## Testy
Testy jednostkowe znajdują się w projekcie `FlightBooking.Tests`. Pokrywają one kluczowe elementy systemu, w tym:

- Zniżki dla biletów (np. urodzinowe, dla lotów do Afryki w czwartki).
- Ograniczenia minimalnej ceny biletu.
- Brak zniżek, gdy kryteria nie są spełnione.
Testy zostały napisane w oparciu o NUnit i można je uruchomić za pomocą Visual Studio lub ```dotnet test``` w terminalu.

---

## Plan rozwoju
- [x] Implementacja podstawowego modelu domenowego wraz z podstawowymi testami jednostkowymi.

Cel kroku:

Zaprezentowanie struktury projektu, pierwszego pomysłu na realizację zadania, przygotowanie do dalszego rozwoju.

- [x] Dodanie nowych kryteriów zniżek poprzez implementację interfejsu IDiscountCriteria.

Cel kroku:

Rozszerzenie logiki biznesowej poprzez umożliwienie łatwego dodawania kolejnych reguł zniżek zgodnie z zasadą Open/Closed.

- [x] Rozszerzenie funkcjonalności zarządzania lotami.

Cel kroku:

Dodanie możliwości aktualizacji, usuwania lotów oraz bardziej zaawansowanego wyszukiwania lotów w systemie.

- [ ] Przygotowanie kodu umożliwiającego łatwą integrację z API lub bazą danych.

Cel kroku:

Zapewnienie spójnej struktury i interfejsów, które ułatwią dodanie infrastruktury w przyszłości.

- [ ] Rozbudowa modelu domenowego o walidację danych wejściowych.

Cel kroku:

Wprowadzenie walidacji na poziomie domeny, np. poprawności ID lotu, dat i godzin.

- [ ] Implementacja mechanizmu logowania dla kryteriów zniżek (dla tenantów grupy A).

Cel kroku:

Wprowadzenie rejestracji zastosowanych kryteriów zniżek z opcją łatwego rozszerzenia logiki dla przyszłych wymagań.

- [ ] Usprawnienie zarządzania cenami lotów.

Cel kroku:

Wprowadzenie dynamicznych reguł ustalania cen, np. na podstawie popularności lotu, sezonowości lub innych czynników.

- [ ] Optymalizacja systemu pod kątem wydajności.

Cel kroku:

Analiza i eliminacja potencjalnych wąskich gardeł w logice biznesowej, np. przetwarzania dużych zbiorów lotów i zniżek.

---

## Zastosowane wzorce projektowe
- **Repository Pattern**: choć nie zaimplementowano infrastruktury, model domenowy jest gotowy do integracji z repozytorium danych.
- **Strategy Pattern**: logika zniżek jest oparta na strategii, co ułatwia dodawanie nowych kryteriów.
- **Dependency Injection (DI)**: klasy usługowe są zaprojektowane tak, aby wspierały wstrzykiwanie zależności.

---

## Zasady SOLID
- **Single Responsibility**: Każda klasa ma jedno odpowiedzialne zadanie (np. Flight reprezentuje dane lotu, DiscountService obsługuje logikę zniżek).
- **Open/Closed**: Kryteria zniżek można łatwo rozszerzać bez modyfikacji istniejącego kodu.
- **Liskov Substitution**: Każda implementacja interfejsu IDiscountCriteria może być użyta zamiennie.
- **Interface Segregation**: Interfejsy są podzielone na konkretne role.
- **Dependency Inversion**: Usługi operują na abstrakcjach (interfejsy) zamiast na implementacjach.

---

## Uwagi dla osoby sprawdzającej

- Zniżki działają per kryterium i mogą się kumulować. Cena biletu nie spada jednak poniżej 20 euro.
- Kryteria zniżek można dodawać poprzez implementację interfejsu IDiscountCriteria.
- System przewiduje różnice w funkcjonalności dla tenantów grup A i B:
Dla grupy A zapisywane są zastosowane kryteria zniżek.
Dla grupy B kryteria te nie są zapisywane.

---

## Jak dodać nowe kryterium zniżek?
Dodaj nową klasę, która implementuje IDiscountCriteria.
Zaimplementuj logikę metody IsApplicable oraz GetDiscountAmount.
Zarejestruj nowe kryterium w odpowiedniej klasie (np. DiscountService).
Przykład nowego kryterium:

```csharp
public class WeekendDiscount : IDiscountCriteria
{
    public bool IsApplicable(Flight flight, DateTime purchaseDate, DateTime? buyerBirthDate)
    {
        return flight.DepartureTime.DayOfWeek == DayOfWeek.Saturday || flight.DepartureTime.DayOfWeek == DayOfWeek.Sunday;
    }

    public decimal GetDiscountAmount() => 5m;
}
```
