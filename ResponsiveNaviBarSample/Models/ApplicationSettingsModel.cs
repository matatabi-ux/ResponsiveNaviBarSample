#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace ResponsiveNaviBarSample.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Serialization;
    using ResponsiveNaviBarSample.Entities;
    using Windows.Storage;

    /// <summary>
    /// アプリケーション設定情報モデル
    /// </summary>
    public class ApplicationSettingsModel
    {
        #region Privates

        /// <summary>
        /// 設定情報保存先フォルダ
        /// </summary>
        private static readonly StorageFolder StoreFolder = ApplicationData.Current.LocalFolder;

        /// <summary>
        /// 設定情報ファイル名
        /// </summary>
        private static readonly string FileName = @"app-settings.xml";

        /// <summary>
        /// 初期値設定情報ファイル名
        /// </summary>
        private static readonly string DefaultFilePath = @"ms-appx:///Assets/Data/default-app-settings.xml";

        /// <summary>
        /// 排他処理制御オブジェクト
        /// </summary>
        private static readonly Semaphore LockObject = new Semaphore(1, 1);

        #endregion //Privates

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static ApplicationSettingsModel()
        {
        }

        /// <summary>
        /// 設定情報
        /// </summary>
        public ApplicationSettings Settings { get; private set; }

        /// <summary>
        /// 設定情報を読み込みます
        /// </summary>
        /// <returns>成功した場合は true, それ以外は false</returns>
        public async Task<bool> LoadAsync()
        {
            try
            {
                var file = await StoreFolder.GetFileAsync(FileName);
                using (var stream = await file.OpenSequentialReadAsync())
                {
                    var serializer = new XmlSerializer(typeof(ApplicationSettings));
                    this.Settings = serializer.Deserialize(stream.AsStreamForRead()) as ApplicationSettings;
                }

                return true;
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine(string.Format("ApplicationSettingsModel::LoadAsync(): {0} が見つかりません。デフォルトの設定情報を読み込みます。", FileName));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            try
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(DefaultFilePath, UriKind.Absolute));
                using (var stream = await file.OpenSequentialReadAsync())
                {
                    var serializer = new XmlSerializer(typeof(ApplicationSettings));
                    this.Settings = serializer.Deserialize(stream.AsStreamForRead()) as ApplicationSettings;
                }
                await this.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// 設定情報を保存します
        /// </summary>
        /// <returns>Task</returns>
        public async Task SaveAsync()
        {
            try
            {
                LockObject.WaitOne();

                var file = await StoreFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
                using (var stream = await file.OpenStreamForWriteAsync())
                {
                    var wiriter = XmlWriter.Create(
                        stream,
                        new XmlWriterSettings()
                        {
                            Indent = false,
                            Encoding = Encoding.UTF8,
                            NewLineOnAttributes = false,
                        });

                    var serializer = new XmlSerializer(typeof(ApplicationSettings));
                    serializer.Serialize(wiriter, this.Settings);
                }

                LockObject.Release();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
