namespace FlashCards.Models;

/// <summary>
/// A single flashcard plus the SM-2 spaced-repetition state used to schedule it.
/// </summary>
public class Card
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Front { get; set; } = "";
    public string Back { get; set; } = "";

    // --- SM-2 scheduling state ---
    /// <summary>Ease factor; starts at 2.5 and never drops below 1.3.</summary>
    public double EaseFactor { get; set; } = 2.5;

    /// <summary>Number of consecutive successful reviews.</summary>
    public int Repetitions { get; set; }

    /// <summary>Current review interval, in days.</summary>
    public int IntervalDays { get; set; }

    /// <summary>Date (UTC) the card next becomes due for review.</summary>
    public DateTime DueDate { get; set; } = DateTime.UtcNow.Date;

    public DateTime? LastReviewedUtc { get; set; }

    /// <summary>True once the card has been reviewed at least once.</summary>
    public bool IsNew => LastReviewedUtc is null;

    public bool IsDue(DateTime nowUtc) => DueDate.Date <= nowUtc.Date;
}
