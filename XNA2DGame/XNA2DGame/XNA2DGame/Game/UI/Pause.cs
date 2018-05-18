using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNA2DGame
{
    /// <summary>
    /// ポーズクラス
    /// </summary>
    class Pause
    {



        // ポーズメニューはクラス化したほうが良い?











        // メンバー変数
        private int             menuNumber;                     // メニュー番号
        private const int       menuNumberMax = 2;              // メニュー番号の最大
        private const int       menuCount = menuNumberMax + 1;
        private float[]         menuBarsTexScale;               // メニュー画像の拡大倍率

        private bool    isPause;            // ポーズ情報
        private bool    isChangeScene;      // シーンチェンジ情報
        private bool    pushFlg;            // 入力フラグ

        private Game1   game1;  // ゲームのメインクラス(Exit用)

        // メニュー名テーブル
        private readonly string[] menuBarsTexName = 
        {
            "pause_menu_back",
            "pause_menu_title",
            "pause_menu_exit",
        };


        private const float menuBarTexSizeX = 320;
        private const float menuBarTexSizeY = 120;
        private const float menuBarsStartDispPosX = (Screen.Width - menuBarTexSizeX) / 2;
        private const float menuBarsStartDispPosY = 100;
        private const float collectionValue = 100;


        // メニュー座標テーブル
        private readonly Vector2[] menuBarsPosTable = 
        {
            new Vector2( menuBarsStartDispPosX, menuBarsStartDispPosY ),
            new Vector2( menuBarsStartDispPosX, menuBarsStartDispPosY + menuBarTexSizeY + collectionValue ),
            new Vector2( menuBarsStartDispPosX, menuBarsStartDispPosY + menuBarTexSizeY + menuBarTexSizeY + collectionValue + collectionValue ),
        };

        




        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Pause( Game1 game1 ) 
        {
            this.game1 = game1;



            menuBarsTexScale = new float[ menuCount ];
        }



        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize() 
        {
            menuNumber      = 0;        // メニュー番号
            isPause         = false;    // ポーズ情報
            isChangeScene   = false;    // シーンチェンジ情報
            pushFlg         = false;    // 入力フラグ



            for ( int i = 0; i < menuCount; i++ )
            {
                menuBarsTexScale[i] = 1.0f;
            }
            menuBarsTexScale[0] = 1.2f;
        }



        /// <summary>
        /// 更新
        /// </summary>
        public void Update() 
        {
            SwitchingPause();       // ポーズの切り替え
            LimitMenuNumber();      // メニュー番号の制限
            SelectMenu();           // メニューの選択
        }



        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw() 
        {
            // ポーズ中以外は処理しない
            if ( !(isPause) ) return;



            Vector2 pauseBackTexSize = new Vector2( 900, 700 );
            Vector2 pauseBackTexPos  = new Vector2( Screen.Width - pauseBackTexSize.X, Screen.Height - pauseBackTexSize.Y ) / 2;


            // ポーズ背景を表示
            Renderer.Instance.DrawTexture( "pause_back_black", Vector2.Zero );
            Renderer.Instance.DrawTexture( "pause_back", pauseBackTexPos );


            // 選択中のメニューを表示
            for ( int i = 0; i < menuCount; i++ ) 
            {
                Renderer.Instance.DrawTexture(
                    menuBarsTexName[i],
                    menuBarsPosTable[i],
                    menuBarsTexScale[i]
                );
            }
        }



        /// <summary>
        /// ポーズの切り替え
        /// </summary>
        private void SwitchingPause() 
        {
            // ポーズ中へ切り替え
            if ( !pushFlg &&
                 InputKeyboard.Instance.State( Keys.Enter ) == InputKeyboard.KS.DOWN )
            {
                if ( !(isPause) )   isPause = true;     // ポーズ情報
                pushFlg = true;                         // 入力フラグ
            }
        }



        /// <summary>
        /// メニュー番号の制限
        /// </summary>
        private void LimitMenuNumber()
        {
            // ポーズ中以外は処理しない
            if ( !(isPause) ) return;


            // メニュー番号の制御
            if ( menuNumber >= menuNumberMax )
            {
                menuNumber = menuNumberMax;
            }
            if ( menuNumber <= 0 )
            {
                menuNumber = 0;
            }
        }



        /// <summary>
        /// メニューの選択
        /// </summary>
        private void SelectMenu() 
        {
            // ポーズ中以外は処理しない
            if ( !(isPause) ) return;



            int mn = menuNumber;
            int idx = 0;

            // メニュー番号の更新
            if ( InputKeyboard.Instance.State( Keys.Down )   == InputKeyboard.KS.DOWN )
            {
                ++menuNumber;

                idx = ( menuNumber >= menuNumberMax )? menuNumberMax : menuNumber;
                mn  = idx - 1;

                menuBarsTexScale[ idx ] = 1.2f;
                menuBarsTexScale[ mn ]  = 1.0f;
            }
            if ( InputKeyboard.Instance.State( Keys.Up )     == InputKeyboard.KS.DOWN )
            {
                --menuNumber;

                idx = ( menuNumber <= 0 )? 0 : menuNumber;
                mn  = idx + 1;

                menuBarsTexScale[ idx ] = 1.2f;
                menuBarsTexScale[ mn ]  = 1.0f;
            }


            // 選択中のメニューを実行
            if ( pushFlg &&
                 InputKeyboard.Instance.State( Keys.Enter ) == InputKeyboard.KS.DOWN )
            {
                switch ( menuNumber )
                {
                    case 0:     
                        isPause = false;
                        isChangeScene = false;
                        break;

                    case 1:
                        isChangeScene = true;
                        break;

                    case 2:
                        game1.Exit();
                        break;
                        

                    default:
                        break;
                }

                pushFlg = false;
            }
        }



        /// <summary>
        /// メニューのシーンゲッター
        /// </summary>
        /// <returns></returns>
        public Scene NextScene() 
        {
            switch ( menuNumber )
            {
                case 1: return Scene.Title;
                case 2: return Scene.Unknown;

                default: return Scene.Unknown;

            }
        }



        /// <summary>
        /// ポーズ情報ゲッター
        /// </summary>
        /// <returns></returns>
        public bool IsPause() { return isPause; }



        /// <summary>
        /// シーンチェンジ情報ゲッター
        /// </summary>
        /// <returns></returns>
        public bool IsChangeScene() { return isChangeScene; }
    }
}