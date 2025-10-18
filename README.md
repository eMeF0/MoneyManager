# MoneyManager (WIP)

Aplikacja webowa do zarządzania finansami osobistymi: śledzenie transakcji (przychody/wydatki), kont, kategorii, budżetów oraz szybkie podsumowania na pulpicie.

> **Status**: projekt w trakcie tworzenia (MVP). Część funkcjonalności jest gotowa, część w budowie.

---

## Technologie
- **.NET 8 / ASP.NET Core MVC**
- **Entity Framework Core** (SQLite)
- **Bootstrap 5 + Bootstrap Icons**
- Minimalny front-end własny
  
---

## Funkcjonalności

### Pulpit (Dashboard)
- KPI: bilans całkowity, zmiana m/m, przychody i wydatki bieżącego miesiąca.
- Ostatnie transakcje, skróty nawigacyjne.
- Podstawowe wykresy (payloady pod wykresy słupkowe/kołowe w `ViewModels.Common`).

### Transakcje
- Dodawanie/edycja/usuwanie transakcji (przychód/wydatek).
- Filtrowanie: konto, kategoria, zakres dat.
- Sortowanie: po dacie i kwocie (rosnąco/malejąco).
- Paginacja (util: `Utilities/PaginatedList`).

### Kategorie
- CRUD kategorii (nazwa/opis).
- Przypisanie transakcji do kategorii.

### Konta
- CRUD kont (nazwa, saldo).
- Widok szczegółów z listą transakcji danego konta + filtrowanie/sortowanie.

### Budżety
- Budżety powiązane z kontami i kategoriami (limit, okres).
- Ekran alokacji i podgląd dostępności limitów (WIP).

### Raporty
- Zagregowane zestawienia miesięczne (przychody/wydatki/saldo).
- Zestawienie wydatków wg kategorii (payload pod wykres kołowy).

---
