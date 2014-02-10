#region License
//-----------------------------------------------------------------------
// <copyright>
//     Copyright matatabi-ux 2014.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace ResponsiveNaviBarSample.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Threading.Tasks;
    using ResponsiveNaviBarSample.Views;
    using Windows.ApplicationModel;
    using Windows.Foundation.Collections;
    using Windows.UI.Popups;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Documents;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Markup;
    using Windows.UI.Xaml.Media;

    /// <summary>
    /// インジケータとヘッダ表示可能なレスポンシブデザイン FlipView
    /// </summary>
    [TemplatePart(Name = "HeaderContentPresenter", Type = typeof(ContentPresenter))]
    [TemplatePart(Name = "Indicator", Type = typeof(ListBox))]
    [TemplatePart(Name = "ItemContainer", Type = typeof(FlipView))]
    public class ResponsiveDesignedFlipView : FlipView
    {
        #region Privates

        /// <summary>
        /// ヘッダ
        /// </summary>
        private ContentPresenter header;

        /// <summary>
        /// インジケータ
        /// </summary>
        private ListBox indicator;

        /// <summary>
        /// ページ表示用 FlipView
        /// </summary>
        private FlipView pageView;

        /// <summary>
        /// アイテム表示用 GridView
        /// </summary>
        private List<ListViewBase> itemContainers = new List<ListViewBase>();

        /// <summary>
        /// フォーカスされていた GridView
        /// </summary>
        private ListViewBase preFocusContainer;

        /// <summary>
        /// コレクションビュー
        /// </summary>
        private CollectionViewSource viewSource = new CollectionViewSource();

        #endregion //Privates

        #region Header 依存関係プロパティ
        /// <summary>
        /// ヘッダ 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty HeaderProperty
            = DependencyProperty.Register(
            "Header",
            typeof(string),
            typeof(ResponsiveDesignedFlipView),
            new PropertyMetadata(
                null,
                (s, e) =>
                {
                    var control = s as ResponsiveDesignedFlipView;
                    if (control != null)
                    {
                        control.OnHeaderChanged();
                    }
                }));

        /// <summary>
        /// ヘッダ 変更イベントハンドラ
        /// </summary>
        private void OnHeaderChanged()
        {
            if (this.header == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(this.Header))
            {
                this.header.Visibility = Visibility.Visible;
                this.header.Content = this.Header;
            }
            else
            {
                this.header.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// ヘッダ
        /// </summary>
        public string Header
        {
            get { return (string)this.GetValue(HeaderProperty); }
            set { this.SetValue(HeaderProperty, value); }
        }
        #endregion //Header 依存関係プロパティ

        #region HeaderTemplate 依存関係プロパティ
        /// <summary>
        /// ヘッダテンプレート 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty
            = DependencyProperty.Register(
            "HeaderTemplate",
            typeof(DataTemplate),
            typeof(ResponsiveDesignedFlipView),
            new PropertyMetadata(
                null,
                (s, e) =>
                {
                    var control = s as ResponsiveDesignedFlipView;
                    if (control != null)
                    {
                        control.OnHeaderTemplateChanged();
                    }
                }));

        /// <summary>
        /// ヘッダテンプレート 変更イベントハンドラ
        /// </summary>
        private void OnHeaderTemplateChanged()
        {
            if (this.header == null)
            {
                return;
            }
            this.header.ContentTemplate = this.HeaderTemplate;
        }

        /// <summary>
        /// ヘッダテンプレート
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)this.GetValue(HeaderTemplateProperty); }
            set { this.SetValue(HeaderTemplateProperty, value); }
        }
        #endregion //HeaderTemplate 依存関係プロパティ

        #region IndicatorPlacement 依存関係プロパティ
        /// <summary>
        /// インジケータ位置 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty IndicatorPlacementProperty
            = DependencyProperty.Register(
            "IndicatorPlacement",
            typeof(IndicatorPlacement),
            typeof(ResponsiveDesignedFlipView),
            new PropertyMetadata(
                IndicatorPlacement.Default,
                (s, e) =>
                {
                    var control = s as ResponsiveDesignedFlipView;
                    if (control != null)
                    {
                        control.OnIndicatorPlacementChanged();
                    }
                }));

        /// <summary>
        /// インジケータ位置 変更イベントハンドラ
        /// </summary>
        private void OnIndicatorPlacementChanged()
        {
            if (this.indicator == null)
            {
                return;
            }

            switch (this.IndicatorPlacement)
            {
                case Controls.IndicatorPlacement.Above:
                case Controls.IndicatorPlacement.Default:
                    Grid.SetRow(this.indicator, 1);
                    break;

                case Controls.IndicatorPlacement.Below:
                    Grid.SetRow(this.indicator, 3);
                    break;
            }
        }

        /// <summary>
        /// インジケータ位置
        /// </summary>
        public IndicatorPlacement IndicatorPlacement
        {
            get { return (IndicatorPlacement)this.GetValue(IndicatorPlacementProperty); }
            set { this.SetValue(IndicatorPlacementProperty, value); }
        }
        #endregion //IndicatorPlacement 依存関係プロパティ

        #region ItemWidth 依存関係プロパティ
        /// <summary>
        /// 列幅 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty ItemWidthProperty
            = DependencyProperty.Register(
            "ItemWidth",
            typeof(double),
            typeof(ResponsiveDesignedFlipView),
            new PropertyMetadata(
                250d,
                (s, e) =>
                {
                    var control = s as ResponsiveDesignedFlipView;
                    if (control != null)
                    {
                        control.OnItemWidthChanged();
                    }
                }));

        /// <summary>
        /// 列幅 変更イベントハンドラ
        /// </summary>
        private void OnItemWidthChanged()
        {
            this.OnItemsChanged(this);
        }

        /// <summary>
        /// 列幅
        /// </summary>
        public double ItemWidth
        {
            get { return (double)this.GetValue(ItemWidthProperty); }
            set { this.SetValue(ItemWidthProperty, value); }
        }
        #endregion //ItemWidth 依存関係プロパティ

        #region ItemHeight 依存関係プロパティ
        /// <summary>
        /// 行幅 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty
            = DependencyProperty.Register(
            "ItemHeight",
            typeof(double),
            typeof(ResponsiveDesignedFlipView),
            new PropertyMetadata(
                40d,
                (s, e) =>
                {
                    var control = s as ResponsiveDesignedFlipView;
                    if (control != null)
                    {
                        control.OnItemHeightChanged();
                    }
                }));

        /// <summary>
        /// 行幅 変更イベントハンドラ
        /// </summary>
        private void OnItemHeightChanged()
        {
            this.OnItemsChanged(this);
        }

        /// <summary>
        /// 行幅
        /// </summary>
        public double ItemHeight
        {
            get { return (double)this.GetValue(ItemHeightProperty); }
            set { this.SetValue(ItemHeightProperty, value); }
        }
        #endregion //ItemHeight 依存関係プロパティ

        #region MaximumRows 依存関係プロパティ
        /// <summary>
        /// 最大行数 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty MaximumRowsProperty
            = DependencyProperty.Register(
            "MaximumRows",
            typeof(int),
            typeof(ResponsiveDesignedFlipView),
            new PropertyMetadata(
                1,
                (s, e) =>
                {
                    var control = s as ResponsiveDesignedFlipView;
                    if (control != null)
                    {
                        control.OnMaximumRowsChanged();
                    }
                }));

        /// <summary>
        /// 最大行数 変更イベントハンドラ
        /// </summary>
        private void OnMaximumRowsChanged()
        {
            this.OnItemsChanged(this);
        }

        /// <summary>
        /// 最大行数
        /// </summary>
        public int MaximumRows
        {
            get { return (int)this.GetValue(MaximumRowsProperty); }
            set { this.SetValue(MaximumRowsProperty, value); }
        }
        #endregion //MaximumRows 依存関係プロパティ

        /// <summary>
        /// アイテムクリックイベント
        /// </summary>
        public event ItemClickEventHandler ItemClick;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ResponsiveDesignedFlipView()
        {
            this.DefaultStyleKey = typeof(ResponsiveDesignedFlipView);
            this.Loaded += this.OnLoaded;
        }

        /// <summary>
        /// 読み込み完了イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded += this.OnUnloaded;

            this.header = this.GetTemplateChild("HeaderContentPresenter") as ContentPresenter;
            this.OnHeaderTemplateChanged();
            this.OnHeaderChanged();

            this.pageView = this.GetTemplateChild("ItemContainer") as FlipView;
            this.indicator = this.GetTemplateChild("Indicator") as ListBox;
            this.SizeChanged += this.OnSizeChanged;
            this.OnIndicatorPlacementChanged();
            this.OnItemsChanged(this);

            this.pageView.SetBinding(
                FlipView.ItemsSourceProperty,
                new Binding()
                {
                    Source = this.viewSource,
                    Mode = BindingMode.TwoWay,
                });
            this.indicator.SetBinding(
                ListBox.ItemsSourceProperty,
                new Binding()
                {
                    Source = this.viewSource,
                    Mode = BindingMode.TwoWay,
                });

            this.SelectionChanged += this.OnSelectionChanged;
            this.OnSelectionChanged(this, null);
        }

        /// <summary>
        /// フォーカスイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (this.itemContainers.Count < 1)
            {
                return;
            }

            if (this.itemContainers.Contains(sender))
            {
                var container = sender as ListViewBase;
                var item = container.ContainerFromItem(this.SelectedItem) as Control;
                if (item == null
                    && this.preFocusContainer != null
                    && this.itemContainers.Contains(this.preFocusContainer)
                    && this.itemContainers.IndexOf(this.preFocusContainer) > this.itemContainers.IndexOf(container))
                {
                    item = container.ContainerFromIndex(container.Items.Count - 1) as Control;
                    if (item != null)
                    {
                        item.Focus(FocusState.Keyboard);
                    }
                }

                this.preFocusContainer = container;
            }
        }

        /// <summary>
        /// フォーカスイベントハンドラ
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            this.OnGotFocus(this, e);
        }

        /// <summary>
        /// サイズ変更イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.OnItemsChanged(this);
        }

        /// <summary>
        /// メモリ解放イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= this.OnUnloaded;
            this.SelectionChanged -= this.OnSelectionChanged;
            this.SizeChanged -= this.OnSizeChanged;
            foreach (var container in this.itemContainers)
            {
                if (container == null)
                {
                    continue;
                }
                container.ItemClick -= this.OnItemClick;
                container.GotFocus -= this.OnGotFocus;
            }
        }

        /// <summary>
        /// 子要素破棄時の処理
        /// </summary>
        protected override void OnDisconnectVisualChildren()
        {
            this.Loaded -= this.OnLoaded;
            base.OnDisconnectVisualChildren();
        }

        /// <summary>
        /// アイテム変更後処理
        /// </summary>
        /// <param name="e">変更者</param>
        protected async override void OnItemsChanged(object e)
        {
            if (this.pageView == null || this.indicator == null || this.Items == null)
            {
                return;
            }

            var maxColumns = Math.Floor((this.ActualWidth - 78) / this.ItemWidth);
            var indicatorWidth = Math.Floor((this.ActualWidth - 78) / Math.Ceiling(this.Items.Count / maxColumns)) - 7;
            var pages = new ObservableCollection<PageDescriptor>();
            foreach (var item in this.Items)
            {
                if (pages.LastOrDefault() == null || pages.Last().Items.Count >= maxColumns * this.MaximumRows)
                {
                    pages.Add(new PageDescriptor()
                    {
                        ItemContainerStyle = this.ItemContainerStyle,
                        ItemContainerStyleSelector = this.ItemContainerStyleSelector,
                        ItemTemplate = this.ItemTemplate,
                        ItemTemplateSelector = this.ItemTemplateSelector,
                        ItemWidth = this.ItemWidth,
                        ItemHeight = this.ItemHeight,
                        IndicatorWidth = indicatorWidth,
                        MaximumRowsOrColumns = this.MaximumRows,
                    });
                }
                pages.Last().Items.Add(item);
            }
            this.viewSource.Source = pages;

            if (pages.Count > 1)
            {
                this.indicator.Visibility = Visibility.Visible;
            }
            else
            {
                this.indicator.Visibility = Visibility.Collapsed;
            }

            await Task.Delay(1);

            this.itemContainers = new List<ListViewBase>();

            for (int i = 0; i < this.pageView.Items.Count; i++)
            {
                var page = this.pageView.ContainerFromIndex(i) as FlipViewItem;
                if (page == null)
                {
                    await this.Dispatcher.RunAsync(
                        Windows.UI.Core.CoreDispatcherPriority.Normal,
                        () =>
                        {
                            page = this.pageView.ContainerFromIndex(i) as FlipViewItem;
                        });
                }
                if (page == null)
                {
                    continue;
                }

                var container = VisualTreeUtility.FindChild<ListViewBase>(page.ContentTemplateRoot);
                if (container == null)
                {
                    continue;
                }
                this.itemContainers.Add(container);
                container.ItemClick -= this.OnItemClick;
                container.GotFocus -= this.OnGotFocus;
                container.ItemClick += this.OnItemClick;
                container.GotFocus += this.OnGotFocus;
            }
        }

        /// <summary>
        /// 選択変更イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.pageView == null || this.indicator == null || this.viewSource == null || this.Items == null || DesignMode.DesignModeEnabled)
            {
                return;
            }

            var pages = this.viewSource.Source as ObservableCollection<PageDescriptor>;
            if (pages == null)
            {
                return;
            }

            if (this.ItemsSource is ICollectionView)
            {
                this.SelectedIndex = ((ICollectionView)this.ItemsSource).CurrentPosition;
            }

            var currentPage = pages.FirstOrDefault(p => p.Items.Contains(this.SelectedItem));
            if (currentPage != null)
            {
                this.pageView.SelectedItem = currentPage;
            }
        }

        /// <summary>
        /// アイテムクリックイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.ItemClick == null)
            {
                return;
            }
            this.ItemClick.Invoke(sender, e);
        }

        /// <summary>
        /// ページ情報
        /// </summary>
        internal class PageDescriptor
        {
            /// <summary>
            /// アイテムの横幅
            /// </summary>
            public double ItemWidth { get; set; }

            /// <summary>
            /// アイテムの縦幅
            /// </summary>
            public double ItemHeight { get; set; }

            /// <summary>
            /// インジケータの横幅
            /// </summary>
            public double IndicatorWidth { get; set; }

            /// <summary>
            /// 最大行数
            /// </summary>
            public int MaximumRowsOrColumns { get; set; }

            /// <summary>
            /// アイテム
            /// </summary>
            public ObservableCollection<object> Items { get; set; }

            /// <summary>
            /// コンテナスタイル
            /// </summary>
            public Style ItemContainerStyle { get; set; }

            /// <summary>
            /// コンテナスタイルセレクタ
            /// </summary>
            public StyleSelector ItemContainerStyleSelector { get; set; }

            /// <summary>
            /// テンプレート
            /// </summary>
            public DataTemplate ItemTemplate { get; set; }

            /// <summary>
            /// テンプレートセレクタ
            /// </summary>
            public DataTemplateSelector ItemTemplateSelector { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public PageDescriptor()
            {
                this.Items = new ObservableCollection<object>();
            }
        }
    }

    /// <summary>
    /// インジケータ位置列挙型
    /// </summary>
    public enum IndicatorPlacement
    {
        /// <summary>
        /// FlipView の上にインジケータを表示します
        /// </summary>
        Default = 0,

        /// <summary>
        /// FlipView の上にインジケータを表示します
        /// </summary>
        Above = 1,

        /// <summary>
        /// FlipView の下にインジケータを表示します
        /// </summary>
        Below = 2,
    }
}
