using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNA2DGame
{
    /// <summary>
    /// フェードクラス
    /// </summary>
    class Fade
    {
        // メンバー変数
        private float   alpha;          // アルファ値
        private float   fadeTimer;      // フェードタイマー
        private bool    fadeEnd;        // フェード終了
        private bool    fadeFlg;        // フェードフラグ
        private bool    fadeInitFlg;    // フェード初期化フラグ



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Fade() 
        {
            alpha           = 0;            // アルファ値
            fadeTimer       = 0.5f * 60;    // フェードタイマー
            fadeEnd         = false;        // フェード終了
            fadeFlg         = false;        // フェードフラグ
            fadeInitFlg     = false;        // フェード初期化フラグ
        }



        /// <summary>
        /// 描画
        /// </summary>
        public void Draw() 
        {
            // フェード画像を透過表示
            Renderer.Instance.DrawTexture( "fade", Vector2.Zero, (int)alpha );
        }



        /// <summary>
        /// フェードアウト
        /// </summary>
        /// <returns></returns>
        public bool FadeOut() 
        {
            // 初期化してないなら初期化
            if ( !fadeInitFlg )
            {
                alpha           = 0;        // 透明
                fadeEnd         = false;    // フェード終了でない
                fadeInitFlg     = true;     // 初期化した
            }


            // アルファが規定値を越えたらフェード終了
            alpha += 255 / fadeTimer;
            if ( alpha >= 255 )
            {
                alpha           = 255;      // 不透明
                fadeEnd         = true;     // フェード終了
                fadeInitFlg     = false;    // 初期化してない

                fadeFlg         = ( !fadeFlg )? true : false;   // フェードフラグ更新
            }


            // フェード終了の情報を返す
            return fadeEnd;
        }



        /// <summary>
        /// フェードイン
        /// </summary>
        /// <returns></returns>
        public bool FadeIn() 
        {
            // 初期化してないなら初期化
            if ( !fadeInitFlg )
            {
                alpha           = 255;      // 不透明
                fadeEnd         = false;    // フェード終了でない
                fadeInitFlg     = true;     // 初期化した
            }


            // アルファが規定値を越えたら終了
            alpha -= 255 / fadeTimer;
            if ( alpha <= 0 )
            {
                alpha           = 0;        // 透明
                fadeEnd         = true;     // フェード終了
                fadeInitFlg     = false;    // 初期化してない

                fadeFlg         = ( !fadeFlg )? true : false;   // フェードフラグ更新
            }


            // フェード終了の情報を返す
            return fadeEnd;
        }



        /// <summary>
        /// フェードフラグゲッター
        /// </summary>
        /// <returns></returns>
        public bool GetFadeFlg() { return fadeFlg; }
    }
}
