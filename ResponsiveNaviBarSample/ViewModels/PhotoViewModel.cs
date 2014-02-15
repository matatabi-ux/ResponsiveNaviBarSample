#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace ResponsiveNaviBarSample.ViewModels
{
    using ResponsiveNaviBarSample.Entities;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// PhotoViewModel
    /// </summary>
    public partial class PhotoViewModel : ViewModelBase, IPhotoViewModel
    {
        /// <summary>
        /// Entity から ViewModel を生成する
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>ViewModel</returns>
        public static PhotoViewModel Convert(PhotoItem entity)
        {
            return new PhotoViewModel()
            {
                UniqueId = Guid.NewGuid().ToString(),
                Title = entity.Title,
                ImageUri = entity.ImageUri,
                OwnerName = entity.OwnerName,
            };
        }
    }
}
