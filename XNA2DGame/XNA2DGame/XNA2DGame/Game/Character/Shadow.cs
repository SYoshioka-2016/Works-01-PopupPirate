using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;



namespace XNA2DGame
{
    /// <summary>
    /// 影クラス
    /// </summary>
    class Shadow
    {
        // メンバ変数
        private const string        filename    = "shadow";                 // 画像ファイル名
        private readonly Vector2    size        = new Vector2( 64, 64 );    // 画像サイズ



        public Shadow() { }



        /// <summary>
        /// 影の描画
        /// </summary>
        /// <param name="objPos"></param>
        /// <param name="objScale"></param>
        /// <param name="objSize"></param>
        public void DrawShadow( Vector3 objPos, Vector2 objScale, Vector2 objSize ) 
        {
            // 基準の高さの最大
            const float heightMax = 500;

            // 床からオブジェクトまでの高さ
            float height = Math.Abs( objPos.Y );
            height = ( height > heightMax )? heightMax : height;
 
            // 影の拡大倍率の最大、最小
            const float max = 1.0f, min = 0.3f;

            // 影の拡大倍率
            float shadowScale = (max - min) * ( height / heightMax ) + min;
            shadowScale = 1 + min - shadowScale;

            // 描画する座標
            Vector2 drawPos = new Vector2( 
                                  objPos.X - ((this.size.X * objScale.X * shadowScale) / 2), 
                                  objPos.Z + (objSize.Y / 2) - (this.size.Y * objScale.Y) / 2 
                              );


            // 影の描画
            Renderer.Instance.DrawTexture( filename, drawPos, objScale * shadowScale );
        }
    }
}
