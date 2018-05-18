using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNA2DGame
{
    /// <summary>
    /// タイマークラス
    /// </summary>
    class GameTimer
    {
        // メンバー変数
        private int         gameTimeCounter;            // ゲームタイムカウンター
        private const int   gameTimeMax = 60 * 100;     // ゲームタイムの最大

        // 自己インスタンス
        private static readonly GameTimer instance = new GameTimer();



        /// <summary>
        /// コンストラクタ
        /// </summary>
        private GameTimer() { }
        static GameTimer() { }



        /// <summary>
        /// 自己インスタンスゲッター
        /// </summary>
        public static GameTimer Instance { get { return instance; } }



        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize() 
        {
            gameTimeCounter = gameTimeMax;
        }



        /// <summary>
        /// 更新
        /// </summary>
        public void Update() 
        {
            gameTimeCounter--;
        }



        /// <summary>
        /// 描画
        /// </summary>
        public void Draw() 
        {
            Renderer.Instance.DrawNumber( "number", Vector2.Zero, new Vector2(64, 128), new Vector2(0.5f, 0.5f), gameTimeCounter / 60, 3 );
        }



        /// <summary>
        /// ゲームの経過時間
        /// </summary>
        /// <returns></returns>
        public int GetElapsedGameTime() { return gameTimeMax - gameTimeCounter; }



        /// <summary>
        /// ゲッター群
        /// </summary>
        public int GetGameTime() { return gameTimeCounter; }    // ゲームタイム
    }
}
