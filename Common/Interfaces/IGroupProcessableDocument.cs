using System;
using Resto.Data;

namespace Resto.Common.Interfaces
{
    /// <summary>
    /// Интерфейс, который должны реализовывать классы, с экземплярами которых проводятся групповые
    /// операции над документами (проведение / распроведение, удаление, и т.д.). Групповые операции
    /// реализованы в классах, имплементирующих интерфейс <see cref="IGroupDocumentProcessor{T}"/>
    /// </summary>
    public interface IGroupProcessableDocument
    {
        Guid? DocumentID { get; }
        string Number { get; }
        DocumentType Type { get; }
        DateTime? Date { get; }
        bool Deleted { get; }
        bool Processed { get; }
    }
}