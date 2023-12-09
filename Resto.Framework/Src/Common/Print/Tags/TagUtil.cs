namespace Resto.Framework.Common.Print.Tags
{
    public sealed class TagUtil : TagBase
    {
        public static readonly TagUtil Bell;

        static TagUtil()
        {
            Bell = new TagUtil("bell");
        }

        private TagUtil(string name)
            : base(name)
        {}
    }
}