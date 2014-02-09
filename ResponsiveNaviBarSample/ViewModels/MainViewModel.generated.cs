﻿//<auto-generated>
namespace ResponsiveNaviBarSample.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// MainViewModel のインタフェース
    /// </summary>
    public partial interface IMainViewModel
    {
        bool IsLoaded { get; set; }
    }

    /// <summary>
    /// MainViewModel
    /// </summary>
    [DataContract]
    public partial class MainViewModel : ViewModelBase, IMainViewModel
    {
        #region IsLoaded:読み込み済みフラグ プロパティ
        /// <summary>
        /// 読み込み済みフラグ
        /// </summary>
        private bool isLoaded;

        /// <summary>
        /// 読み込み済みフラグ の変更前の処理
        /// </summary>
        partial void OnIsLoadedChanging(bool value);

        /// <summary>
        /// 読み込み済みフラグ の変更後の処理
        /// </summary>
        partial void OnIsLoadedChanged();

        /// <summary>
        /// 読み込み済みフラグ の取得および設定
        /// </summary>
        [IgnoreDataMember]
        public bool IsLoaded
        {
            get
            {
                return this.isLoaded;
            }

            set
            {
                if (this.isLoaded != value)
                {
                    this.OnIsLoadedChanging(value);
                    this.SetProperty<bool>(ref this.isLoaded, value);
                    this.OnIsLoadedChanged();
                }
            }
        }
        #endregion //IsLoaded:読み込み済みフラグ プロパティ

    }

}