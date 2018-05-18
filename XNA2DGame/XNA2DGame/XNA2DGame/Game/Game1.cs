using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XNA2DGame
{
    /// <summary>
    /// 基底 Game クラスから派生した、ゲームのメイン クラスです。
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // メンバー変数
        GraphicsDeviceManager   graphics;       // グラフィックデバイスマネージャー
        SceneManager            sceneManager;   // シーンマネージャー
        


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            // デバイス設定
            DeviceManager.Instance.SetDevice( Content, graphics );

            // 画面サイズ設定
            graphics.PreferredBackBufferWidth   = Screen.Width;
            graphics.PreferredBackBufferHeight  = Screen.Height;
        }



        /// <summary>
        /// ゲームが実行を開始する前に必要な初期化を行います。
        /// ここで、必要なサービスを照会して、関連するグラフィック以外のコンテンツを
        /// 読み込むことができます。base.Initialize を呼び出すと、使用するすべての
        /// コンポーネントが列挙されるとともに、初期化されます。
        /// </summary>
        protected override void Initialize()
        {
            // TODO: ここに初期化ロジックを追加します。

            // クラスの生成
            sceneManager = new SceneManager();  //シーンマネージャー


            // シーンの追加
            sceneManager.Add( Scene.Title,      new Title() );      // タイトル
            //sceneManager.Add( Scene.GamePlay,   new GamePlay() );   // ゲームプレイ
            sceneManager.Add( Scene.GamePlay,   new GamePlay(this) );   // ゲームプレイ
            sceneManager.Add( Scene.Ending,     new Ending() );     // エンディング


            // 最初のシーン設定
            sceneManager.Change( Scene.Title );  // タイトル
            //sceneManager.Change( Scene.GamePlay );

            // タイトル設定
            base.Window.Title = "XNA2DGame";

            base.Initialize();
        }



        /// <summary>
        /// LoadContent はゲームごとに 1 回呼び出され、ここですべてのコンテンツを
        /// 読み込みます。
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: this.Content クラスを使用して、ゲームのコンテンツを読み込みます。
        }



        /// <summary>
        /// UnloadContent はゲームごとに 1 回呼び出され、ここですべてのコンテンツを
        /// アンロードします。
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: ここで ContentManager 以外のすべてのコンテンツをアンロードします。

            Renderer.Instance.Unload();         // レンダラー
            Sound.Instance.Unload();            // サウンド
            DeviceManager.Instance.Unload();    // デバイスマネージャー
        }



        /// <summary>
        /// ワールドの更新、衝突判定、入力値の取得、オーディオの再生などの
        /// ゲーム ロジックを、実行します。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        protected override void Update(GameTime gameTime)
        {
            // ゲームの終了条件をチェックします。
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown( Keys.Escape ))
                this.Exit();

            // TODO: ここにゲームのアップデート ロジックを追加します。

            sceneManager.Update();  // シーンマネージャー

            base.Update(gameTime);
        }



        /// <summary>
        /// ゲームが自身を描画するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: ここに描画コードを追加します。

            // 描画の開始
            Renderer.Instance.Begin();

            // ムービーの開始
            Movie.Instance.Begin();

            // 描画
            sceneManager.Draw();  // シーンマネージャー

            // 描画の終了
            Renderer.Instance.End();

            // ムービーの終了
            Movie.Instance.End();


            base.Draw(gameTime);
        }
    }
}
