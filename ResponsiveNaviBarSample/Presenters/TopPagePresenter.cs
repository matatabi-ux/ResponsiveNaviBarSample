#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace ResponsiveNaviBarSample.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ResponsiveNaviBarSample.ViewModels;
    using ResponsiveNaviBarSample.Views;

    /// <summary>
    /// TOP 画面の Presenter
    /// </summary>
    public class TopPagePresenter : PagePresenterBase<TopPage, TopPageViewModel>, IPresenter<TopPage, TopPageViewModel>
    {
        #region IPresenter<TView, TViewModel>

        /// <summary>
        /// View
        /// </summary>
        public TopPage View
        {
            get
            {
                return this.Page as TopPage;
            }

            set
            {
                this.Page = value;
            }
        }

        /// <summary>
        /// ViewModel
        /// </summary>
        public TopPageViewModel ViewModel
        {
            get
            {
                return this.PageViewModel as TopPageViewModel;
            }

            set
            {
                this.PageViewModel = value;
            }
        }

        #endregion //IPresenter<TView, TViewModel>

        /// <summary>
        /// 状態読み込み処理
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        protected override void OnLoadState(object sender, Common.LoadStateEventArgs e)
        {
            base.OnLoadState(sender, e);

            if (e.PageState != null && e.PageState.ContainsKey(this.ViewModel.GetType().Name))
            {
                this.ViewModel = e.PageState[this.ViewModel.GetType().Name] as TopPageViewModel;
            }

            this.View.DataContext = this.ViewModel;
        }

        /// <summary>
        /// 状態保存処理
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        protected override void OnSaveState(object sender, Common.SaveStateEventArgs e)
        {
            e.PageState[this.ViewModel.GetType().Name] = this.ViewModel;

            base.OnSaveState(sender, e);
        }
    }
}
