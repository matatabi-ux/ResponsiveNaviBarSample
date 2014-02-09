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
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices.WindowsRuntime;
    using ResponsiveNaviBarSample.Common;
    using ResponsiveNaviBarSample.Presenters;
    using Windows.Foundation;
    using Windows.Foundation.Collections;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Navigation;
    using ResponsiveNaviBarSample.ViewModels;

    /// <summary>
    /// トップ画面
    /// </summary>
    public sealed partial class TopPage : PageBase, IView<TopPagePresenter>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TopPage()
            : base()
        {
            this.InitializeComponent();
        }

        #region IView<TPresenter>

        /// <summary>
        /// この画面の Presenter
        /// </summary>
        public TopPagePresenter Presenter
        {
            get { return this.PagePresenter as TopPagePresenter; }
        }

        #endregion //IView<TPresenter>

        /// <summary>
        /// アイテムクリックイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            this.FlipView.SelectedItem = e.ClickedItem;
        }
    }
}
