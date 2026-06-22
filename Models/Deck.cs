namespace FlashCards.Models;

/// <summary>A named collection of flashcards.</summary>
public class Deck
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public List<Card> Cards { get; set; } = new();

    public int DueCount(DateTime nowUtc) => Cards.Count(c => c.IsDue(nowUtc));
}
