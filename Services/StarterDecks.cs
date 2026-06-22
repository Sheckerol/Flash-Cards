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
    public static readonly Guid HiraganaExtendedId = new("a3a3a3a3-0000-0000-0000-000000000003");
    public static readonly Guid KatakanaExtendedId = new("a4a4a4a4-0000-0000-0000-000000000004");

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

        yield return BuildDeck(
            HiraganaExtendedId,
            "Hiragana — Dakuten & Combos",
            "Voiced (が), half-voiced (ぱ) and combined (きゃ) hiragana.",
            HiraganaExtended);

        yield return BuildDeck(
            KatakanaExtendedId,
            "Katakana — Dakuten & Combos",
            "Voiced (ガ), half-voiced (パ) and combined (キャ) katakana.",
            KatakanaExtended);
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

    private static readonly (string, string)[] HiraganaExtended =
    {
        // Dakuten (voiced)
        ("が", "ga"), ("ぎ", "gi"), ("ぐ", "gu"), ("げ", "ge"), ("ご", "go"),
        ("ざ", "za"), ("じ", "ji"), ("ず", "zu"), ("ぜ", "ze"), ("ぞ", "zo"),
        ("だ", "da"), ("ぢ", "ji (di)"), ("づ", "zu (du)"), ("で", "de"), ("ど", "do"),
        ("ば", "ba"), ("び", "bi"), ("ぶ", "bu"), ("べ", "be"), ("ぼ", "bo"),
        // Handakuten (half-voiced)
        ("ぱ", "pa"), ("ぴ", "pi"), ("ぷ", "pu"), ("ぺ", "pe"), ("ぽ", "po"),
        // Yōon (combinations)
        ("きゃ", "kya"), ("きゅ", "kyu"), ("きょ", "kyo"),
        ("しゃ", "sha"), ("しゅ", "shu"), ("しょ", "sho"),
        ("ちゃ", "cha"), ("ちゅ", "chu"), ("ちょ", "cho"),
        ("にゃ", "nya"), ("にゅ", "nyu"), ("にょ", "nyo"),
        ("ひゃ", "hya"), ("ひゅ", "hyu"), ("ひょ", "hyo"),
        ("みゃ", "mya"), ("みゅ", "myu"), ("みょ", "myo"),
        ("りゃ", "rya"), ("りゅ", "ryu"), ("りょ", "ryo"),
        ("ぎゃ", "gya"), ("ぎゅ", "gyu"), ("ぎょ", "gyo"),
        ("じゃ", "ja"), ("じゅ", "ju"), ("じょ", "jo"),
        ("びゃ", "bya"), ("びゅ", "byu"), ("びょ", "byo"),
        ("ぴゃ", "pya"), ("ぴゅ", "pyu"), ("ぴょ", "pyo"),
    };

    private static readonly (string, string)[] KatakanaExtended =
    {
        // Dakuten (voiced)
        ("ガ", "ga"), ("ギ", "gi"), ("グ", "gu"), ("ゲ", "ge"), ("ゴ", "go"),
        ("ザ", "za"), ("ジ", "ji"), ("ズ", "zu"), ("ゼ", "ze"), ("ゾ", "zo"),
        ("ダ", "da"), ("ヂ", "ji (di)"), ("ヅ", "zu (du)"), ("デ", "de"), ("ド", "do"),
        ("バ", "ba"), ("ビ", "bi"), ("ブ", "bu"), ("ベ", "be"), ("ボ", "bo"),
        // Handakuten (half-voiced)
        ("パ", "pa"), ("ピ", "pi"), ("プ", "pu"), ("ペ", "pe"), ("ポ", "po"),
        // Yōon (combinations)
        ("キャ", "kya"), ("キュ", "kyu"), ("キョ", "kyo"),
        ("シャ", "sha"), ("シュ", "shu"), ("ショ", "sho"),
        ("チャ", "cha"), ("チュ", "chu"), ("チョ", "cho"),
        ("ニャ", "nya"), ("ニュ", "nyu"), ("ニョ", "nyo"),
        ("ヒャ", "hya"), ("ヒュ", "hyu"), ("ヒョ", "hyo"),
        ("ミャ", "mya"), ("ミュ", "myu"), ("ミョ", "myo"),
        ("リャ", "rya"), ("リュ", "ryu"), ("リョ", "ryo"),
        ("ギャ", "gya"), ("ギュ", "gyu"), ("ギョ", "gyo"),
        ("ジャ", "ja"), ("ジュ", "ju"), ("ジョ", "jo"),
        ("ビャ", "bya"), ("ビュ", "byu"), ("ビョ", "byo"),
        ("ピャ", "pya"), ("ピュ", "pyu"), ("ピョ", "pyo"),
    };
}
