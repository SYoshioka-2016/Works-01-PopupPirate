using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNA2DGame
{
    /// <summary>
    /// タイトルクラス
    /// </summary>
    class Title : IScene
    {
        // メンバー変数
        private bool isEnd;             // 終了情報

        private const string soundFileName = "bgm_title";   // サウンドファイル名



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Title() 
        {
        }



        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize() 
        {
            isEnd           = false;    // 終了判定を「継続」に設定
        }



        /// <summary>
        /// 更新
        /// </summary>
        public void Update() 
        {
            // BGM再生
            Sound.Instance.Play( soundFileName );



            // スペースキーが押されたら終了
            if ( 
                InputKeyboard.Instance.State( Keys.Space ) == InputKeyboard.KS.DOWN ||
                InputGamePad.Instance.State( Buttons.Start, PlayerIndex.One ) == InputGamePad.BS.DOWN
            )
            {
                isEnd = true;  // 終了判定を「終了」に設定
            }
        }



        /// <summary>
        /// 描画
        /// </summary>
        public void Draw() 
        {
            Renderer.Instance.DrawTexture( "title_back", Vector2.Zero );                                // タイトル
            Renderer.Instance.DrawTexture( "title_text_pop-up_pirate", 200, new Vector2(960, 128) );    // タイトルテキスト
            Renderer.Instance.DrawTexture( "text_push_space", 550, new Vector2(480, 80), 0.5 );         // テキスト
        }



        /// <summary>
        /// 終了判定
        /// </summary>
        /// <returns></returns>
        public bool IsEnd() { return isEnd; }



        /// <summary>
        /// 次のシーン(ゲームプレイ)
        /// </summary>
        /// <returns></returns>
        public Scene Next() { return Scene.GamePlay; }



        /// <summary>
        /// コンテンツのロード
        /// </summary>
        public void LoadContent() 
        {
            // テクスチャのロード
            Renderer.Instance.LoadTexture( "title_back",                "graphic\\title\\" );  // タイトル
            Renderer.Instance.LoadTexture( "title_text_pop-up_pirate",  "graphic\\title\\" );  // タイトルテキスト
            Renderer.Instance.LoadTexture( "text_push_space",           "graphic\\text\\" );   // テキスト

            // サウンドのロード
            Sound.Instance.LoadSound( soundFileName, "sound\\bgm\\", true );  // タイトルBGM
        }



        /// <summary>
        /// コンテンツコレクションのクリア
        /// </summary>
        public void ClearContentCollection() 
        {
            // BGM停止
            Sound.Instance.Stop( soundFileName );



            Renderer.Instance.ClearContentCollection();      // レンダラー
            Sound.Instance.ClearContentCollection();         // サウンド
        }
    }
}
