using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#pragma warning disable 642
namespace Resto.Framework.Common.Helpers
{
    public static class VisualExtensions
    {
        public static IEnumerable<DependencyObject> GetChildren(this Visual visual)
        {
            // <ap> You probably wonder, "WTF!?" I wonder too.
            // <ap> Sometimes during creation of control tree, Content is already assigned, but VisualTreeHelper.GetChildrenCount still returns null...
            var contentCtl = visual as ContentControl;
            return VisualTreeHelper.GetChildrenCount(visual) == 0 && contentCtl != null && contentCtl.Content as Visual != null
                ? EnumerableEx.Return((DependencyObject)contentCtl.Content)
                : new VisualChildrenEnumerable(visual);
        }

        public static DependencyObject GetParent(this Visual visual)
        {
            return VisualTreeHelper.GetParent(visual);
        }

        public static TParent GetParent<TParent>(this Visual visual)
            where TParent : class
        {
            DependencyObject ctl;
            for (ctl = visual; !(ctl is TParent); ctl = VisualTreeHelper.GetParent(ctl))
                ;
            return ctl as TParent;
        }

        public static Visual GetRootParent(this Visual visual)
        {
            DependencyObject root = visual;
            for (var d = root; d != null; d = VisualTreeHelper.GetParent(d))
                root = d;
            return (Visual)root;
        }

        public static IEnumerable<DependencyObject> GetFullParentPath(this Visual visual)
        {
            for (DependencyObject d = visual; d != null; d = VisualTreeHelper.GetParent(d))
                yield return d;
        }

        public static TControl FindChild<TControl, TProp>(this Visual parent, DependencyProperty prop, TProp value)
            where TControl : DependencyObject
        {
            foreach (var child in parent.GetChildren())
            {
                var propValue = child.GetValue(prop);
                if (child is TControl && (propValue is TProp || Equals(propValue, null)) && Equals((TProp)propValue, value))
                    return (TControl)child;
                var result = child is Visual ? FindChild<TControl, TProp>((Visual)child, prop, value) : null;
                if (result != null)
                    return result;
            }
            return null;
        }

        public static bool IsControlActive(this UIElement control)
        {
            return control.GetFullParentPath().OfType<UIElement>().All(c => c.IsEnabled && c.IsVisible);
        }

        public static bool IsControlVisible(this UIElement control)
        {
            return control.GetFullParentPath().OfType<UIElement>().All(c => c.IsVisible);
        }

        private class VisualChildrenEnumerable : IEnumerable<DependencyObject>
        {
            private int Count { get; set; }
            private Visual Visual { get; set; }

            public VisualChildrenEnumerable(Visual visual)
            {
                Visual = visual;
                Count = VisualTreeHelper.GetChildrenCount(visual);
            }

            public IEnumerator<DependencyObject> GetEnumerator()
            {
                for (var i = 0; i < Count; i++)
                    yield return VisualTreeHelper.GetChild(Visual, i);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
