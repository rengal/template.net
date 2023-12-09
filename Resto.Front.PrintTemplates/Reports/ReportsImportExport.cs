﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Resto.Common.Extensions;
using Resto.Data;
 using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Framework.Common.XmlSerialization;

namespace Resto.Front.PrintTemplates.Reports
{
    public static class ReportsImportExport
    {
        #region Consts
        private static readonly XNamespace IikoFrontReportNamespace = "http://iiko.ru/XmlSchemas/IikoFrontReport.xsd";

        private static readonly XName NameAttribute = "Name";
        private static readonly XName CategoryNameAttribute = "CategoryName";
        private static readonly XName RequirementsAttribute = "Requirements";
        private static readonly XName LabelAttribute = "Label";
        private static readonly XName PromptAttribute = "Prompt";
        private static readonly XName PersistentAttribute = "Persistent";
        private static readonly XName DefaultValueAttribute = "DefaultValue";
        private static readonly XName DefaultKindAttribute = "DefaultKind";
        private static readonly XName MinValueAttribute = "MinValue";
        private static readonly XName MaxValueAttribute = "MaxValue";
        private static readonly XName MaxLengthAttribute = "MaxLength";
        private static readonly XName ShowTimeEditorAttribute = "ShowTimeEditor";

        private static readonly XName RootElement = IikoFrontReportNamespace + "IikoFrontReport";
        private static readonly XName TemplateElement = IikoFrontReportNamespace + "Template";
        private static readonly XName PageElement = IikoFrontReportNamespace + "Page";

        private static readonly XName BooleanParameterElement = IikoFrontReportNamespace + "BooleanParameter";
        private static readonly XName NumberParameterElement = IikoFrontReportNamespace + "NumberParameter";
        private static readonly XName NumberParameterIntegerElement = IikoFrontReportNamespace + "Integer";
        private static readonly XName NumberParameterAmountElement = IikoFrontReportNamespace + "Amount";
        private static readonly XName NumberParameterMoneyElement = IikoFrontReportNamespace + "Money";
        private static readonly XName StringParameterElement = IikoFrontReportNamespace + "StringParameter";
        private static readonly XName DateTimePeriodParameterElement = IikoFrontReportNamespace + "DateTimePeriodParameter";
        private static readonly XName EnumParameterElement = IikoFrontReportNamespace + "EnumParameter";
        private static readonly XName EnumValueElement = IikoFrontReportNamespace + "Value";
        private static readonly XName TerminalsScopeParameterElement = IikoFrontReportNamespace + "TerminalsScopeParameter";
        private static readonly XName CounteragentsParameterElement = IikoFrontReportNamespace + "CounteragentsParameter";
        #endregion

        #region Conversions
        private static readonly Dictionary<ReportRequirements, string> RequirementsToString;
        private static readonly Dictionary<string, ReportRequirements> StringToRequirements;

        private static readonly Dictionary<DateTimePeriodReportParameterValue, string> DateTimePeriodToString;
        private static readonly Dictionary<string, DateTimePeriodReportParameterValue> StringToDateTimePeriod;

        private static readonly Dictionary<TerminalsScopeReportParameterValue, string> TerminalsScopeToString;
        private static readonly Dictionary<string, TerminalsScopeReportParameterValue> StringToTerminalsScope;

        private static readonly Dictionary<NumberReportParameterKind, XName> NumberKindToName;
        private static readonly Dictionary<XName, NumberReportParameterKind> NameToNumberKind;

        private static readonly Dictionary<CounteragentsReportParameterKind, string> CounteragentTypeToString;
        private static readonly Dictionary<string, CounteragentsReportParameterKind> StringToCounteragentType;

        static ReportsImportExport()
        {
            RequirementsToString =
                new Dictionary<ReportRequirements, string>(3)
                {
                    { ReportRequirements.REQUIRES_EVENTS, "RequiresEvents" },
                    { ReportRequirements.REQUIRES_ORDERS, "RequiresOrders" },
                    { ReportRequirements.REQUIRES_SERVER, "RequiresServer" }
                };

            StringToRequirements = RequirementsToString.Invert();


            DateTimePeriodToString =
                new Dictionary<DateTimePeriodReportParameterValue, string>(10)
                {
                    { DateTimePeriodReportParameterValue.CAFE_SESSION, "CafeSession" },
                    { DateTimePeriodReportParameterValue.BUSINESS_DAY, "BusinessDay" },
                    { DateTimePeriodReportParameterValue.CURRENT_DAY, "CurrentDay" },
                    { DateTimePeriodReportParameterValue.CURRENT_WEEK, "CurrentWeek" },
                    { DateTimePeriodReportParameterValue.CURRENT_MONTH, "CurrentMonth" },
                    { DateTimePeriodReportParameterValue.CURRENT_YEAR, "CurrentYear" },
                    { DateTimePeriodReportParameterValue.PREVIOUS_DAY, "PreviousDay" },
                    { DateTimePeriodReportParameterValue.PREVIOUS_WEEK, "PreviousWeek" },
                    { DateTimePeriodReportParameterValue.PREVIOUS_MONTH, "PreviousMonth" },
                    { DateTimePeriodReportParameterValue.PREVIOUS_YEAR, "PreviousYear" }
                };

            StringToDateTimePeriod = DateTimePeriodToString.Invert();


            TerminalsScopeToString =
                new Dictionary<TerminalsScopeReportParameterValue, string>(2)
                {
                    { TerminalsScopeReportParameterValue.ALL_TERMINALS, "AllTerminals" },
                    { TerminalsScopeReportParameterValue.CURRENT_TERMINAL, "CurrentTerminal" }
                };

            StringToTerminalsScope = TerminalsScopeToString.Invert();

            CounteragentTypeToString =
                new Dictionary<CounteragentsReportParameterKind, string>(3)
                {
                    { CounteragentsReportParameterKind.EMPLOYEE, "Employee" },
                    { CounteragentsReportParameterKind.CLIENT, "Client" },
                    { CounteragentsReportParameterKind.SUPPLIER, "Supplier" }
                };

            StringToCounteragentType = CounteragentTypeToString.Invert();


            NumberKindToName =
                new Dictionary<NumberReportParameterKind, XName>(3)
                {
                    { NumberReportParameterKind.INTEGER, NumberParameterIntegerElement },
                    { NumberReportParameterKind.AMOUNT, NumberParameterAmountElement },
                    { NumberReportParameterKind.MONEY, NumberParameterMoneyElement }
                };

            NameToNumberKind = NumberKindToName.Invert();
        }
        #endregion

        #region Import
        [NotNull, Pure]
        public static FrontReport Import([NotNull] string report)
        {
            if (report == null)
                throw new ArgumentNullException(nameof(report));

            XDocument xml;
            try
            {
                xml = XDocument.Parse(report);
            }
            catch (XmlException e)
            {
                throw new ReportValidationException("Report xml parsing error", e);
            }

            ValidateReportXml(xml);

            var reportElement = xml.Root;
            Debug.Assert(reportElement != null);

            var result = new FrontReport();

            var nameAttribute = reportElement.Attribute(NameAttribute);
            result.Name = new LocalizableValue(nameAttribute?.Value ?? string.Empty);

            var categoryNameAttribute = reportElement.Attribute(CategoryNameAttribute);
            result.CategoryName = new LocalizableValue(categoryNameAttribute?.Value ?? string.Empty);

            var requirementsAttribute = reportElement.Attribute(RequirementsAttribute);
            result.ReportRequirements = requirementsAttribute == null ? null : StringToRequirements[requirementsAttribute.Value];

            result.Pages = reportElement.Elements(PageElement).Select(ConvertElementToPage).ToList();

            var templateElement = reportElement.Element(TemplateElement);
            Debug.Assert(templateElement != null);
            result.Template = templateElement.Value;

            return result;
        }

        [NotNull, Pure]
        private static ReportParametersPage ConvertElementToPage([NotNull] XElement element)
        {
            Debug.Assert(element != null);

            var promptAttribute = element.Attribute(PromptAttribute);
            var prompt = promptAttribute?.Value ?? string.Empty;
            return new ReportParametersPage(name: element.Attribute(NameAttribute).Value, prompt: prompt)
            {
                Parameters = element.Elements().Select(ConvertElementToParameter).ToList()
            };
        }

        [NotNull, Pure]
        private static ReportParameter ConvertElementToParameter([NotNull] XElement element)
        {
            Debug.Assert(element != null);

            var result =
                TryConvertElementToBooleanParameter(element) ??
                TryConvertElementToNumberParameter(element) ??
                TryConvertElementToStringParameter(element) ??
                TryConvertElementToDateTimePeriodParameter(element) ??
                TryConvertElementToCustomEnumParameter(element) ??
                TryConvertElementToTerminalsScopeParameter(element) ??
                TryConvertElementToCounteragentsParameter(element);

            Debug.Assert(result != null);

            return result;
        }

        [CanBeNull, Pure]
        private static ReportParameter TryConvertElementToBooleanParameter([NotNull] XElement element)
        {
            Debug.Assert(element != null);

            if (element.Name != BooleanParameterElement)
                return null;

            var result = new BooleanReportParameter();
            ReadCommonParameterProperties(element, result);
            result.DefaultValue = bool.Parse(element.Attribute(DefaultValueAttribute).Value);

            return result;
        }

        [CanBeNull, Pure]
        private static ReportParameter TryConvertElementToNumberParameter([NotNull] XElement element)
        {
            Debug.Assert(element != null);

            if (element.Name != NumberParameterElement)
                return null;

            var result = new NumberReportParameter();
            ReadCommonParameterProperties(element, result);

            var childElement = element.Elements().Single();

            result.ParameterKind = NameToNumberKind[childElement.Name];
            result.MinValue = decimal.Parse(childElement.Attribute(MinValueAttribute).Value, CultureInfo.InvariantCulture);
            result.MaxValue = decimal.Parse(childElement.Attribute(MaxValueAttribute).Value, CultureInfo.InvariantCulture);
            result.DefaultValue = decimal.Parse(childElement.Attribute(DefaultValueAttribute).Value, CultureInfo.InvariantCulture);

            return result;
        }

        [CanBeNull, Pure]
        private static ReportParameter TryConvertElementToStringParameter([NotNull] XElement element)
        {
            Debug.Assert(element != null);

            if (element.Name != StringParameterElement)
                return null;

            var result = new StringReportParameter();
            ReadCommonParameterProperties(element, result);
            result.DefaultValue = element.Attribute(DefaultValueAttribute).Value;
            result.MaxLength = int.Parse(element.Attribute(MaxLengthAttribute).Value, CultureInfo.InvariantCulture);

            return result;
        }

        [CanBeNull, Pure]
        private static ReportParameter TryConvertElementToDateTimePeriodParameter([NotNull] XElement element)
        {
            Debug.Assert(element != null);

            if (element.Name != DateTimePeriodParameterElement)
                return null;

            var result = new DateTimePeriodReportParameter();
            ReadCommonParameterProperties(element, result);
            result.DefaultValue = StringToDateTimePeriod[element.Attribute(DefaultValueAttribute).Value];
            result.ShowTimeEditor = bool.Parse(element.Attribute(ShowTimeEditorAttribute).Value);

            return result;
        }

        [CanBeNull, Pure]
        private static ReportParameter TryConvertElementToCustomEnumParameter([NotNull] XElement element)
        {
            Debug.Assert(element != null);

            if (element.Name != EnumParameterElement)
                return null;

            var result = new CustomEnumReportParameter();
            ReadCommonParameterProperties(element, result);

            var defaultValue = element.Attribute(DefaultValueAttribute).Value;
            result.Values = new List<CustomEnumReportParameterValue>();

            foreach (var valueElement in element.Elements())
            {
                var name = valueElement.Attribute(NameAttribute).Value;
                var label = valueElement.Attribute(LabelAttribute).Value;
                var value = new CustomEnumReportParameterValue(name, label) { IsDefault = name == defaultValue };

                result.Values.Add(value);
            }

            return result;
        }

        private static ReportParameter TryConvertElementToTerminalsScopeParameter([NotNull] XElement element)
        {
            Debug.Assert(element != null);

            if (element.Name != TerminalsScopeParameterElement)
                return null;

            var result = new TerminalsScopeReportParameter();
            ReadCommonParameterProperties(element, result);
            result.DefaultValue = StringToTerminalsScope[element.Attribute(DefaultValueAttribute).Value];

            return result;
        }

        private static ReportParameter TryConvertElementToCounteragentsParameter([NotNull] XElement element)
        {
            Debug.Assert(element != null);

            if (element.Name != CounteragentsParameterElement)
                return null;

            var result = new CounteragentsReportParameter();
            ReadCommonParameterProperties(element, result);
            result.CounteragentType = StringToCounteragentType[element.Attribute(DefaultKindAttribute).Value];

            return result;
        }

        private static void ReadCommonParameterProperties([NotNull] XElement element, [NotNull] ReportParameter parameter)
        {
            Debug.Assert(element != null);
            Debug.Assert(parameter != null);

            parameter.Name = element.Attribute(NameAttribute).Value;
            parameter.Label = element.Attribute(LabelAttribute).Value;
            parameter.Persistent = bool.Parse(element.Attribute(PersistentAttribute).Value);
        }
        #endregion

        #region Export
        private static readonly XmlWriterSettings WriterSettings =
            new XmlWriterSettings
            {
                CheckCharacters = false,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false,
                Indent = true,
                IndentChars = "    ",
                CloseOutput = true
            };

        [NotNull, Pure]
        public static string Export([NotNull] FrontReport report)
        {
            if (report == null)
                throw new ArgumentNullException(nameof(report));

            var root = new XElement(RootElement);

            if (!report.NameLocal.IsNullOrWhiteSpace())
                root.Add(new XAttribute(NameAttribute, report.Name));
            if (report.CategoryName != null && !report.CategoryName.Local.IsNullOrWhiteSpace())
                root.Add(new XAttribute(CategoryNameAttribute, report.CategoryName.Local));
            if (report.ReportRequirements != null)
                root.Add(new XAttribute(RequirementsAttribute, RequirementsToString[report.ReportRequirements]));

            foreach (var page in report.GetPagesNullSafe())
                root.Add(ConvertPageToElement(page));

            root.Add(new XElement(TemplateElement, report.Template));

            var doc = new XDocument();
            doc.Add(root);

            string result;

            using (var stringWriter = new Serializer.StringWriterWithEncoding(WriterSettings.Encoding))
            {
                using (var writer = XmlWriter.Create(stringWriter, WriterSettings))
                {
                    doc.WriteTo(writer);
                    writer.Flush();
                }

                result = stringWriter.ToString();
            }

#if DEBUG
            ValidateReportXml(XDocument.Parse(result));
#endif

            return result;
        }

        [NotNull, Pure]
        private static XElement ConvertPageToElement([NotNull] ReportParametersPage page)
        {
            Debug.Assert(page != null);

            var result = new XElement(PageElement);

            result.Add(new XAttribute(NameAttribute, page.Name));

            if (!string.IsNullOrWhiteSpace(page.Prompt))
                result.Add(new XAttribute(PromptAttribute, page.Prompt));

            foreach (var parameter in page.GetParametersNullSafe())
                result.Add(ConvertParameterToElement(parameter));

            return result;
        }

        [NotNull, Pure]
        private static XElement ConvertParameterToElement([NotNull] ReportParameter parameter)
        {
            Debug.Assert(parameter != null);

            var result =
                TryConvertBooleanParameterToElement(parameter as BooleanReportParameter) ??
                TryConvertNumberParameterToElement(parameter as NumberReportParameter) ??
                TryConvertStringParameterToElement(parameter as StringReportParameter) ??
                TryConvertDateTimePeriodParameterToElement(parameter as DateTimePeriodReportParameter) ??
                TryConvertCustomEnumParameterToElement(parameter as CustomEnumReportParameter) ??
                TryConvertTerminalsScopeParameterToElement(parameter as TerminalsScopeReportParameter) ??
                TryConvertCounteragentsParameterToElement(parameter as CounteragentsReportParameter);

            if (result == null)
                throw new ArgumentOutOfRangeException(nameof(parameter), parameter, "Unsupported report parameter type");

            return result;
        }

        [CanBeNull, Pure]
        private static XElement TryConvertBooleanParameterToElement([CanBeNull] BooleanReportParameter parameter)
        {
            return parameter == null
                ? null
                : CreateParameterElement(BooleanParameterElement, parameter, parameter.DefaultValue);
        }

        [CanBeNull, Pure]
        private static XElement TryConvertNumberParameterToElement([CanBeNull] NumberReportParameter parameter)
        {
            if (parameter == null)
                return null;

            var child = new XElement(NumberKindToName[parameter.ParameterKind]);

            child.Add(new XAttribute(MinValueAttribute, parameter.MinValue.GetValueOrFakeDefault()));
            child.Add(new XAttribute(MaxValueAttribute, parameter.MaxValue.GetValueOrFakeDefault()));
            child.Add(new XAttribute(DefaultValueAttribute, parameter.DefaultValue.GetValueOrFakeDefault()));

            var result = CreateParameterElement(NumberParameterElement, parameter);

            result.Add(child);

            return result;
        }

        [CanBeNull, Pure]
        private static XElement TryConvertStringParameterToElement([CanBeNull] StringReportParameter parameter)
        {
            if (parameter == null)
                return null;

            var result = CreateParameterElement(StringParameterElement, parameter, parameter.DefaultValue);

            result.Add(new XAttribute(MaxLengthAttribute, parameter.MaxLength));

            return result;
        }

        [CanBeNull, Pure]
        private static XElement TryConvertDateTimePeriodParameterToElement([CanBeNull] DateTimePeriodReportParameter parameter)
        {
            if (parameter == null)
                return null;

            var result = CreateParameterElement(DateTimePeriodParameterElement, parameter, DateTimePeriodToString[parameter.DefaultValue]);

            result.Add(new XAttribute(ShowTimeEditorAttribute, parameter.ShowTimeEditor));

            return result;
        }

        [CanBeNull, Pure]
        private static XElement TryConvertCustomEnumParameterToElement([CanBeNull] CustomEnumReportParameter parameter)
        {
            if (parameter == null)
                return null;

            Debug.Assert(parameter.Values.Count(value => value.IsDefault) == 1);

            var defaultValue = parameter.Values.Single(v => v.IsDefault).Name;
            var result = CreateParameterElement(EnumParameterElement, parameter, defaultValue);

            foreach (var value in parameter.Values)
                result.Add(new XElement(EnumValueElement, new XAttribute(NameAttribute, value.Name), new XAttribute(LabelAttribute, value.Label)));

            return result;
        }

        private static XElement TryConvertTerminalsScopeParameterToElement([CanBeNull] TerminalsScopeReportParameter parameter)
        {
            return parameter == null
                ? null
                : CreateParameterElement(TerminalsScopeParameterElement, parameter, TerminalsScopeToString[parameter.DefaultValue]);
        }

        private static XElement TryConvertCounteragentsParameterToElement([CanBeNull] CounteragentsReportParameter parameter)
        {
            if (parameter == null)
                return null;

            var result = CreateParameterElement(CounteragentsParameterElement, parameter);

            result.Add(new XAttribute(DefaultKindAttribute, CounteragentTypeToString[parameter.CounteragentType]));

            return result;
        }

        [NotNull, Pure]
        private static XElement CreateParameterElement([NotNull] XName name, [NotNull] ReportParameter parameter, [CanBeNull] object defaultValue = null)
        {
            Debug.Assert(name != null);
            Debug.Assert(parameter != null);

            var result = new XElement(name);

            result.Add(new XAttribute(NameAttribute, parameter.Name));
            result.Add(new XAttribute(LabelAttribute, parameter.Label));
            result.Add(new XAttribute(PersistentAttribute, parameter.Persistent));
            if (defaultValue != null)
                result.Add(new XAttribute(DefaultValueAttribute, defaultValue));

            return result;
        }
        #endregion

        #region Validation
        private static readonly Lazy<XmlSchemaSet> ValidationSchemaSet = new Lazy<XmlSchemaSet>(CreateXmlSchemaSet);

        private static void ValidateReportXml([NotNull] XDocument doc)
        {
            Debug.Assert(doc != null);

            var root = doc.Root;
            Debug.Assert(root != null);

            root.SetDefaultXmlNamespace(IikoFrontReportNamespace);

            var validationErrors = new List<string>();
            ValidationEventHandler errorHandler = (_, error) => validationErrors.Add(string.Format("{0}: {1}", error.Severity, error.Message));

            doc.Validate(ValidationSchemaSet.Value, errorHandler);

            if (validationErrors.Any())
                throw new ReportValidationException("Report xml validation error", validationErrors);

            // У числовых параметров значения по умолчанию должны попадать в интервал между минимумом и максимумом,
            // а значения минимума и максимума должны быть согласованы между собой.
            // Такие ограничения невозможно выразить с помощью xsd-схемы, поэтому проверяем их вручную.
            foreach (var numberParameterElement in root.Elements(PageElement).SelectMany(page => page.Elements(NumberParameterElement)))
            {
                var childElement = numberParameterElement.Elements().Single();

                var minValue = decimal.Parse(childElement.Attribute(MinValueAttribute).Value, CultureInfo.InvariantCulture);
                var maxValue = decimal.Parse(childElement.Attribute(MaxValueAttribute).Value, CultureInfo.InvariantCulture);
                var defaultValue = decimal.Parse(childElement.Attribute(DefaultValueAttribute).Value, CultureInfo.InvariantCulture);

                if (minValue > maxValue)
                    validationErrors.Add(string.Format("Error: {0} greater than {1} in parameter '{2}'.", MinValueAttribute, MaxValueAttribute, numberParameterElement.Attribute(NameAttribute).Value));

                if (defaultValue < minValue)
                    validationErrors.Add(string.Format("Error: {0} less than {1} in parameter '{2}'.", DefaultValueAttribute, MinValueAttribute, numberParameterElement.Attribute(NameAttribute).Value));

                if (defaultValue > maxValue)
                    validationErrors.Add(string.Format("Error: {0} greater than {1} in parameter '{2}'.", DefaultValueAttribute, MaxValueAttribute, numberParameterElement.Attribute(NameAttribute).Value));
            }

            // У строковых параметров длина значения по умолчанию не должна быть больше максимальной длины.
            // Такое ограничение невозможно выразить с помощью xsd-схемы, поэтому проверяем его вручную.
            foreach (var stringParameterElement in root.Elements(PageElement).SelectMany(page => page.Elements(StringParameterElement)))
            {
                var defaultValue = stringParameterElement.Attribute(DefaultValueAttribute).Value;
                var maxLength = int.Parse(stringParameterElement.Attribute(MaxLengthAttribute).Value);

                if (defaultValue.Length > maxLength)
                    validationErrors.Add(string.Format("Error: {0} length greater than {1} in parameter '{2}'.", DefaultValueAttribute, MaxLengthAttribute, stringParameterElement.Attribute(NameAttribute).Value));
            }

            if (validationErrors.Any())
                throw new ReportValidationException("Report xml validation error", validationErrors);
        }

        private static void SetDefaultXmlNamespace([NotNull] this XElement element, [NotNull] XNamespace xmlns)
        {
            Debug.Assert(element != null);
            Debug.Assert(xmlns != null);

            if (element.Name.NamespaceName != xmlns.NamespaceName)
                element.Name = xmlns + element.Name.LocalName;
            foreach (var e in element.Elements())
                e.SetDefaultXmlNamespace(xmlns);
        }

        private static XmlSchemaSet CreateXmlSchemaSet()
        {
            var result = new XmlSchemaSet();
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Resto.Front.PrintTemplates.Reports.IikoFrontReport.xsd"))
            {
                Debug.Assert(stream != null);
                using (var reader = XmlReader.Create(stream))
                {
                    result.Add(IikoFrontReportNamespace.NamespaceName, reader);
                }
            }

            return result;
        }
        #endregion
    }

    public sealed class ReportValidationException : Exception
    {
        public ReportValidationException(string message, IEnumerable<string> errors)
            : base(errors.StartWith(message).Join(Environment.NewLine))
        { }

        public ReportValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}