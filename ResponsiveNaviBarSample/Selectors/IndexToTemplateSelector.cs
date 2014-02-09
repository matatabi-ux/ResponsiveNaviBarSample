#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace ResponsiveNaviBarSample.Selectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Markup;

    /// <summary>
    /// XAML 定義用 DataTemlate コレクションクラス
    /// </summary>
    [ContentProperty()]
    public class DataTemplateCollection : List<DataTemplate> { }

    /// <summary>
    /// Index を DataTemplate に変換する Selector
    /// </summary>
    public class IndexToTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// テンプレートリスト
        /// </summary>
        public DataTemplateCollection Templates { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IndexToTemplateSelector()
            : base()
        {
            this.Templates = new DataTemplateCollection();
        }

        /// <summary>
        /// Index から対応する DataTemplate を返却します
        /// </summary>
        /// <param name="item">int値</param>
        /// <param name="container">コンテナ</param>
        /// <returns>DataTemplate</returns>
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            int index = 0;

            if (item != null && int.TryParse(item.ToString(), out index) && Templates.Count > index)
            {
                return this.Templates[index];
            }

            return null;
        }
    }
}
