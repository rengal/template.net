using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.XmlSerialization.SerializationContexts;
using Resto.Framework.Common.XmlSerialization.Serializers;

namespace Resto.Framework.Common.XmlSerialization
{
    public sealed class ArgumentsSerializer
    {
        #region Inner Types
        private abstract class Argument
        {
            #region Inner Types
            private sealed class SealedTypeArgument : Argument
            {
                [NotNull]
                private readonly ISerializer serializer;

                internal SealedTypeArgument([NotNull] string name, [NotNull] Type type)
                    : base(name, type)
                {
                    Debug.Assert(type.IsSealed);

                    serializer = SerializersManager.GetSerializer(type);
                }

                internal override void Serialize([NotNull] XmlWriter writer, [NotNull] object value, [NotNull] ISerializationContext context)
                {
                    Debug.Assert(writer != null);
                    Debug.Assert(value != null);
                    Debug.Assert(context != null);


                    serializer.WriteObjectToElement(name, writer, type, value, false, context, SerializationMetadata.Empty);
                }
            }

            private sealed class NonSealedTypeArgument : Argument
            {
                internal NonSealedTypeArgument([NotNull] string name, [NotNull] Type type)
                    : base(name, type)
                {
                    Debug.Assert(!type.IsSealed);
                }

                internal override void Serialize([NotNull] XmlWriter writer, [NotNull] object value, [NotNull] ISerializationContext context)
                {
                    Debug.Assert(writer != null);
                    Debug.Assert(value != null);
                    Debug.Assert(context != null);


                    SerializersManager.GetSerializer(value.GetType()).WriteObjectToElement(name, writer, type, value, false, context, SerializationMetadata.Empty);
                }
            }
            #endregion

            #region Factory
            private static readonly ThreadSafeCache<Pair<string, Type>, Argument> ArgumentsCache =
                new ThreadSafeCache<Pair<string, Type>, Argument>(Create);

            private static Argument Create(Pair<string, Type> nameAndType)
            {
                Debug.Assert(nameAndType.First != null);
                Debug.Assert(nameAndType.Second != null);

                if (nameAndType.Second.IsSealed)
                    return new SealedTypeArgument(nameAndType.First, nameAndType.Second);

                return new NonSealedTypeArgument(nameAndType.First, nameAndType.Second);
            }

            internal static Argument GetFor([NotNull] string name, [NotNull] Type type)
            {
                Debug.Assert(name != null);
                Debug.Assert(type != null);

                return ArgumentsCache[new Pair<string, Type>(name, type)];
            }
            #endregion

            #region Fields
            [NotNull]
            private readonly string name;
            [NotNull]
            private readonly Type type;
            #endregion

            #region Ctor
            private Argument([NotNull] string name, [NotNull] Type type)
            {
                Debug.Assert(name != null);
                Debug.Assert(type != null);

                this.name = name;
                this.type = type;
            }
            #endregion

            #region Methods
            internal abstract void Serialize([NotNull] XmlWriter writer, [NotNull] object value, [NotNull] ISerializationContext context);
            #endregion
        }
        #endregion

        [NotNull]
        private readonly string description;
        private readonly List<Argument> arguments = new List<Argument>();

        public ArgumentsSerializer([NotNull] string description)
        {
            if (description == null)
                throw new ArgumentNullException(nameof(description));

            this.description = description;
        }

        public void AddArgument([NotNull] string name, [NotNull] Type type)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            arguments.Add(Argument.GetFor(name, type));
        }

        public void WriteArguments([NotNull] XmlWriter writer, [NotNull] object[] argumentValues, [NotNull] ISerializationContext context)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (argumentValues == null)
                throw new ArgumentNullException(nameof(argumentValues));
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (argumentValues.Length != arguments.Count)
                throw new SerializerException(string.Format("Illegal count of values: {0} != {1}. {2}", arguments.Count, argumentValues.Length, description));


            for (var i = 0; i < arguments.Count; i++)
            {
                var value = argumentValues[i];
                if (value == null)
                    continue;

                var argument = arguments[i];
                argument.Serialize(writer, value, context);
            }
        }
    }
}