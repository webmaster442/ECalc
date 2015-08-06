using ECalc.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ECalc.Classes
{
    class Helpers
    {
        public static bool IsComplex(object o)
        {
            return o is Complex;
        }

        public static Complex GetComplex(object o)
        {
            if (IsComplex(o)) return (Complex)o;
            else return new Complex((double)o, 0);
        }

        public static void ErrorDialog(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;
            T foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }
            return foundChild;
        }

        public static IEnumerable<T> FindChildren<T>(DependencyObject source) where T : DependencyObject
        {
            if (source != null)
            {
                var childs = GetChildObjects(source);
                foreach (DependencyObject child in childs)
                {
                    //analyze if children match the requested type
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    //recurse tree
                    foreach (T descendant in FindChildren<T>(child))
                    {
                        yield return descendant;
                    }
                }
            }
        }

        public static IEnumerable<DependencyObject> GetChildObjects(DependencyObject parent)
        {
            if (parent == null) yield break;

            if (parent is ContentElement || parent is FrameworkElement)
            {
                //use the logical tree for content / framework elements
                foreach (object obj in LogicalTreeHelper.GetChildren(parent))
                {
                    var depObj = obj as DependencyObject;
                    if (depObj != null) yield return (DependencyObject)obj;
                }
            }
            else
            {
                //use the visual tree per default
                int count = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < count; i++)
                {
                    yield return VisualTreeHelper.GetChild(parent, i);
                }
            }
        }

        public static string ResultToString(object o)
        {
            StringBuilder sb = new StringBuilder();
            if (IsComplex(o))
            {
                Complex cplx = GetComplex(o);
                sb.Append("R: ");
                sb.Append(cplx.Real);
                sb.Append(" i: ");
                sb.Append(cplx.Imaginary);
                sb.Append(" φ: ");
                switch (Engine.Mode)
                {
                    case TrigMode.DEG:
                        sb.Append(TrigFunctions.Rad2Deg(cplx.Phase));
                        sb.Append(" °");
                        break;
                    case TrigMode.GRAD:
                        sb.Append(TrigFunctions.Rad2Grad(cplx.Phase));
                        sb.Append(" grad");
                        break;
                    case TrigMode.RAD:
                        sb.Append(cplx.Phase);
                        sb.Append(" rad");
                        break;
                }

                return sb.ToString();
            }
            else return o.ToString();
        }
    }
}
