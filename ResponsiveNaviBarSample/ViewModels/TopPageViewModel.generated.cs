﻿//<auto-generated>
#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

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
    /// トップ画面の ViewModel のインタフェース
    /// </summary>
    public partial interface ITopPageViewModel
    {
        IList<PhotoViewModel> Photos { get; set; }
    }

    /// <summary>
    /// トップ画面の ViewModel
    /// </summary>
    [DataContract]
    public partial class TopPageViewModel : ViewModelBase, ITopPageViewModel
    {
        #region Photos:写真情報 プロパティ
        /// <summary>
        /// 写真情報
        /// </summary>
        private IList<PhotoViewModel> photos; 

        /// <summary>
        /// 写真情報 の変更前の処理
        /// </summary>
        partial void OnPhotosChanging(IList<PhotoViewModel> value);

        /// <summary>
        /// 写真情報 の変更後の処理
        /// </summary>
        partial void OnPhotosChanged();

        /// <summary>
        /// 写真情報 の取得および設定
        /// </summary>
        [IgnoreDataMember]
        public IList<PhotoViewModel> Photos
        {
            get
            {
                return this.photos;
            }

            set
            {
                if (this.photos != value)
                {
                    this.OnPhotosChanging(value);
                    this.SetProperty<IList<PhotoViewModel>>(ref this.photos, value);
                    this.OnPhotosChanged();
                }
            }
        }
        #endregion //Photos:写真情報 プロパティ

    }

}