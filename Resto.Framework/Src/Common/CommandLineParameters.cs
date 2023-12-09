using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Класс для разбора параметров командной строки.
    /// </summary>
    /// <remarks>
    /// Регистронезависимый, разделитель параметров '/', разделитель значений '='
    /// </remarks>
    /// <example>
    /// "/ParentProcessId=56 /ServerChannelId=65 /HomePath="C:\Program Files\Some App" /StartedEventName=SomeAppStarted"
    /// </example>
    public sealed class CommandLineParameters
    {
        private const string ParameterFormat = @"^/([\w]+)=(""?)(.+)\2$";
        private static readonly Regex ParameterRegex = new Regex(ParameterFormat, RegexOptions.Compiled);
        private readonly IDictionary<string, string> parameters;

        public CommandLineParameters([NotNull] params string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));
            if (args.Any(string.IsNullOrEmpty))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(args));
            var invalidParameter = args.FirstOrDefault(parameter => !ParameterRegex.IsMatch(parameter));
            if (invalidParameter != null)
                throw new ArgumentException(string.Format("Parameter [{0}] doesn't match to format [{1}]", invalidParameter, ParameterFormat), nameof(args));

            parameters = args.Select(parameter => ParameterRegex.Match(parameter))
                .ToDictionary(match => match.Groups[1].Value, match => match.Groups[3].Value, StringComparer.OrdinalIgnoreCase);
        }

        public CommandLineParameters([NotNull] IDictionary<string, string> parameters)
        {
            this.parameters = new Dictionary<string, string>(parameters);
        }

        public string this[[NotNull] string key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                return parameters[key];
            }
        }

        public bool TryGet([NotNull]string key, out string val)
        {            
            return parameters.TryGetValue(key, out val);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var kvp in parameters.Where(parameter => !parameter.Value.IsNullOrWhiteSpace()))
            {
                sb.AppendFormat(kvp.Value.Contains(' ') ? "/{0}=\"{1}\" " : "/{0}={1} ", kvp.Key, kvp.Value);
            }
            return sb.ToString(0, sb.Length - 1);
        }
    }
}