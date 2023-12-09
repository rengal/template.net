// This file was generated with T4.
// Do not edit it manually.



namespace Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels
{
    /// <summary>
    /// Тип удаления позиции заказа
    /// </summary>
    public enum OrderEntryDeletionType
    {
        /// <summary>
        /// Без списания
        /// </summary>
        WithoutWriteoff,

        /// <summary>
        /// Списание за счёт сотрудника
        /// </summary>
        WriteoffAtTheExpenseOfEmployee,

        /// <summary>
        /// Списание за счёт заведения
        /// </summary>
        WriteoffAtTheExpenseOfCafe,

    }
}
