// This file was generated with T4.
// Do not edit it manually.

// ReSharper disable RedundantUsingDirective

using Resto.Front.PrintTemplates.RmsEntityWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels
{
    /// <summary>
    /// Чек удаления модификаторов
    /// </summary>
    public interface IDeleteModifiersServiceCheque : ITemplateRootModel, IServiceChequeBase
    {
        /// <summary>
        /// Причина удаления
        /// </summary>
        [NotNull]
        string DeleteReason { get; }

        /// <summary>
        /// Модификаторы, для которых печатается чек
        /// </summary>
        [NotNull]
        IEnumerable<IModifierEntry> ModifierEntries { get; }

    }

    internal sealed class DeleteModifiersServiceCheque : ServiceChequeBase, IDeleteModifiersServiceCheque
    {
        #region Fields
        private readonly string deleteReason;
        private readonly List<ModifierEntry> modifierEntries = new List<ModifierEntry>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private DeleteModifiersServiceCheque()
        {}

        internal DeleteModifiersServiceCheque([NotNull] CopyContext context, [NotNull] IDeleteModifiersServiceCheque src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            deleteReason = src.DeleteReason;
            modifierEntries = src.ModifierEntries.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ModifierEntry.Convert)).ToList();
        }

        #endregion

        #region Props
        public string DeleteReason
        {
            get { return GetLocalizedValue(deleteReason); }
        }

        public IEnumerable<IModifierEntry> ModifierEntries
        {
            get { return modifierEntries; }
        }

        #endregion
    }

    public static partial class ChequeSerializer
    {
        [NotNull]
        public static string Serialize([NotNull] this IDeleteModifiersServiceCheque cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new DeleteModifiersServiceCheque(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<IDeleteModifiersServiceCheque>(copy, "DeleteModifiersServiceCheque");
        }
    }
}
