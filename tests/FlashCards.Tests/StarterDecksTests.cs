using FlashCards.Services;
using Xunit;

namespace FlashCards.Tests;

public class StarterDecksTests
{
    [Fact]
    public void ProvidesAllFourKanaDecks()
    {
        var decks = StarterDecks.All().ToList();

        Assert.Equal(4, decks.Count);
        Assert.Contains(decks, d => d.Id == StarterDecks.HiraganaId);
        Assert.Contains(decks, d => d.Id == StarterDecks.KatakanaId);
        Assert.Contains(decks, d => d.Id == StarterDecks.HiraganaExtendedId);
        Assert.Contains(decks, d => d.Id == StarterDecks.KatakanaExtendedId);
    }

    [Fact]
    public void BaseDecksHaveAll46BasicCharacters()
    {
        var baseIds = new[] { StarterDecks.HiraganaId, StarterDecks.KatakanaId };
        foreach (var deck in StarterDecks.All().Where(d => baseIds.Contains(d.Id)))
            Assert.Equal(46, deck.Cards.Count);
    }

    [Fact]
    public void ExtendedDecksCoverDakutenHandakutenAndCombos()
    {
        // 25 dakuten/handakuten + 33 yōon = 58 per script.
        var extendedIds = new[] { StarterDecks.HiraganaExtendedId, StarterDecks.KatakanaExtendedId };
        foreach (var deck in StarterDecks.All().Where(d => extendedIds.Contains(d.Id)))
            Assert.Equal(58, deck.Cards.Count);
    }

    [Fact]
    public void EveryCardHasFrontAndBack()
    {
        foreach (var deck in StarterDecks.All())
        {
            Assert.All(deck.Cards, c => Assert.False(string.IsNullOrWhiteSpace(c.Front)));
            Assert.All(deck.Cards, c => Assert.False(string.IsNullOrWhiteSpace(c.Back)));
        }
    }

    [Fact]
    public void KanaCharactersAreUniqueWithinEachDeck()
    {
        foreach (var deck in StarterDecks.All())
        {
            var fronts = deck.Cards.Select(c => c.Front).ToList();
            Assert.Equal(fronts.Count, fronts.Distinct().Count());
        }
    }

    [Fact]
    public void StarterDeckIdsAreStableAcrossCalls()
    {
        var first = StarterDecks.All().Select(d => d.Id).ToList();
        var second = StarterDecks.All().Select(d => d.Id).ToList();

        Assert.Equal(first, second);
    }
}
