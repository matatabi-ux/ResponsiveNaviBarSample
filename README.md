### 幅がせまくなったらインジケータつきの FlipView 表示になるナビゲーションバー

Windows ストアアプリの UX ガイドラインでは FlipView に下記のようなインジケータをつけることが推奨されています

![レスポンシブナビバー](http://cdn-ak.f.st-hatena.com/images/fotolife/m/matatabi_ux/20140209/20140209102259.png)

Windows ストアでもナビバー部分がこのような表示になっており、画面幅が狭いときなどに便利なのでぜひ使いたい

しかし、FlipView にはインジケータを表示する機能はついてなかったり・・・ないならば作るしかない！

インジケータ部分が ListBox、左右のページ遷移は FlipView、内部の項目表示は GridView を使いました

ついでに Header や インジケータ表示位置の変更機能をつけてみたり・・・いやー思ったよりも大変でした；

