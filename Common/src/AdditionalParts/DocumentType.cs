using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public partial class DocumentType
    {
        /// <summary>
        /// Список действующих в текущем релизе типов документов <see cref="DocumentType"/>.
        /// </summary>
        /// <remarks>
        /// RestoServer:resto.back.documents.DocumentType.getNonObsoleteValues()
        /// </remarks>
        private static IReadOnlyList<DocumentType> nonObsoleteValues;

        /// <summary>
        /// Список разрешённых текущей лицензией типов документов <see cref="DocumentType"/>.
        /// </summary>
        /// <remarks>
        /// Использует <see cref="LicenseServiceInstance"/>
        /// RestoServer:resto.back.documents.DocumentType.getActiveValues()
        /// </remarks>
        private static IReadOnlyList<DocumentType> activeValues;

        /// <summary>
        /// Словарь соответствий между классами документов и значениями <see cref="DocumentType"/>
        /// </summary>
        private static Dictionary<Type, DocumentType> documentTypesMap;

        /// <summary>
        /// Возвращает true, если лицензия разрешает использовать данный тип документа
        /// </summary>
        /// <remarks>
        /// Использует <see cref="LicenseServiceInstance"/>
        /// RestoServer:resto.back.documents.DocumentType.isAllowedByLicense()
        /// </remarks>
        public bool IsAllowedByLicense => true;

        public override bool Equals(object obj)
        {
            DocumentType type = obj as DocumentType;
            if (type == null)
                return false;
            return type.__value.Equals(__value);
        }

        /// <summary>
        /// Пытается для данного типа документа получить тройку прав (чтение, изменение, проведение).
        /// </summary>
        /// <returns><c>true</c> в случае успеха (известны права для данного типа документов), иначе - <c>false</c>.</returns>
        public bool TryGetPermissions(out Permission view, out Permission edit, out Permission process)
        {
            view = edit = process = null;

            if (this == CONSOLIDATED_ORDER)
            {
                view = Permission.CAN_ACCESS_CONSOLIDATED_ORDERS;
                edit = Permission.CAN_ACCESS_CONSOLIDATED_ORDERS;
                process = Permission.CAN_ACCESS_CONSOLIDATED_ORDERS;
            }
            else if (this == INCOMING_INVENTORY)
            {
                view = Permission.CAN_VIEW_INVENTORY;
                edit = Permission.CAN_EDIT_INVENTORY;
                process = Permission.CAN_PROCESS_INVENTORY;
            }
            else if (this == INCOMING_INVOICE || this == OUTGOING_INVOICE)
            {
                view = Permission.CAN_ACCESS_INVOICE_VIEW;
                edit = Permission.CAN_ACCESS_INVOICE_CREATE;
                process = Permission.CAN_ACCESS_INVOICE_REALIZATION;
            }
            else if (this == INCOMING_SERVICE || this == OUTGOING_SERVICE)
            {
                view = Permission.CAN_VIEW_SERVICES;
                edit = Permission.CAN_CREATE_AND_EDIT_SERVICES_WITHOUT_PROCESSING;
                process = Permission.CAN_PROCESS_SERVICES;
            }
            else if (this == RETURNED_INVOICE)
            {
                view = Permission.CAN_ACCESS_RETURNEDINVOICE_VIEW;
                edit = Permission.CAN_ACCESS_RETURNEDINVOICE_CREATE;
                process = Permission.CAN_ACCESS_RETURNEDINVOICE_REALIZATION;
            }
            else if (this == INCOMING_RETURNED_INVOICE)
            {
                view = Permission.CAN_ACCESS_INCOMING_RETURNED_INV_VIEW;
                edit = Permission.CAN_ACCESS_INCOMING_RETURNED_INV_CREATE;
                process = Permission.CAN_ACCESS_INCOMING_RETURNED_INV_REALIZATION;
            }
            else if (this == INTERNAL_TRANSFER)
            {
                view = Permission.CAN_ACCESS_INTERNAL_INVOICES_VIEW;
                edit = Permission.CAN_ACCESS_INTERNAL_INVOICES_CREATE;
                process = Permission.CAN_ACCESS_INTERNAL_INVOICES_REALIZATION;
            }
            else if (this == MENU_CHANGE)
            {
                view = Permission.CAN_ACCESS_MENU_ORDERS;
                edit = Permission.CAN_ACCESS_MENU_ORDERS;
                process = Permission.CAN_PROCESS_MENU_ORDERS;
            }
            else if (this == PRODUCTION_DOCUMENT)
            {
                view = Permission.CAN_ACCESS_PRODUCTION_VIEW;
                edit = Permission.CAN_ACCESS_PRODUCTION_CREATE;
                process = Permission.CAN_ACCESS_PRODUCTION_REALIZATION;
            }
            else if (this == PRODUCTION_ORDER)
            {
                view = Permission.CAN_ACCESS_PRODUCTION;
                edit = Permission.CAN_ACCESS_PRODUCTION;
                process = Permission.CAN_ACCESS_PRODUCTION;
            }
            else if (this == PRODUCT_REPLACEMENT)
            {
                view = Permission.CAN_VIEW_NOMENCLATURE;
                edit = Permission.CAN_EDIT_NOMENCLATURE;
                process = Permission.CAN_EDIT_NOMENCLATURE;
            }
            else if (this == SALES_DOCUMENT)
            {
                view = Permission.CAN_ACCESS_SALES_VIEW;
                edit = Permission.CAN_ACCESS_SALES_CREATE;
                process = Permission.CAN_ACCESS_SALES_REALIZATION;
            }
            else if (this == SESSION_ACCEPTANCE)
            {
                view = Permission.CAN_VIEW_CASH_SESSIONS;
                edit = Permission.CAN_ACCEPT_CASH_SESSIONS;
                process = Permission.CAN_ACCEPT_CASH_SESSIONS;
            }
            else if (this == TRANSFORMATION_DOCUMENT)
            {
                view = Permission.CAN_ACCESS_TRANSFORMATION_VIEW;
                edit = Permission.CAN_ACCESS_TRANSFORMATION_CREATE;
                process = Permission.CAN_ACCESS_TRANSFORMATION_REALIZATION;
            }
            else if (this == WRITEOFF_DOCUMENT)
            {
                view = Permission.CAN_ACCESS_WRITEOFF_VIEW;
                edit = Permission.CAN_ACCESS_WRITEOFF_CREATE;
                process = Permission.CAN_ACCESS_WRITEOFF_REALIZATION;
            }
            else if (this == DISASSEMBLE_DOCUMENT)
            {
                view = Permission.CAN_ACCESS_DISASSEMBLES_VIEW;
                edit = Permission.CAN_ACCESS_DISASSEMBLES_CREATE;
                process = Permission.CAN_ACCESS_DISASSEMBLES_REALIZATION;
            }
            else if (this == PREPARED_REGISTER)
            {
                view = Permission.CAN_ACCESS_PREPAREDREGISTER_VIEW;
                edit = Permission.CAN_ACCESS_PREPAREDREGISTER_CREATE;
                process = Permission.CAN_ACCESS_PREPAREDREGISTER_REALIZATION;
            }
            else if (this == PRODUCTION_ORDER)
            {
                view = Permission.CAN_ACCESS_PRODUCTIONORDER_VIEW;
                edit = Permission.CAN_ACCESS_PRODUCTIONORDER_CREATE;
                process = Permission.CAN_ACCESS_PRODUCTIONORDER_REALIZATION;
            }
            else if (this == PAYROLL)
            {
                view = Permission.CAN_VIEW_EMPLOYEES_PAYROLL;
                edit = Permission.CAN_EDIT_EMPLOYEES_PAYROLL;
            }
            else if (this == EDI_ORDER)
            {
                view = Permission.CAN_CREATE_EXTERNAL_ORDERS;
                edit = Permission.CAN_CREATE_EXTERNAL_ORDERS;
                process = Permission.CAN_CREATE_EXTERNAL_ORDERS;
            }
            // Незарегистрированный тип документа.
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Пытается для данного типа документа получить три коллекции прав (чтение, изменение, проведение).
        /// </summary>
        /// <returns><c>true</c> в случае успеха (известны права для данного типа документов), иначе - <c>false</c>.</returns>
        public bool TryGetPermissions(out List<Permission> viewPermissions, out List<Permission> editPermissions, out List<Permission> processPermissions, DocumentStatus? status = null)
        {
            viewPermissions = new List<Permission>();
            editPermissions = new List<Permission>();
            processPermissions = new List<Permission>();

            var success = TryGetPermissions(out var view, out var edit, out var process);
            if (success)
            {
                if (view != null)
                    viewPermissions.Add(view);
                if (edit != null)
                    editPermissions.Add(edit);
                if (process != null)
                    processPermissions.Add(process);

                if (status == DocumentStatus.PROCESSED)
                {
                    if (this == INCOMING_INVOICE)
                        editPermissions.Add(Permission.CAN_EDIT_PROCESSED_INCOMING_INVOICES);
                    else if (this == OUTGOING_INVOICE)
                        editPermissions.Add(Permission.CAN_EDIT_PROCESSED_OUTGOING_INVOICES);
                    else if (this == INTERNAL_TRANSFER)
                        editPermissions.Add(Permission.CAN_EDIT_PROCESSED_INTERNAL_INVOICES);
                    else if (this == WRITEOFF_DOCUMENT)
                        editPermissions.Add(Permission.CAN_EDIT_PROCESSED_WRITEOFFS);
                    else if (this == PRODUCTION_ORDER)
                        editPermissions.Add(Permission.CAN_EDIT_PROCESSED_PRODUCTIONORDER);
                }
            }

            return success;
        }

        /// <summary>
        /// Выясняет возможности (наличие прав) текущего пользователя по работе с данным типом документа.
        /// </summary>
        public void HasPermissions(out bool view, out bool edit, out bool process, DocumentStatus? status = null)
        {
            view = edit = process = false;

            var currentUser = ServerSession.CurrentSession.GetCurrentUser();
            if (currentUser != null && TryGetPermissions(out var viewPermissions, out var editPermissions, out var processPermissions, status))
            {
                view = viewPermissions.All(currentUser.HasPermission);
                edit = editPermissions.All(currentUser.HasPermission);
                process = processPermissions.All(currentUser.HasPermission);
            }
        }

        /// <summary>
        /// Возвращает признак наличия у текущего пользователя права на просмотр документов данного типа.
        /// </summary>
        public bool HasViewPermission(DocumentStatus? status = null)
        {
            HasPermissions(out var view, out _, out _, status);
            return view;
        }

        /// <summary>
        /// Возвращает признак наличия у текущего пользователя права на редактирование документов данного типа.
        /// </summary>
        public bool HasEditPermission(DocumentStatus? status = null)
        {
            HasPermissions(out _, out var edit, out _, status);
            return edit;
        }

        /// <summary>
        /// Возвращает признак наличия у текущего пользователя права на проведение документов данного типа.
        /// </summary>
        public bool HasProcessPermission(DocumentStatus? status = null)
        {
            HasPermissions(out _, out _, out var process, status);
            return process;
        }

        /// <summary>
        /// Выясняет наличие у текущего пользователя прав по работе с документами хотя бы одного из переданных типов и статусов
        /// </summary>
        /// <param name="documentInfos">Коллекция пар "тип документа-статус"</param>
        /// <param name="view">Признак наличия у пользователя права на просмотр хотя бы одного из документов, сведения о которых переданы методу</param>
        /// <param name="edit">Признак наличия у пользователя права на редактирование хотя бы одного из документов, сведения о которых переданы методу</param>
        /// <param name="process">Признак наличия у пользователя права на проведение хотя бы одного из документов, сведения о которых переданы методу</param>
        public static void HasPermissionsForAny(IEnumerable<Tuple<DocumentType, DocumentStatus?>> documentInfos, out bool view, out bool edit, out bool process)
        {
            view = edit = process = false;
            foreach (var documentInfo in documentInfos)
            {
                documentInfo.Item1.HasPermissions(out var viewCurrent, out var editCurrent, out var processCurrent, documentInfo.Item2);
                if (viewCurrent)
                    view = true;
                if (editCurrent)
                    edit = true;
                if (processCurrent)
                    process = true;
            }
        }

        /// <summary>
        /// Выясняет наличие у текущего пользователя прав по работе с документами хотя бы одного из переданных типов
        /// </summary>
        /// <param name="documentTypes">Коллекция типов документов</param>
        /// <param name="view">Признак наличия у пользователя права на просмотр хотя бы одного из типов документов</param>
        /// <param name="edit">Признак наличия у пользователя права на редактирование хотя бы одного из типов документов</param>
        /// <param name="process">Признак наличия у пользователя права на проведение хотя бы одного из типов документов</param>
        public static void HasPermissionsForAny(IEnumerable<DocumentType> documentTypes, out bool view, out bool edit, out bool process)
        {
            var documentInfos = documentTypes.Select(t => new Tuple<DocumentType, DocumentStatus?>(t, null));
            HasPermissionsForAny(documentInfos, out view, out edit, out process);
        }

        /// <summary>
        /// <para>Возвращает словарь соответствий между типами (классами) документов и значениями <see cref="DocumentType"/></para>
        /// </summary>
        /// <remarks>
        /// Этот словарь нужен только для функции копирования одного типа документа в другой, поэтому на самом деле здесь нужны
        /// маппинги не для всех типов документов, а только для тех, которые могут участвовать в этой функции. Раньше был тест,
        /// проверяющий, что есть маппинги для всех <see cref="DocumentType"/>. Удалил его как неактуальный, поскольку иногда
        /// <see cref="DocumentType"/> создаётся для несущствующего типа документа (например, <see cref="SCHEDULED_PAYMENT"/>,
        /// который нужен только для того, чтобы можно было воспользоваться стандартным механизмом генерации номеров документов).
        /// </remarks>
        private static Dictionary<Type, DocumentType> DocumentTypesMap =>
            documentTypesMap
            // отложенная инициализация используется по двум причинам:
            // 1. чтобы не ссылаться на статические члены класса при инициализации поля (что допустимо, но не изящно)
            // 2. чтобы не держать без необходимости в памяти словарь, пусть и небольшой, но нужный только в очень редких случаях
            ?? (documentTypesMap = new Dictionary<Type, DocumentType>
            {
                {typeof (IncomingInvoice), INCOMING_INVOICE},
                {typeof (IncomingInventory), INCOMING_INVENTORY},
                {typeof (IncomingService), INCOMING_SERVICE},
                {typeof (OutgoingService), OUTGOING_SERVICE},
                {typeof (WriteoffDocument), WRITEOFF_DOCUMENT},
                {typeof (SalesDocument), SALES_DOCUMENT},
                {typeof (ClosedSessionDocument), SESSION_ACCEPTANCE},
                {typeof (InternalTransfer), INTERNAL_TRANSFER},
                {typeof (OutgoingInvoice), OUTGOING_INVOICE},
                {typeof (ReturnedInvoice), RETURNED_INVOICE},
                {typeof (ProductionDocument), PRODUCTION_DOCUMENT},
                {typeof (TransformationDocument), TRANSFORMATION_DOCUMENT},
                {typeof (ProductionOrderDocument), PRODUCTION_ORDER},
                {typeof (ConsolidatedOrderDocument), CONSOLIDATED_ORDER},
                {typeof (PreparedRegisterDocument), PREPARED_REGISTER},
                {typeof (TreeMenuChangeDocument), MENU_CHANGE},
                {typeof (ProductReplacementDocument), PRODUCT_REPLACEMENT},
                {typeof (DisassembleDocument), DISASSEMBLE_DOCUMENT},
                {typeof (Payroll), PAYROLL},
                {typeof (IncomingCashOrder), INCOMING_CASH_ORDER},
                {typeof (OutgoingCashOrder), OUTGOING_CASH_ORDER},
                {typeof (EgaisBalanceStubDocument), EGAIS_BALANCE_QUERY},
                {typeof (EgaisQueryOrganizationStubDocument), EGAIS_QUERY_ORGANIZATION},
                {typeof (EgaisQueryProductStubDocument), EGAIS_QUERY_PRODUCT},
                {typeof (EgaisQueryFormAStubDocument), EGAIS_QUERY_FORM_A},
                {typeof (EgaisOutgoingInvoiceStubDocument), EGAIS_OUTGOING_INVOICE},
                {typeof (EgaisShopIncomingStubDocument), EGAIS_SHOP_INCOMING},
                {typeof (EgaisShopTransferStubDocument), EGAIS_SHOP_TRANSFER},
                {typeof (EgaisShopWriteoffStubDocument), EGAIS_SHOP_WRITEOFF},
                {typeof (EgaisMarkFixStubDocument), EGAIS_MARK_FIX},
                {typeof (EgaisMarkUnFixStubDocument), EGAIS_MARK_UNFIX},
                {typeof (EdiOrderDocument), EDI_ORDER},
                {typeof (VatInvoice), VAT_INVOICE},
            });

        /// <summary>
        /// <para>Возвращает копию словаря соответствий между классами документов и элементами <see cref="DocumentType"/>.</para>
        /// <para>Метод предназначен для тестирования корректности заполнения словаря.</para>
        /// </summary>
        public static IDictionary<Type, DocumentType> GetDocumentTypesMapCopy()
        {
            return new Dictionary<Type, DocumentType>(DocumentTypesMap);
        }

        /// <summary>
        /// Возвращает тип документа <see cref="DocumentType"/> по классу документа <see cref="TDocument"/>
        /// </summary>
        /// <typeparam name="TDocument">Тип документа, должен наследовать от <see cref="AbstractDocument"/></typeparam>
        [NotNull]
        public static DocumentType MapDocumentType<TDocument>() where TDocument : AbstractDocument
        {
            return DocumentTypesMap[typeof(TDocument)];
        }

        /// <summary>
        /// Список действующих в текущем релизе типов документов <see cref="DocumentType"/>.
        /// </summary>
        /// <remarks>
        /// RestoServer:resto.back.documents.DocumentType.getNonObsoleteValues()
        /// </remarks>
        public static IReadOnlyList<DocumentType> NonObsoleteValues =>
            nonObsoleteValues
            ?? (nonObsoleteValues = VALUES.Where(dt => !dt.Obsolete).ToList().AsReadOnly());

        /// <summary>
        /// Список разрешённых текущей лицензией типов документов <see cref="DocumentType"/>.
        /// </summary>
        /// <remarks>
        /// Использует <see cref="LicenseServiceInstance"/>
        /// RestoServer:resto.back.documents.DocumentType.getActiveValues()
        /// </remarks>
        public static IReadOnlyList<DocumentType> ActiveValues =>
            activeValues
            ?? (activeValues = VALUES.Where(dt => !dt.Obsolete && dt.IsAllowedByLicense).ToList().AsReadOnly());

    }
}