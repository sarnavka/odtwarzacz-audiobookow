# Odtwarzacz Audiobooków

Desktopowy odtwarzacz audiobooków dla Windows z kontrolą prędkości bez zmiany tonu głosu, podkładem ambient i synchronizacją pozycji między urządzeniami przez sieć LAN — zbudowany w C# WinForms (.NET 6), z NAudio do odtwarzania i SoundTouch do przetwarzania dźwięku.

---

## Funkcje

**Odtwarzanie**
- Obsługa formatów: MP3, M4A, M4B, WAV, FLAC, OGG, WMA
- Zmiana prędkości w zakresie 0.5x–3.0x **bez zmiany wysokości głosu** (SoundTouch)
- Przewijanie o konfigurowalną liczbę sekund (domyślnie ±30s)
- Automatyczne wznawianie ostatniego audiobooka po uruchomieniu

**Biblioteka**
- Import pojedynczych plików lub całych folderów (rekurencyjnie)
- Automatyczna ekstrakcja metadanych — tytuł, autor, czas trwania, okładka (TagLib#)
- Sortowanie po tytule, autorze, dacie odtworzenia lub czasie trwania
- Wyszukiwanie w czasie rzeczywistym (z debounce 300ms)
- Śledzenie postępu i zapamiętywanie pozycji dla każdej książki osobno

**Podkład dźwiękowy**
- Niezależny odtwarzacz z osobną kontrolą głośności
- 5 wbudowanych dźwięków ambient: Deszcz, Las, Kominek, Ocean, Kawiarnia
- Możliwość załadowania własnego pliku audio jako tła

**Synchronizacja sieciowa (LAN)**
- Synchronizacja pozycji odtwarzania między urządzeniami w sieci lokalnej
- Architektura REQUEST-RESPONSE oparta na NetMQ (ZeroMQ dla .NET)
- Tryb serwera i tryb klienta — jeden komputer prowadzi, pozostałe się synchronizują
- Automatyczne wykrywanie rozbieżności pozycji (>5s) i propozycja synchronizacji

**System rozdziałów**
- Automatyczne generowanie rozdziałów co 30 minut dla długich plików
- Nawigacja przez dwuklik w liście rozdziałów

**Inne**
- Integracja z zasobnikiem systemowym Windows (minimalizacja do tray, menu kontekstowe)
- Pełna obsługa skrótów klawiszowych
- Ustawienia serializowane do `settings.json`

---

## Skróty klawiszowe

| Klawisz | Akcja |
|---|---|
| `Spacja` | Odtwarzaj / Pauza |
| `Escape` | Zatrzymaj |
| `←` / `→` | Przewiń wstecz / do przodu |
| `↑` / `↓` | Zwiększ / zmniejsz prędkość |
| `Ctrl+↑` / `Ctrl+↓` | Zwiększ / zmniejsz głośność |
| `M` | Włącz / wyłącz muzykę tła |

---

## Stos technologiczny

| Warstwa | Technologia |
|---|---|
| Platforma | .NET 6 Windows Forms |
| Odtwarzanie audio | NAudio |
| Kontrola prędkości | SoundTouch (.NET binding) |
| Metadane plików | TagLib# |
| Synchronizacja LAN | NetMQ (ZeroMQ) |
| Serializacja | System.Text.Json |

---

## Wymagania

- Windows 10 / 11
- .NET 6 Runtime

---

## Uruchomienie

1. Sklonuj repozytorium
2. Otwórz `Odtwarzacz audiobookow.sln` w Visual Studio 2022
3. Zbuduj projekt (`Ctrl+Shift+B`)
4. Uruchom (`F5`)

Ustawienia i biblioteka są zapisywane w `settings.json` obok pliku `.exe`.

---

## Synchronizacja sieciowa — konfiguracja

**Serwer** — komputer główny:
1. Zaznacz „Tryb serwera"
2. Ustaw port (domyślnie 5555)
3. Kliknij „Zastosuj" i włącz synchronizację

**Klient** — pozostałe urządzenia:
1. Wpisz adres IP serwera
2. Wpisz ten sam port
3. Kliknij „Zastosuj" i włącz synchronizację

Pozycje są synchronizowane co 2 sekundy. Przy różnicy większej niż 5 sekund aplikacja zaproponuje wyrównanie pozycji.
