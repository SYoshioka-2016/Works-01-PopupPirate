using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNA2DGame
{
    /// <summary>
    /// シーンマネージャークラス
    /// </summary>
    class SceneManager
    {
        // メンバー変数
        private Fade    fade;                   // フェード
        private IScene  currentScene = null;    // 現在のシーン

        // シーンコレクション
        private Dictionary< Scene, IScene > scenes = new Dictionary< Scene, IScene >();



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SceneManager() 
        {
            fade = new Fade();  // フェード


            // コンテンツのロード
            LoadContent();
        }



        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            // 現在のシーンが終了ならフェードアウト
            if ( currentScene.IsEnd() )
            {
                // フェードアウトが終了したらシーンチェンジ
                if ( fade.FadeOut() )
                {
                    // 現在のシーンのコンテンツコレクションのクリア
                    currentScene.ClearContentCollection();


                    // コンテンツのロード
                    LoadContent();


                    // シーンチェンジ
                    Change( currentScene.Next() );
                }
            }
            // 現在のシーンが終了でないなら
            else
            {
                // フェードフラグがオンならフェードイン
                if ( fade.GetFadeFlg() )
                {
                    // フェードイン
                    fade.FadeIn();
                }
                // オフなら現在のシーンの更新
                else
                {
                    // 現在のシーンの更新
                    currentScene.Update();
                }
            }
        }



        /// <summary>
        /// 描画
        /// </summary>
        public void Draw() 
        {
            // 現在のシーンの描画
            currentScene.Draw();


            // フェードの描画
            fade.Draw();
        }



        /// <summary>
        /// シーン追加
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="scene"></param>
        public void Add( Scene sceneName, IScene scene ) 
        {
            // シーンコレクションにシーンクラスを追加
            scenes[ sceneName ] = scene;
        }



        /// <summary>
        /// シーンチェンジ
        /// </summary>
        /// <param name="sceneName"></param>
        public void Change( Scene sceneName )
        {
            // 現在のシーンを変更
            currentScene = scenes[ sceneName ];

            // 現在のシーンの初期化
            currentScene.Initialize();

            // 現在のシーンのコンテンツのロード
            currentScene.LoadContent();
        }



        /// <summary>
        /// コンテンツのロード
        /// </summary>
        private void LoadContent() 
        {
            // テクスチャ
            // フェード
            Renderer.Instance.LoadTexture( "fade", "graphic\\" );   // フェード
        }
    }
}
