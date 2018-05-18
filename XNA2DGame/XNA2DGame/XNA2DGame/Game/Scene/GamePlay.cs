using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNA2DGame
{
    /// <summary>
    /// ゲームプレイ
    /// </summary>
    class GamePlay : IScene
    {
        // メンバー変数
        private bool                isEnd;              // 終了情報
        private Stage               stage;              // ステージ
        private CharacterManager    characterManager;   // キャラクターマネージャー
        private Collision           collision;          // 衝突処理
        private Pause               pause;              // ポーズ

        private const string soundFileName = "bgm_gameplay";   // サウンドファイル名



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GamePlay( Game1 game1 ) 
        {
            stage               = new Stage();                          // ステージ
            characterManager    = new CharacterManager( stage );        // キャラクターマネージャー
            collision           = new Collision( characterManager );    // 衝突処理
            pause               = new Pause( game1 );                   // ポーズ
        }



        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize() 
        {
            isEnd           = false;    // 終了判定を「継続」に設定


            characterManager.Initialize();      // キャラクターマネージャー
            pause.Initialize();                 // ポーズ
            GameTimer.Instance.Initialize();    // ゲームタイマー
        }



        /// <summary>
        /// 更新
        /// </summary>
        public void Update() 
        {
            // BGM再生
            Sound.Instance.Play( soundFileName );



            pause.Update();             // ポーズ

            // シーンチェンジするなら終了
            if ( pause.IsChangeScene() ) isEnd = true;

            // ポーズ中なら処理しない
            if ( pause.IsPause() ) return;



            // プレイヤーの残機が0になったら終了
            if ( characterManager.GetPlayer().IsDead() ) isEnd = true;

            // ゲームタイムが0になったら終了
            if ( GameTimer.Instance.GetGameTime() <= 0 ) isEnd = true;



            characterManager.Update();      // キャラクターマネージャー
            collision.Update();             // 衝突処理
            GameTimer.Instance.Update();    // ゲームタイマー
        }



        /// <summary>
        /// 描画
        /// </summary>
        public void Draw() 
        {
            // 描画順を交換して描画
            SwappingDraw();

            GameTimer.Instance.Draw();

            // プレイヤーの残機を描画
            Character   tmp     = characterManager.GetPlayer();
            Player      player  = ( tmp is Player )? tmp as Player : null;
            if ( player != null ) PlayerStock.Instance.Draw( player.GetStock() );

            pause.Draw();   // ポーズ
        }



        /// <summary>
        /// 終了判定
        /// </summary>
        /// <returns></returns>
        public bool IsEnd() { return isEnd; }



        /// <summary>
        /// 次のシーン(エンディング)
        /// </summary>
        /// <returns></returns>
        public Scene Next() 
        { 
            Scene next;
            if ( pause.IsChangeScene() )    // ポーズでシーンが変わる場合
            {
                next = pause.NextScene();
            }
            else
            {
                next = Scene.Ending;
            }

            return next;
        }



        /// <summary>
        /// コンテンツのロード
        /// </summary>
        public void LoadContent() 
        {
            // テクスチャのロード
            // ステージ
            Renderer.Instance.LoadTexture( "stage",         "graphic\\stage\\" );  // ステージ
            Renderer.Instance.LoadTexture( "stage_back",    "graphic\\stage\\" );  // ステージ背景

            // キャラクター
            Renderer.Instance.LoadTexture( "player",    "graphic\\character\\" );  // プレイヤー
            Renderer.Instance.LoadTexture( "ball",      "graphic\\character\\" );  // 鉄球
//            Renderer.Instance.LoadTexture( "red",       "graphic\\character\\" );  // 大砲
            Renderer.Instance.LoadTexture( "shadow",    "graphic\\character\\" );  // 鉄球


            // UI
            Renderer.Instance.LoadTexture( "number",            "graphic\\text\\" );   // 数字 
            Renderer.Instance.LoadTexture( "pause_back_black",  "graphic\\pause\\" );  // 
            Renderer.Instance.LoadTexture( "pause_back",        "graphic\\pause\\" );  // ポーズ背景
            Renderer.Instance.LoadTexture( "pause_menu_back",   "graphic\\pause\\" );  // Backポーズメニュー
            Renderer.Instance.LoadTexture( "pause_menu_title",  "graphic\\pause\\" );  // タイトルポーズメニュー
            Renderer.Instance.LoadTexture( "pause_menu_exit",   "graphic\\pause\\" );  // EXITポーズメニュー



            // サウンドのロード
            // BGM
            Sound.Instance.LoadSound( soundFileName, "sound\\bgm\\", true );  // ゲームプレイBGM

            // SE
//            Sound.Instance.LoadSound( "se_splash",  "sound\\se\\", false );  // 水落SE
            Sound.Instance.LoadSound( "sound\\se\\se_splash",   false );  // 水落SE
//            Sound.Instance.LoadSound( "se_shot",    "sound\\se\\", false );  // 発射SE
            Sound.Instance.LoadSound( "sound\\se\\se_shot",     false );  // 発射SE
            Sound.Instance.LoadSound( "sound\\se\\se_hit",      false );  // 衝突SE
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



        /// <summary>
        /// 描画順を交換して描画
        /// </summary>
        private void SwappingDraw() 
        {
            // 描画用キャラクターコレクション
            List<Character>[] characterListArray = {
                new List<Character>(),      // [0] ステージより上に有るキャラクターコレクション
                new List<Character>(),      // [1] ステージより下に有るキャラクターコレクション
            };



            // 描画するキャラクターコレクションを取得
            List<Character> characters = characterManager.GetCharacters();

            // 描画用キャラクターコレクションにキャラクターを格納
            foreach ( var c in characters ) 
            {
                // 「ステージより上に有るか」のフラグで格納先を分ける
                switch ( c.GetAboveTheStageFlg() ) 
                {
                    // ステージより上に有る
                    case true:  characterListArray[0].Add( c ); break;

                    // ステージより下に有る
                    case false: characterListArray[1].Add( c ); break;
                }
            }



            // ステージ背景画像
            Renderer.Instance.DrawTexture( "stage_back",    Vector2.Zero );

            // ステージより下に有るキャラクターコレクションを描画
            foreach ( var c in characterListArray[1] ) c.Draw();

            // ステージを描画
            stage.Draw();

            // ステージより上に有るキャラクターコレクションを描画
            foreach ( var c in characterListArray[0] ) c.Draw();
        }
    }
}
