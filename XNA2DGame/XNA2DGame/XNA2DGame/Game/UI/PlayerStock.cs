using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNA2DGame
{
    /// <summary>
    /// プレイヤーの残機数
    /// </summary>
    class PlayerStock
    {
        // メンバー変数

        // 自己インスタンス
        private static readonly PlayerStock instance = new PlayerStock();



        /// <summary>
        /// コンストラクタ
        /// </summary>
        private PlayerStock() { }
        static PlayerStock() { }



        /// <summary>
        /// 自己インスタンスゲッター
        /// </summary>
        public static PlayerStock Instance { get { return instance; } }



        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="playerStock"></param>
        public void Draw( int playerStock ) 
        {
            Vector2 size        = new Vector2( 72, 96 );
            Vector2 rectStart   = new Vector2( size.X * 1, size.Y * 2 );
            Vector2 drawPos     = new Vector2( Screen.Width - size.X * playerStock, 0 );

            // プレイヤーの残機だけ描画
            for ( int i = 0; i < playerStock; i++ )
            {
                Rectangle rect = new Rectangle( (int)rectStart.X, (int)rectStart.Y, (int)size.X, (int)size.Y );

                Renderer.Instance.DrawTexture( "player", drawPos, rect );

                drawPos.X += size.X;
            }
        }
    }
}
