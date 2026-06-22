# FlashCards

A mobile-friendly flashcard app built with **Blazor WebAssembly** as an installable **PWA**. Create decks, study with flippable cards, and review on a schedule driven by the **SM-2 spaced-repetition** algorithm. All data is stored locally in the browser, so it works offline and survives restarts.

## Features

- **Decks & cards CRUD** — create/delete decks and add/remove cards.
- **Study mode** — tap a card to flip between front and back.
- **Spaced repetition** — grade each card (Again / Hard / Good / Easy); the SM-2 scheduler decides when you see it next.
- **Local persistence** — everything is saved to `localStorage` (no account, no server).
- **Installable PWA** — add it to your phone's home screen and use it offline.

## Project layout

```
FlashCards.csproj          Blazor WebAssembly PWA project
Program.cs                 DI registration + app startup
Models/
  Card.cs                  Flashcard + SM-2 state
  Deck.cs                  Named collection of cards
Services/
  Sm2Scheduler.cs          SM-2 spaced-repetition algorithm
  LocalStorage.cs          Wrapper over browser localStorage
  DeckStore.cs             Loads/saves decks, source of truth
Pages/
  Home.razor               Deck list + create deck
  DeckEditor.razor         Manage a deck's cards
  Study.razor              Flip-card study session with grading
Layout/MainLayout.razor    App shell
tests/FlashCards.Tests/    xUnit tests for the SM-2 scheduler
```

## Running locally

Requires the **.NET 8 SDK**.

```bash
# Run the app (then open the printed http://localhost:5xxx URL)
dotnet run -c Release

# Run the unit tests
dotnet test tests/FlashCards.Tests/FlashCards.Tests.csproj

# Produce a publishable static site (output in bin/Release/net8.0/publish/wwwroot)
dotnet publish -c Release
```

Because it's a standalone Blazor WebAssembly app, the published `wwwroot` is just
static files — host it on any static host (GitHub Pages, Netlify, Azure Static
Web Apps, etc.).

## How scheduling works

Each review grade maps to an SM-2 quality score:

| Button | Quality | Effect |
|--------|---------|--------|
| Again  | 0 | Reset the card; review again tomorrow |
| Hard   | 3 | Advance, smaller ease increase |
| Good   | 4 | Advance normally |
| Easy   | 5 | Advance, larger ease increase |

New cards step through intervals of 1 day, then 6 days, then `interval × ease
factor`. The ease factor starts at 2.5 and is floored at 1.3. "Again" cards
re-appear within the same study session.
