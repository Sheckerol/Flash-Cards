using System.Text.Json;
using FlashCards.Models;

namespace FlashCards.Services;

/// <summary>
/// In-memory collection of decks, persisted to localStorage as JSON.
/// Acts as the single source of truth for the app (registered as a singleton).
/// </summary>
public class DeckStore
{
    private const string StorageKey = "flashcards.decks.v1";
    private const string SeededStartersKey = "flashcards.starters.v1";

    private readonly LocalStorage _storage;
    private bool _loaded;

    public List<Deck> Decks { get; private set; } = new();

    /// <summary>Raised whenever the deck collection changes and has been saved.</summary>
    public event Action? OnChange;

    public DeckStore(LocalStorage storage) => _storage = storage;

    /// <summary>Loads decks from storage. Safe to call repeatedly; only loads once.</summary>
    public async Task InitAsync()
    {
        if (_loaded) return;

        var json = await _storage.GetAsync(StorageKey);
        if (!string.IsNullOrWhiteSpace(json))
        {
            try
            {
                Decks = JsonSerializer.Deserialize<List<Deck>>(json) ?? new();
            }
            catch
            {
                Decks = new();
            }
        }

        await EnsureStarterDecksAsync();

        _loaded = true;
    }

    /// <summary>
    /// Adds any built-in starter deck that hasn't been seeded yet. Each starter
    /// is seeded at most once (tracked by id), so deleting one won't bring it back.
    /// </summary>
    private async Task EnsureStarterDecksAsync()
    {
        var seeded = await LoadSeededStarterIdsAsync();
        var changed = false;

        foreach (var starter in StarterDecks.All())
        {
            if (seeded.Contains(starter.Id))
                continue;

            // Only add if the user doesn't already have a deck with this id.
            if (!Decks.Any(d => d.Id == starter.Id))
                Decks.Add(starter);

            seeded.Add(starter.Id);
            changed = true;
        }

        if (changed)
        {
            await _storage.SetAsync(SeededStartersKey, JsonSerializer.Serialize(seeded));
            await SaveAsync();
        }
    }

    private async Task<HashSet<Guid>> LoadSeededStarterIdsAsync()
    {
        var json = await _storage.GetAsync(SeededStartersKey);
        if (string.IsNullOrWhiteSpace(json))
            return new HashSet<Guid>();

        try
        {
            return JsonSerializer.Deserialize<HashSet<Guid>>(json) ?? new HashSet<Guid>();
        }
        catch
        {
            return new HashSet<Guid>();
        }
    }

    public Deck? GetDeck(Guid id) => Decks.FirstOrDefault(d => d.Id == id);

    public async Task<Deck> AddDeckAsync(string name, string description)
    {
        var deck = new Deck { Name = name.Trim(), Description = (description ?? "").Trim() };
        Decks.Add(deck);
        await SaveAsync();
        return deck;
    }

    public async Task DeleteDeckAsync(Guid id)
    {
        Decks.RemoveAll(d => d.Id == id);
        await SaveAsync();
    }

    public async Task AddCardAsync(Deck deck, string front, string back)
    {
        deck.Cards.Add(new Card { Front = front.Trim(), Back = back.Trim() });
        await SaveAsync();
    }

    public async Task DeleteCardAsync(Deck deck, Guid cardId)
    {
        deck.Cards.RemoveAll(c => c.Id == cardId);
        await SaveAsync();
    }

    /// <summary>Persists the current state and notifies listeners.</summary>
    public async Task SaveAsync()
    {
        var json = JsonSerializer.Serialize(Decks);
        await _storage.SetAsync(StorageKey, json);
        OnChange?.Invoke();
    }

    public static IEnumerable<Card> DueCards(Deck deck, DateTime nowUtc) =>
        deck.Cards.Where(c => c.IsDue(nowUtc));
}
