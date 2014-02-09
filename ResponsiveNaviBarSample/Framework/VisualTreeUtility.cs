#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace ResponsiveNaviBarSample.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media;

    /// <summary>
    /// VisualTree ユーティリティ
    /// </summary>
    public class VisualTreeUtility
    {
        /// <summary>
        /// 指定した型の子要素で最初に見つかったビジュアル要素を返す
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="root">探索対象のビジュアル要素</param>
        /// <returns>見つかった場合はその要素</returns>
        public static T FindChild<T>(DependencyObject root, string name = null) where T : FrameworkElement
        {
            var result = root as T;
            if (result != null && (string.IsNullOrEmpty(name) || name.Equals(result.Name)))
            {
                return result;
            }

            int childCount = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < childCount; i++)
            {
                var child = FindChild<T>(VisualTreeHelper.GetChild(root, i), name);
                if (child != null)
                {
                    return child;
                }
            }
            return null;
        }

        /// <summary>
        /// 指定した型の親要素で最初に見つかったビジュアル要素を返す
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="root">探索対象のビジュアル要素</param>
        /// <returns>見つかった場合はその要素</returns>
        public static T FindParent<T>(DependencyObject root, string name = null) where T : FrameworkElement
        {
            var result = root as T;
            if (result != null && (string.IsNullOrEmpty(name) || name.Equals(result.Name)))
            {
                return result;
            }

            var parent = FindParent<T>(VisualTreeHelper.GetParent(root), name);
            if (parent != null)
            {
                return parent;
            }

            return null;
        }
    }
}