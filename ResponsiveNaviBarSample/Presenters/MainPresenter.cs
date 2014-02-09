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
    using Microsoft.Practices.ServiceLocation;
    using ResponsiveNaviBarSample.Common;
    using ResponsiveNaviBarSample.Framework;
    using ResponsiveNaviBarSample.Models;
    using ResponsiveNaviBarSample.ViewModels;
    using ResponsiveNaviBarSample.Views;
    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;
    using Windows.UI.ApplicationSettings;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// メイン Presenter
    /// </summary>
    public class MainPresenter : IPresenter
    {
        #region Privates

        /// <summary>
        /// MainPresenter
        /// </summary>
        private static MainPresenter instance;

        /// <summary>
        /// アプリケーション
        /// </summary>
        private App app;

        /// <summary>
        /// Frame
        /// </summary>
        private Frame rootFrame;

        #endregion //Privates

        /// <summary>
        /// アプリケーション
        /// </summary>
        public App View
        {
            get
            {
                return this.app;
            }

            set
            {
                this.app = value;
            }
        }

        /// <summary>
        /// メイン PageViewModel
        /// </summary>
        public MainViewModel ViewModel
        {
            get
            {
                return ViewModelLocator.Get<MainViewModel>();
            }

            set
            {
            }
        }

        #region Singleton

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private MainPresenter()
        {
        }

        /// <summary>
        /// MainPresenter
        /// </summary>
        public static MainPresenter Instance
        {
            get
            {
                return instance ?? (instance = new MainPresenter());
            }
        }

        #endregion //Singleton

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            this.View = App.Current as App;

            // セッション情報のシリアライズのために保存クラスを SuspensionManager に共有する
            SuspensionManager.KnownTypes.Add(typeof(BindableBase));
            SuspensionManager.KnownTypes.Add(typeof(ViewModelBase));
            SuspensionManager.KnownTypes.Add(typeof(MainViewModel));
            SuspensionManager.KnownTypes.Add(typeof(TopPageViewModel));

            this.View.Suspending += this.OnSuspending;

            ServiceLocator.SetLocatorProvider(() => ModuleContainer.Instance);

            ModuleContainer.Instance.Register<ApplicationSettingsModel>();

            PresenterLocator.Set<MainPresenter>(this);

            // Presenter と Page のひもづけ
            PresenterLocator.Register<TopPagePresenter, TopPage>();

            this.ViewModel.Initialize();
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="e">起動要求とプロセスの詳細を表示します。</param>
        /// <returns>Task</returns>
        public async Task InitializeAsync(LaunchActivatedEventArgs e)
        {
            this.Initialize();

            await ServiceLocator.Current.GetInstance<ApplicationSettingsModel>().LoadAsync();

            this.rootFrame = Window.Current.Content as Frame;

            /* ウィンドウに既にコンテンツが表示されている場合は、アプリケーションの初期化を繰り返さずに、
               ウィンドウがアクティブであることだけを確認してください */

            if (this.rootFrame == null)
            {
                // ナビゲーション コンテキストとして動作するフレームを作成し、最初のページに移動します
                this.rootFrame = new Frame();

                //フレームを SuspensionManager キーに関連付けます                                
                SuspensionManager.RegisterFrame(this.rootFrame, "AppFrame");

                // 既定の言語を設定します
                this.rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                this.rootFrame.NavigationFailed += this.OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // 必要な場合のみ、保存されたセッション状態を復元します
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        //状態の復元に何か問題があります。
                        //状態がないものとして続行します
                    }
                }

                // フレームを現在のウィンドウに配置します
                Window.Current.Content = this.rootFrame;
            }
            if (this.rootFrame.Content == null)
            {
                // ナビゲーションの履歴スタックが復元されていない場合、最初のページに移動します。
                // このとき、必要な情報をナビゲーション パラメーターとして渡して、新しいページを
                // 作成します
                this.rootFrame.Navigate(typeof(TopPage), e.Arguments);
            }

            // 現在のウィンドウがアクティブであることを確認します
            Window.Current.Activate();

            SettingsPane.GetForCurrentView().CommandsRequested += this.OnCommandsRequested;
        }

        /// <summary>
        /// MainViewModel のデータを読み込みます
        /// </summary>
        /// <returns>Task</returns>
        public async Task LoadAsync()
        {
            // セッションにデータがある場合は入れ替えます
            if (SuspensionManager.SessionState != null && SuspensionManager.SessionState.ContainsKey("MainViewModel"))
            {
                var sessionData = SuspensionManager.SessionState["MainViewModel"] as MainViewModel;

                ModuleContainer.Instance.SetInstance<MainViewModel>(sessionData);
            }
            else
            {
                //TODO:データ読み込み処理
            }

            this.ViewModel.IsLoaded = true;
        }

        /// <summary>
        /// チャーム表示要求時
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="args">イベント引数</param>
        private void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Discard()
        {
            SettingsPane.GetForCurrentView().CommandsRequested -= this.OnCommandsRequested;
            this.View.Suspending -= this.OnSuspending;
            this.View.Exit();
        }

        /// <summary>
        /// 特定のページへの移動が失敗したときに呼び出されます
        /// </summary>
        /// <param name="sender">移動に失敗したフレーム</param>
        /// <param name="e">ナビゲーション エラーの詳細</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// アプリケーションの実行が中断されたときに呼び出されます。アプリケーションの状態は、
        /// アプリケーションが終了されるのか、メモリの内容がそのままで再開されるのか
        /// わからない状態で保存されます。
        /// </summary>
        /// <param name="sender">中断要求の送信元。</param>
        /// <param name="e">中断要求の詳細。</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            SuspensionManager.SessionState["MainViewModel"] = this.ViewModel;
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}
