using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.XmlSerialization.DeserializationContexts
{
    public sealed class XmlDeserializationStack
    {
        private const int DefaultMaxPathStackDepth = 10; // минимизируем увеличения буфера, считая, что большинство объектов имеют вложенность не больше 10

        private readonly Stack<Frame> frames = new Stack<Frame>(DefaultMaxPathStackDepth);

        public string ToDebugString()
        {
            var result = new StringBuilder();
            foreach (var frame in frames.Reverse())
            {
                switch (frame.Kind)
                {
                    case FrameKind.Entity:
                        if (frame.Name != null && frame.Type != null)
                            result.Append($"{frame.Type.Name} ({frame.Name})");
                        else if (frame.Name != null)
                            result.Append(frame.Name);
                        else
                            result.Append(frame.Type.Name);
                        break;

                    case FrameKind.Field:
                        result.Append(frame.Name);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                result.Append(" -> ");
            }
            return result.ToString();
        }

        public void PushEntity<T>([CanBeNull] string id)
        {
            frames.Push(new Frame(FrameKind.Entity, id, typeof(T)));
        }

        public void PopEntity()
        {
            frames.Pop();
        }

        public void PushField(string fieldName)
        {
            frames.Push(new Frame(FrameKind.Field, fieldName, null));
        }

        public void PopField()
        {
            frames.Pop();
        }

        private struct Frame
        {
            public readonly FrameKind Kind;
            [CanBeNull]
            public readonly string Name;
            [CanBeNull]
            public readonly Type Type;

            public Frame(FrameKind kind, [CanBeNull] string name, [CanBeNull] Type type)
            {
                Kind = kind;
                Name = name;
                Type = type;
            }
        }

        private enum FrameKind
        {
            Entity,
            Field
        }
    }
}
