using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class CorporatedEntity : INamed, ICorporatedEntityProps
    {
        #region ICorporatedEntityProps Members

        public string CEDescription
        {
            get { return Description; }
            set { Description = value; }
        }

        public PersistedEntity CEParent
        {
            get { return Parent; }
            set
            {
                var corporatedEntity = value as CorporatedEntity;
                if (corporatedEntity == null)
                {
                    throw new ArgumentException("CEParent must be CorporatedEntity");
                }
                Parent = corporatedEntity;
            }
        }

        public CorporatedEntityType CEType
        {
            get { return GetCEType(); }
        }

        protected virtual CorporatedEntityType GetCEType()
        {
            // CEType должен имплементироваться в наследниках
            throw new NotImplementedException("CEType is not implemented");
        }

        #endregion

        #region INamed Members

        public string NameLocal 
        {
            get { return Name; }
            set { Name = value; }
        }

        #endregion

        public override string ToString()
        {
            return NameLocal;
        }

        public override bool Equals(object obj)
        {
            var entity = obj as CorporatedEntity;
            return entity != null && entity.Id.Equals(Id);
        }

        /// <summary>
        /// Метод возвращает признак того что заданная корпоративная единица является дочерней по отношению к <see cref="corporatedEntity"/>
        /// </summary>
        public bool IsChildOf([NotNull]CorporatedEntity corporatedEntity)
        {
            if (corporatedEntity == null) throw new ArgumentNullException(nameof(corporatedEntity));

            CorporatedEntity parent = Parent;
            while (parent != null)
            {
                if (parent.Equals(corporatedEntity))
                {
                    return true;
                }
                parent = parent.parent;
            }
            return false;
        }

        /// <summary>
        /// Метод возвращает список всех родителей заданной единицы корпорации
        /// </summary>
        [NotNull]
        public IEnumerable<CorporatedEntity> GetAllParents()
        {
            CorporatedEntity current = parent;
            while (current != null)
            {
                yield return current;
                current = current.parent;
            }
        }

        /// <summary>
        /// Метод возвращает список всех детей заданной единицы корпорации
        /// </summary>
        [NotNull]
        public IEnumerable<CorporatedEntity> GetAllChildren()
        {
            IEnumerable<CorporatedEntity> corporatedEntities =
                EntityManager.INSTANCE.GetAllNotDeleted<CorporatedEntity>();
            foreach (CorporatedEntity entity in corporatedEntities)
            {
                if (entity.IsChildOf(this))
                {
                    yield return entity;
                }
            }
        }

        /// <summary>
        /// Метод возвращает список всех детей заданной единицы корпорации, типа T
        /// </summary>
        [NotNull]
        public IEnumerable<T> GetAllChildren<T>()
        {
            return GetAllChildren().OfType<T>();
        }
    }
}