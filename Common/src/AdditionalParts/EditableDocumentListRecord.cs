namespace Resto.Data
{
    partial class EditableDocumentListRecord
    {
        public override EditableDocumentType EditableDocumentType
        {
            get { return EditableDocumentType.GetDocumentType(this); }
        }
    }
}