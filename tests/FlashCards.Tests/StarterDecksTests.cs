using FlashCards.Services;
using Xunit;

namespace FlashCards.Tests;

public class StarterDecksTests
{
    [Fact]
    public void ProvidesHiraganaAndKatakanaDecks()
    {
        var decks = StarterDecks.All().ToList();

        Assert.Equal(2, decks.Count);
        Assert.Contains(decks, d => d.Id == StarterDecks.HiraganaId);
        Assert.Contains(decks, d => d.Id == StarterDecks.KatakanaId);
    }

    [Fact]
    public void EachKanaDeckHasAll46BasicCharacters()
    {
        foreach (var deck in StarterDecks.All())
        {
            Assert.Equal(46, deck.Cards.Count);
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
