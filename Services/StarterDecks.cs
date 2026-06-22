using FlashCards.Models;

namespace FlashCards.Services;

/// <summary>
/// Built-in decks the app ships with. Each has a stable <see cref="Deck.Id"/>
/// so it can be added once and not duplicated on later loads.
/// </summary>
public static class StarterDecks
{
    public static readonly Guid HiraganaId = new("a1a1a1a1-0000-0000-0000-000000000001");
    public static readonly Guid KatakanaId = new("a2a2a2a2-0000-0000-0000-000000000002");

    /// <summary>Returns fresh instances of every starter deck.</summary>
    public static IEnumerable<Deck> All()
    {
        yield return BuildDeck(
            HiraganaId,
            "Hiragana (ひらがな)",
            "The basic 46 hiragana — see the character, recall its sound.",
            Hiragana);

        yield return BuildDeck(
            KatakanaId,
            "Katakana (カタカナ)",
            "The basic 46 katakana — see the character, recall its sound.",
            Katakana);
    }

    private static Deck BuildDeck(Guid id, string name, string description, (string Kana, string Romaji)[] kana)
    {
        var deck = new Deck { Id = id, Name = name, Description = description };
        foreach (var (kanaChar, romaji) in kana)
        {
            deck.Cards.Add(new Card { Front = kanaChar, Back = romaji });
        }
        return deck;
    }

    private static readonly (string, string)[] Hiragana =
    {
        ("あ", "a"), ("い", "i"), ("う", "u"), ("え", "e"), ("お", "o"),
        ("か", "ka"), ("き", "ki"), ("く", "ku"), ("け", "ke"), ("こ", "ko"),
        ("さ", "sa"), ("し", "shi"), ("す", "su"), ("せ", "se"), ("そ", "so"),
        ("た", "ta"), ("ち", "chi"), ("つ", "tsu"), ("て", "te"), ("と", "to"),
        ("な", "na"), ("に", "ni"), ("ぬ", "nu"), ("ね", "ne"), ("の", "no"),
        ("は", "ha"), ("ひ", "hi"), ("ふ", "fu"), ("へ", "he"), ("ほ", "ho"),
        ("ま", "ma"), ("み", "mi"), ("む", "mu"), ("め", "me"), ("も", "mo"),
        ("や", "ya"), ("ゆ", "yu"), ("よ", "yo"),
        ("ら", "ra"), ("り", "ri"), ("る", "ru"), ("れ", "re"), ("ろ", "ro"),
        ("わ", "wa"), ("を", "wo"),
        ("ん", "n"),
    };

    private static readonly (string, string)[] Katakana =
    {
        ("ア", "a"), ("イ", "i"), ("ウ", "u"), ("エ", "e"), ("オ", "o"),
        ("カ", "ka"), ("キ", "ki"), ("ク", "ku"), ("ケ", "ke"), ("コ", "ko"),
        ("サ", "sa"), ("シ", "shi"), ("ス", "su"), ("セ", "se"), ("ソ", "so"),
        ("タ", "ta"), ("チ", "chi"), ("ツ", "tsu"), ("テ", "te"), ("ト", "to"),
        ("ナ", "na"), ("ニ", "ni"), ("ヌ", "nu"), ("ネ", "ne"), ("ノ", "no"),
        ("ハ", "ha"), ("ヒ", "hi"), ("フ", "fu"), ("ヘ", "he"), ("ホ", "ho"),
        ("マ", "ma"), ("ミ", "mi"), ("ム", "mu"), ("メ", "me"), ("モ", "mo"),
        ("ヤ", "ya"), ("ユ", "yu"), ("ヨ", "yo"),
        ("ラ", "ra"), ("リ", "ri"), ("ル", "ru"), ("レ", "re"), ("ロ", "ro"),
        ("ワ", "wa"), ("ヲ", "wo"),
        ("ン", "n"),
    };
}
