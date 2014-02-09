#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace ResponsiveNaviBarSample.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    /// <summary>
    /// アプリケーション設定情報
    /// </summary>
    public partial class ApplicationSettings : IApplicationSettings
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ApplicationSettings()
        {
            this.Photos = new List<PhotoItem>();
        }
    }
}
