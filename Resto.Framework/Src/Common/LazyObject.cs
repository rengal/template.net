using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public abstract class LazyObject
    {
        protected static readonly SynchronizedCollection<WeakReference> Refs = new SynchronizedCollection<WeakReference>();

        protected abstract void Reset();

        /// <summary>
        /// Метод, деинициализирующий все, созданные к моменту вызова lazy.
        /// Предназначен только для использования в тестах для деинициализации созданных
        /// lazy между тестами.
        /// Вызвается посредством рефлексии в Resto.Framework.Testing.TestFixtureBase.BaseTearDown().
        /// При изменении имени метода нужно изменить также строковые имена во всех неявных использованиях.
        /// </summary>
        /// <remarks>
        /// ResetLazyObjectAspect добавляется в TestDecorator по умолчанию, для очистки lazy между тестами
        /// нет необходимости помечать тест особенными аттрибутами.
        /// </remarks>
        [UsedImplicitly]
        private static void ResetAll()
        {
            lock (Refs.SyncRoot)
            {
                Refs.Select(wr => wr.Target).OfType<LazyObject>().ForEach(t => t.Reset());
            }
        }
    }

    /// <summary>
    /// Класс, реализующий ленивую инициализацию.
    /// </summary>
    /// <remarks>
    /// Изначально в качестве реализации ленивой инициализации использовался класс
    /// <see cref="Lazy{T}"/>, однако там нельзя было деиницилизировать (<see cref="Reset"/>) созданные lazy.
    /// Это приводило к зависимостям в тестах, если lazy были статическими полями объектов, так как деинициализации
    /// этих объектов  между тестами не проходило. 
    /// </remarks>
    public sealed class LazyObject<T> : LazyObject where T : class
    {
        private readonly Func<T> valueFactory;
        private T value;
        private readonly object syncLock = new object();
  
        /// <param name="valueFactory">Метод, конструирущий объект при первом обращении к <see cref="Value"/></param>
        public LazyObject(Func<T> valueFactory)
        {
            this.valueFactory = valueFactory;
            Refs.Add(new WeakReference(this));
        }

        /// <summary>
        /// Возвращает объект, создаваемый лениво (при первом запросе к этому свойству) посредством 
        /// <see cref="valueFactory"/>. Потокобезопасен - при обращении из разных потоков будет создан
        /// только один объект.
        /// </summary>
        /// <remarks>
        /// После первого вызова и до вызова <see cref="Reset"/> при обращении к свойству используется его
        /// закешированное значение (<see cref="valueFactory"/> не вызывается).
        /// </remarks>
        public T Value
        {
            get
            {
                if (value != null)
                    return value;

                lock (syncLock)
                {
                    if (value != null)
                        return value;

                    value = valueFactory();
                    return value;
                }
            }
        }

        /// <summary>
        /// Деинициализация. Скидывает закешированное значение, полученное вызовом <see cref="valueFactory"/>.
        /// После вызова этого метода обращение к <see cref="Value"/> снова инициирует вызов <see cref="valueFactory"/>.
        /// </summary>
        /// <remarks>
        /// Не является потокобезопасным, вызов этого метода одновременно со свойством Value 
        /// может привести к побочным эффектам. Нежелательно вызывать этот метод,
        /// кроме как из метода <see cref="LazyObject.ResetAll"/>.
        /// </remarks>
        protected override void Reset()
        {
            value = null;
        }
    }
}
