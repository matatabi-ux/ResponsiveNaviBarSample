#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace ResponsiveNaviBarSample.ViewModels
{
    using Microsoft.Practices.ServiceLocation;
    using ResponsiveNaviBarSample.Entities;
    using ResponsiveNaviBarSample.Framework;
    using ResponsiveNaviBarSample.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// トップ画面の ViewModel
    /// </summary>
    public partial class TopPageViewModel : ViewModelBase, ITopPageViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TopPageViewModel()
        {
            this.photos = new ObservableCollection<PhotoViewModel>();
            if(!ModuleContainer.Instance.IsRegistered<ApplicationSettingsModel>())
            {
                return;
            }

            foreach (var item in ServiceLocator.Current.GetInstance<ApplicationSettingsModel>().Settings.Photos)
            {
                this.photos.Add(PhotoViewModel.Convert(item));
            }
        }
    }

    /// <summary>
    /// トップ画面の ViewModel のインタフェース
    /// </summary>
    public partial interface ITopPageViewModel
    {
    }
}
