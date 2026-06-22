using FlashCards.Models;

namespace FlashCards.Services;

/// <summary>How well the user recalled a card. Values map to SM-2 quality grades.</summary>
public enum ReviewGrade
{
    Again = 0, // complete blackout
    Hard = 3,  // correct, but with serious difficulty
    Good = 4,  // correct after some hesitation
    Easy = 5   // perfect recall
}

/// <summary>
/// Classic SM-2 spaced-repetition scheduler.
/// See https://super-memory.com/english/ol/sm2.htm
/// </summary>
public static class Sm2Scheduler
{
    public static void Apply(Card card, ReviewGrade grade, DateTime nowUtc)
    {
        int q = (int)grade;

        if (q < 3)
        {
            // Failed recall: restart the repetition cycle, review again tomorrow.
            card.Repetitions = 0;
            card.IntervalDays = 1;
        }
        else
        {
            card.Repetitions += 1;
            card.IntervalDays = card.Repetitions switch
            {
                1 => 1,
                2 => 6,
                _ => (int)Math.Round(card.IntervalDays * card.EaseFactor)
            };
        }

        // Ease factor is always updated, then floored at 1.3.
        double ef = card.EaseFactor + (0.1 - (5 - q) * (0.08 + (5 - q) * 0.02));
        card.EaseFactor = Math.Max(1.3, ef);

        card.DueDate = nowUtc.Date.AddDays(card.IntervalDays);
        card.LastReviewedUtc = nowUtc;
    }

    /// <summary>
    /// Returns the interval (in days) a given grade would produce, without
    /// mutating the card. Used to label the review buttons.
    /// </summary>
    public static int PreviewIntervalDays(Card card, ReviewGrade grade)
    {
        int q = (int)grade;
        if (q < 3) return 1;

        int reps = card.Repetitions + 1;
        return reps switch
        {
            1 => 1,
            2 => 6,
            _ => (int)Math.Round(card.IntervalDays * card.EaseFactor)
        };
    }
}
