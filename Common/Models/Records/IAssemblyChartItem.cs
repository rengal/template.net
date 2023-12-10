using System;
using Resto.Data;

namespace Resto.Common.Models
{
    /// <summary>
    /// Данные строки тех. карты (ингредиента).
    /// Промежуточный интерфейс, служащий для постороения модели для шаблона
    /// <see cref="ModelAssemblyChartItem"/>
    /// </summary>
    public interface IAssemblyChartItem
    {
        /// <summary>
        /// Нетто, ед. изм.
        /// </summary>
        decimal AmountMiddle { get; }

        /// <summary>
        /// Выход, ед. изм.
        /// </summary>
        decimal AmountOut { get; }

        /// <summary>
        /// Технология приготовления
        /// </summary>
        string TechnologyString { get; }

        /// <summary>
        /// Id элемента
        /// </summary>
        Guid ItemId { get; }

        /// <summary>
        /// Id родительского элемента
        /// </summary>
        Guid ItemParentId { get; }

        /// <summary>
        /// Количество на порцию
        /// </summary>
        decimal AmountPerPortion { get; }

        /// <summary>
        /// Количество потерь
        /// </summary>
        decimal LossesCount { get; }

        /// <summary>
        /// Потери в %
        /// </summary>
        decimal LossesPercent { get; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        int Number { get; }

        /// <summary>
        /// Интервал действия тех. карты
        /// </summary>
        string DateInterval { get; }

        /// <summary>
        /// Продукт
        /// </summary>
        Product Product { get; }

        /// <summary>
        /// Уровень вложенности
        /// </summary>
        int TreeLevel { get; }

        /// <summary>
        /// Фасовка
        /// </summary>
        Container Package { get; }

        /// <summary>
        /// Кол-во в фасовке
        /// </summary>
        decimal PackageCount { get; }

        /// <summary>
        /// Брутто, ед. изм.
        /// </summary>
        decimal AmountIn { get; }

        /// <summary>
        /// Брутто, кг
        /// </summary>
        decimal AmountInMU { get; }

        /// <summary>
        /// Нетто, кг
        /// </summary>
        decimal AmountMiddleMU { get; }

        /// <summary>
        /// Выход, кг
        /// </summary>
        decimal AmountOutMU { get; }

        /// <summary>
        /// Брутто на 1 порц., г
        /// </summary>
        decimal AmountInGrammOnePortion { get; set; }

        /// <summary>
        /// Нетто на 1 порц., г
        /// </summary>
        decimal AmountMiddleGrammOnePortion { get; set; }

        /// <summary>
        /// Выход на 1 порц., г
        /// </summary>
        decimal AmountOutGrammOnePortion { get; set; }

        /// <summary>
        /// Потери при холодной обработке, %
        /// </summary>
        decimal ColdLossPercent { get; }

        /// <summary>
        /// Потери при горячей обработке, %
        /// </summary>
        decimal HotLossPercent { get; }

        /// <summary>
        /// Коды групп аллергенов
        /// </summary>
        string AllergenGroups { get; }

        /// <summary>
        /// Группы аллергенов.
        /// <para>
        /// Отображается в виде [код - наименование]
        /// </para>
        /// </summary>
        string AllergenGroupsFull { get; }
    }
}