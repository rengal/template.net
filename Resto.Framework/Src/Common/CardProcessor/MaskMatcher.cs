using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.CardProcessor
{
    /// <summary>
    /// Класс реализующий поиск по шаблону. При совпадении, часть сиволов может быть "захвачена".
    /// </summary>
    /// <remarks>
    /// Каждый символ в шаблоне имеет определенный смысл:
    /// 'N' --- допустимый символ (cм. <see cref="MaskMatcherCaptureKind"/>), этот символ является частью "захвата";
    /// '*' --- любой символ, это символ не является часть "захвата";
    /// любой другой символ --- ищется точное совпадение символа, этот символ не является часть "захвата". 
    /// </remarks>
    public sealed class MaskMatcher
    {
        private readonly List<Mask> masks;

        public MaskMatcher([NotNull] IEnumerable<string> masks, MaskMatcherCaptureKind captureKind = MaskMatcherCaptureKind.Digits)
        {
            if (masks == null)
                throw new ArgumentNullException(nameof(masks));

            this.masks = masks
                .Where(m => !string.IsNullOrEmpty(m))
                .Select(mask => new Mask(mask, captureKind))
                .ToList();
        }

        [NotNull]
        public IList<string> GetMatches([NotNull] string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            return masks.Select(m => m.Match(value)).Where(m => m != null).Select(m => m.Match).ToList();
        }

        [NotNull]
        public IList<string> GetCaptures([NotNull] string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            return masks.Select(m => m.Match(value)).Where(m => m != null).Select(m => m.Capture).ToList();
        }

        private sealed class Mask
        {
            private Regex Regex { get; set; }

            public Mask([NotNull] string mask, MaskMatcherCaptureKind captureKind)
            {
                if (string.IsNullOrEmpty(mask))
                    throw new ArgumentOutOfRangeException(nameof(mask));
                Regex = CreateRegex(mask, captureKind);
            }

            [NotNull]
            private static Regex CreateRegex([NotNull] string mask, MaskMatcherCaptureKind captureKind)
            {
                var buffer = new StringBuilder();
                buffer.Append("^");
                for (var i = 0; i < mask.Length; i++)
                {
                    switch (mask[i])
                    {
                        case 'N':
                            switch (captureKind)
                            {
                                case MaskMatcherCaptureKind.Digits:
                                    AddRegexItem(buffer, mask, ref i, c => "\\d", true);
                                    break;
                                case MaskMatcherCaptureKind.DigitsAndLatinChars:
                                    AddRegexItem(buffer, mask, ref i, c => "[0-9a-zA-Z]", true);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException(nameof(captureKind));
                            }
                            break;
                        case '*':
                            AddRegexItem(buffer, mask, ref i, c => ".", false);
                            break;
                        default:
                            AddRegexItem(buffer, mask, ref i, c => Regex.Escape(c.ToString()), false);
                            break;
                    }
                }
                buffer.Append("$");

                return new Regex(buffer.ToString(), RegexOptions.Compiled);
            }

            public MaskMatch Match([NotNull] string text)
            {
                var match = Regex.Match(text);
                if (!match.Success)
                    return null;
                return new MaskMatch
                {
                    Match = match.Value,
                    Capture = match.Groups.Cast<Group>().Skip(1).SelectMany(
                        gr => gr.Captures.Cast<Capture>().Select(c => c.Value)).Join(string.Empty)
                };
            }

            private static void AddRegexItem(StringBuilder buffer, string inputString, ref int index, Func<char, string> regexReplacment, bool isCapture)
            {
                var symbol = inputString[index];
                var k = index;
                while (k < inputString.Length && inputString[k] == symbol)
                    k++;
                buffer.AppendFormat(isCapture ? "({0}{{{1}}})" : "{0}{{{1}}}", regexReplacment(symbol), k - index);
                index = k - 1;
            }
        }

        private sealed class MaskMatch
        {
            public string Match { get; set; }
            public string Capture { get; set; }
        }
    }

    /// <summary>
    /// Множество символов захватываемых подстановкой 'N' в шаблоне.
    /// </summary>
    public enum MaskMatcherCaptureKind
    {
        /// <summary>
        /// Только цифры.
        /// </summary>
        Digits,
        /// <summary>
        /// Цифры и латинские символы.
        /// </summary>
        DigitsAndLatinChars
    }
}