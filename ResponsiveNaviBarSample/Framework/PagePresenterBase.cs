﻿//<auto-generated>
#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace ResponsiveNaviBarSample.Presenters
{
    using ResponsiveNaviBarSample.Common;
    using ResponsiveNaviBarSample.Framework;
    using ResponsiveNaviBarSample.ViewModels;
    using ResponsiveNaviBarSample.Views;

    /// <summary>
    /// 画面用 Presenter
    /// </summary>
    public class PagePresenterBase<TView, TViewModel> : IPresenter, ICleanup
        where TView : PageBase
        where TViewModel : ViewModelBase, new()
    {
        #region Privates

        /// <summary>
        /// Page
        /// </summary>
        private PageBase page;

        /// <summary>
        /// PageViewModel
        /// </summary>
        private ViewModelBase pageViewModel;

        #endregion //Privates

        /// <summary>
        /// Page
        /// </summary>
        public PageBase Page
        {
            get
            {
                return this.page;
            }

            set
            {
                this.page = value;
            }
        }

        /// <summary>
        /// PageViewModel
        /// </summary>
        public ViewModelBase PageViewModel
        {
            get
            {
                return this.pageViewModel;
            }

            set
            {
                this.pageViewModel = value;
            }
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public virtual void Initialize()
        {
            this.Page.NavigationHelper.LoadState += this.OnLoadState;
            this.Page.NavigationHelper.SaveState += this.OnSaveState;

            this.PageViewModel = ViewModelLocator.Get<TViewModel>();
            this.PageViewModel.Initialize();
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public virtual void Discard()
        {
            this.Cleanup();
        }

        /// <summary>
        /// 状態読み込み処理
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        protected virtual void OnLoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// 状態保存処理
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        protected virtual void OnSaveState(object sender, SaveStateEventArgs e)
        {
        }

        /// <summary>
        /// インスタンスを解放します
        /// </summary>
        public virtual void Cleanup()
        {
            this.PageViewModel.Discard();

            this.Page.NavigationHelper.LoadState -= this.OnLoadState;
            this.Page.NavigationHelper.SaveState -= this.OnSaveState;
        }

    }
}
