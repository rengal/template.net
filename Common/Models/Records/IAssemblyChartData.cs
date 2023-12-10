using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using Resto.Data;

namespace Resto.Common.Models
{
    /// <summary>
    /// Данные тех. карты.
    /// Промежуточный интерфейс, служащий для постороения модели для шаблона
    /// <see cref="ModelAssemblyChart"/>
    /// </summary>
    public interface IAssemblyChartData
    {
        /// <summary>
        /// Руководитель.
        /// </summary>
        string LeaderName { get;  }

        /// <summary>
        /// Ответственный разработчик.
        /// </summary>
        string TechnologistName { get;  }

        /// <summary>
        /// Заведующий производством
        /// </summary>
        string WorksManager { get;  }

        /// <summary>
        /// Дата начала тех. карты.
        /// </summary>
        DateTime ACStartDate { get;  }

        /// <summary>
        /// Номенклатурный код продукта.
        /// </summary>
        string NomenclatureCode { get; }

        /// <summary>
        /// Пищевая и энергетическая ценность (Жиры на 100 г. продукта).
        /// </summary>
        decimal FatPer100Gramm { get; }

        /// <summary>
        /// Пищевая и энергетическая ценность (Углеводы на 100 г. продукта).
        /// </summary>
        decimal CarbohydratePer100Gramm { get; }

        /// <summary>
        /// Пищевая и энергетическая ценность (Белки на 100 г. продукта).
        /// </summary>
        decimal FiberPer100Gramm { get; }

        /// <summary>
        /// Пищевая и энергетическая ценность (ККал на 100 г. продукта).
        /// </summary>
        decimal CaloriesPer100Gramm { get;  }

        /// <summary>
        /// Юридическое лицо.
        /// </summary>
        string JurPerson { get; }

        /// <summary>
        /// Список подразделений
        /// </summary>
        string Departments { get; }

        /// <summary>
        /// Суммарный выход.
        /// </summary>
        string CustomOutString { get; }

        /// <summary>
        /// Продукт с размером
        /// </summary>
        ProductSizeKey ProductSizeKey { get; }

        /// <summary>
        /// Подразделение
        /// </summary>
        DepartmentEntity Department { get; }

        /// <summary>
        /// Суммарный выход
        /// </summary>
        decimal FullOut { get; }

        /// <summary>
        /// Норма закладки
        /// </summary>
        decimal AssembledAmount { get; }

        /// <summary>
        /// Комментарий к суммарному выходу
        /// </summary>
        string OutputComment { get; }

        /// <summary>
        /// Технология приготовления
        /// </summary>
        string TechnologyDescription { get; }

        /// <summary>
        /// Требования к оформлению, подаче и реализации
        /// </summary>
        string Appearance { get; }

        /// <summary>
        /// Органолептические показатели
        /// </summary>
        string Organoleptic { get; }

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

        /// <summary>
        /// Строки тех. карты (ингредиенты)
        /// </summary>
        IEnumerable<IAssemblyChartItem> Items { get; }

        /// <summary>
        /// Оценочная себестоимость ингредиента (за единицу)
        /// </summary>
        [NotNull]
        EvaluableDecimalValue GetCostPricePerUnit([NotNull] Product ingredient);

        /// <summary>
        /// Инициализация данных
        /// </summary>
        void Init();
    }
}