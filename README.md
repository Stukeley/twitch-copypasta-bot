# twitch-copypasta-bot
EN This bot automatically collects copypastas in Twitch chat and saves them for future use. For now Polish language only - English version soon.  
PL Ten bot automatycznie kolekcjonuje copypasty na Twitch czacie i zapisuje je na przyszły użytek. KEKW  

# **HEAVY WIP**
Prace nadal trwają. Póki co aplikacja wygląda jak kupa i działa niewiele lepiej, ale powoli dążymy do celu.  

# Funkcje
- Bot na życzenie użytkownika dołącza do wybranego kanału i zapisuje wiadomości, a następnie "wyciąga" z nich ewentualne Copypasty
- Konfiguracja: nazwa kanału, co ile wiadomości ma odbywać się ewaluacja i ile takich samych wiadomości jest wymagane aby uznać wiadomość za copypastę (większy kanał -> większe te wartości) (soon)
- Wymagany jest plik z danymi dostępu do bota - instrukcja wkrótce!
- Interfejs graficzny - przeglądanie Copypast, edycja, dodawanie tytułów, oznaczanie jako ulubione, konfiguracja wewnątrz aplikacji (soon)
- Domyślny zapis copypast do bazy danych, na życzenie użytkownika również do pliku txt
- Czarna Lista wiadomości (np. emotek) - czyli takich, które nie będą rejestrowane jako Copypasty
- Statystyki - ile past w bazie, daty dodania, ilość past bez tytułu (tytuł nie jest wymagany, jednak jest mocno zalecany)
- Obsługa Loggingu - dla przeciętnego użytkownika niezbyt istotna sprawa, ale istnieje opcja podglądu praktycznie wszystkiego co dzieje się w aplikacji
- Idealne, jeśli chat Twojego ulubionego streamera zalany jest copypastami których nie chce ci się ręcznie zapisywać, oraz do których chcesz mieć szybki dostęp - aby spamować je z innymi użytkownikami!

# Instrukcja
Soon!

# TODO
A lot.

# Specyfikacja Techniczna
1. .NET Framework 4.8
2. WPF & Material Design http://materialdesigninxaml.net/)
3. Entity Framework 6 (podejście Code First)
4. TwitchLib (https://github.com/TwitchLib/TwitchLib)
