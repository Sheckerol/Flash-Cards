using FlashCards.Models;
using FlashCards.Services;
using Xunit;

namespace FlashCards.Tests;

public class Sm2SchedulerTests
{
    private static readonly DateTime Now = new(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void FirstGoodReview_SchedulesOneDayOut()
    {
        var card = new Card();

        Sm2Scheduler.Apply(card, ReviewGrade.Good, Now);

        Assert.Equal(1, card.Repetitions);
        Assert.Equal(1, card.IntervalDays);
        Assert.Equal(Now.Date.AddDays(1), card.DueDate);
        Assert.False(card.IsNew);
    }

    [Fact]
    public void SecondGoodReview_SchedulesSixDaysOut()
    {
        var card = new Card();

        Sm2Scheduler.Apply(card, ReviewGrade.Good, Now);
        Sm2Scheduler.Apply(card, ReviewGrade.Good, Now.AddDays(1));

        Assert.Equal(2, card.Repetitions);
        Assert.Equal(6, card.IntervalDays);
    }

    [Fact]
    public void ThirdReview_IntervalScalesByEaseFactor()
    {
        var card = new Card();

        Sm2Scheduler.Apply(card, ReviewGrade.Good, Now);            // interval 1
        Sm2Scheduler.Apply(card, ReviewGrade.Good, Now.AddDays(1)); // interval 6
        Sm2Scheduler.Apply(card, ReviewGrade.Good, Now.AddDays(7)); // interval = round(6 * EF)

        // After two "Good" (q=4) reviews EF stays at 2.5, so 6 * 2.5 = 15.
        Assert.Equal(3, card.Repetitions);
        Assert.Equal(15, card.IntervalDays);
    }

    [Fact]
    public void AgainGrade_ResetsRepetitionsAndIntervalToOne()
    {
        var card = new Card();
        Sm2Scheduler.Apply(card, ReviewGrade.Good, Now);
        Sm2Scheduler.Apply(card, ReviewGrade.Good, Now.AddDays(1));

        Sm2Scheduler.Apply(card, ReviewGrade.Again, Now.AddDays(7));

        Assert.Equal(0, card.Repetitions);
        Assert.Equal(1, card.IntervalDays);
        Assert.Equal(Now.AddDays(7).Date.AddDays(1), card.DueDate);
    }

    [Fact]
    public void EaseFactor_NeverDropsBelowFloor()
    {
        var card = new Card();

        // Repeated failures should push EF down to, but not below, 1.3.
        for (int i = 0; i < 10; i++)
        {
            Sm2Scheduler.Apply(card, ReviewGrade.Again, Now.AddDays(i));
        }

        Assert.True(card.EaseFactor >= 1.3);
    }

    [Fact]
    public void EasyGrade_IncreasesEaseFactor()
    {
        var card = new Card();

        Sm2Scheduler.Apply(card, ReviewGrade.Easy, Now);

        Assert.True(card.EaseFactor > 2.5);
    }

    [Theory]
    [InlineData(ReviewGrade.Again, 1)]
    [InlineData(ReviewGrade.Good, 1)]
    [InlineData(ReviewGrade.Easy, 1)]
    public void PreviewInterval_MatchesAppliedInterval_ForNewCard(ReviewGrade grade, int expected)
    {
        var card = new Card();

        int preview = Sm2Scheduler.PreviewIntervalDays(card, grade);
        Sm2Scheduler.Apply(card, grade, Now);

        Assert.Equal(expected, preview);
        Assert.Equal(card.IntervalDays, preview);
    }

    [Fact]
    public void NewCard_IsDueImmediately()
    {
        var card = new Card();
        // A freshly created card defaults its due date to the creation date,
        // so it should be due as of "now".
        Assert.True(card.IsDue(DateTime.UtcNow));
    }
}
