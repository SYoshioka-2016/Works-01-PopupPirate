using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XNA2DGame
{
    /// <summary>
    /// 描画クラス
    /// シングルトン
    /// </summary>
    class Renderer
    {
        // メンバー変数
        private uint flashingCountTimer = 0;    // 点滅表示用カウンター

        private ContentManager  contentManager = DeviceManager.Instance.GetCM;      // コンテンツマネージャー
        private SpriteBatch     spriteBatch                                         // スプライトバッチ
            = new SpriteBatch( DeviceManager.Instance.GetGDM.GraphicsDevice );

        // テクスチャコレクション
        private Dictionary< string, Texture2D > textures    = new Dictionary< string, Texture2D >();

        // 自己インスタンス
        private static readonly Renderer instance           = new Renderer();



        /// <summary>
        /// コンストラクタ
        /// </summary>
        static Renderer() { }
        private Renderer() { }

        

        /// <summary>
        /// インスタンスゲッター
        /// </summary>
        public static Renderer Instance 
        {
            get { return instance; }
        }



        /// <summary>
        /// テクスチャのロード
        /// </summary>
        /// <param name="name"></param>
        public void LoadTexture( string name )
        { 
            textures[ name ] = contentManager.Load< Texture2D >( name ); 
        }



        /// <summary>
        /// テクスチャのロード
        /// </summary>
        /// <param name="name"></param>         名前
        /// <param name="folderName"></param>   フォルダー名( \\付き )
        public void LoadTexture( string name, string folderName )
        { 
            textures[ name ] = contentManager.Load< Texture2D >( folderName + name ); 
        }



        /// <summary>
        /// 解放
        /// </summary>
        public void Unload()
        {
            textures.Clear();           // テクスチャコレクション
            contentManager.Unload();    // コンテンツマネージャー
        }



        /// <summary>
        /// コンテンツコレクションのクリア
        /// </summary>
        public void ClearContentCollection() 
        {
            textures.Clear();           // テクスチャコレクション
        }



        /// <summary>
        /// 描画の開始
        /// </summary>
        public void Begin() { spriteBatch.Begin(); }



        /// <summary>
        /// 描画の終了
        /// </summary>
        public void End() { spriteBatch.End(); }



        /// <summary>
        /// 描画
        /// 通常表示
        /// </summary>
        /// <param name="name"></param>                 名前
        /// <param name="position"></param>             座標
        public void DrawTexture( string name, Vector2 position )
        {
            spriteBatch.Draw
            ( 
                textures[ name ],               // テクスチャ
                position,                       // 座標
                Color.White                     // RGBA                 (指定無し)
            );
        }



        /// <summary>
        /// 描画
        /// 画面中央に表示
        /// </summary>
        /// <param name="name"></param>                 名前
        /// <param name="leftUpPos_Y"></param>          Y座標
        /// <param name="texSize"></param>              画像サイズ
        public void DrawTexture( string name, float positionY, Vector2 texSize )
        {
            Vector2 drawPos = new Vector2( (Screen.Width - texSize.X) / 2, positionY );
            spriteBatch.Draw
            ( 
                textures[ name ],               // テクスチャ
                drawPos,                        // 座標
                Color.White                     // RGBA                 (指定無し)
            );
        }



        /// <summary>
        /// 描画
        /// 透過表示
        /// </summary>
        /// <param name="name"></param>                 名前
        /// <param name="position"></param>             座標
        /// <param name="blend"></param>                RGBA
        public void DrawTexture( string name, Vector2 position, int blend )
        {
            spriteBatch.Draw
            (
                textures[ name ],                           // テクスチャ
                position,                                   // 座標
                new Color( blend, blend, blend, blend )     // RGBA
            );
        }



        /// <summary>
        /// 描画
        /// 画面中央に表示
        /// 点滅表示
        /// </summary>
        /// <param name="name"></param>                 名前
        /// <param name="positionY"></param>            Y座標
        /// <param name="texSize"></param>              画像サイズ
        /// <param name="interval"></param>             点滅間隔(秒)
        public void DrawTexture( string name, float positionY, Vector2 texSize, double interval )
        {
            // カウントアップ
            flashingCountTimer++;


            // 一定間隔で点滅
            double  time    = 60.0 * interval;
            int     blend   = 255;
            if ( flashingCountTimer < time ) 
            {
                blend = 255;
            }
            else if ( flashingCountTimer < time * 2 ) 
            {   
                blend = 100;
            }
            else flashingCountTimer = 0;

            Vector2 drawPos = new Vector2( (Screen.Width - texSize.X) / 2, positionY );
            spriteBatch.Draw
            (
                textures[ name ],                           // テクスチャ
                drawPos,                                    // 座標
                new Color( blend, blend, blend, blend )     // RGBA
            );
        }



        /// <summary>
        /// 描画
        /// 切り出し表示
        /// </summary>
        /// <param name="name"></param>                 名前
        /// <param name="position"></param>             座標
        /// <param name="rect"></param>                 切り出し位置
        public void DrawTexture( string name, Vector2 position, Rectangle rect )
        {
            spriteBatch.Draw
            (
                textures[ name ],               // テクスチャ
                position,                       // 座標
                rect,                           // 切り出し位置
                Color.White                     // RGBA                 (指定無し)
            );
        }



        /// <summary>
        /// 描画
        /// 拡大縮小表示
        /// </summary>
        /// <param name="name"></param>                 名前
        /// <param name="position"></param>             座標
        /// <param name="scale"></param>                拡大倍率
        public void DrawTexture( string name, Vector2 position, Vector2 scale )
        {
            spriteBatch.Draw
            (
                textures[ name ],           // テクスチャ
                position,                   // 座標
                null,                       // 切り出し位置         (指定無し)
                Color.White,                // RGBA                 (指定無し)
                0,                          // 回転角度             (指定無し)
                new Vector2( 0, 0 ),        // 回転の中心座標       (指定無し)
                scale,                      // 拡大倍率
                SpriteEffects.None,         // エフェクト           (指定無し)
                0.0f                        // レイヤー             (指定無し)
            );
        }



        /// <summary>
        /// 描画
        /// 拡大縮小表示
        /// </summary>
        /// <param name="name"></param>                 名前
        /// <param name="position"></param>             座標
        /// <param name="scale"></param>                拡大倍率
        public void DrawTexture( string name, Vector2 position, float scale )
        {
            spriteBatch.Draw
            (
                textures[ name ],           // テクスチャ
                position,                   // 座標
                null,                       // 切り出し位置         (指定無し)
                Color.White,                // RGBA                 (指定無し)
                0,                          // 回転角度             (指定無し)
                new Vector2( 0, 0 ),        // 回転の中心座標       (指定無し)
                scale,                      // 拡大倍率
                SpriteEffects.None,         // エフェクト           (指定無し)
                0.0f                        // レイヤー             (指定無し)
            );
        }



        /// <summary>
        /// 描画
        /// 切り出し表示
        /// 拡大縮小表示
        /// </summary>
        /// <param name="name"></param>                 名前
        /// <param name="position"></param>             座標
        /// <param name="rect"></param>                 切り出し位置
        /// <param name="scale"></param>                拡大倍率
        public void DrawTexture( string name, Vector2 position, Rectangle rect, Vector2 scale )
        {
            spriteBatch.Draw
            (
                textures[ name ],           // テクスチャ
                position,                   // 座標
                rect,                       // 切り出し位置
                Color.White,                // RGBA                 (指定無し)
                0,                          // 回転角度             (指定無し)
                new Vector2( 0, 0 ),        // 回転の中心座標       (指定無し)
                scale,                      // 拡大倍率
                SpriteEffects.None,         // エフェクト           (指定無し)
                0.0f                        // レイヤー             (指定無し)
            );
        }



        /// <summary>
        /// 描画
        /// 透過表示
        /// 拡大縮小表示
        /// </summary>
        /// <param name="name"></param>                 名前
        /// <param name="position"></param>             座標
        /// <param name="blend"></param>                RGBA
        /// <param name="scale"></param>                拡大倍率
        public void DrawTexture( string name, Vector2 position, int blend, Vector2 scale )
        {
            spriteBatch.Draw
            (
                textures[ name ],           // テクスチャ
                position,                   // 座標
                null,                       // 切り出し位置         (指定無し)
                new Color(                  // RGBA
                    blend, 
                    blend, 
                    blend, 
                    blend 
                ),                          
                0,                          // 回転角度             (指定無し)
                new Vector2( 0, 0 ),        // 回転の中心座標       (指定無し)
                scale,                      // 拡大倍率
                SpriteEffects.None,         // エフェクト           (指定無し)
                0.0f                        // レイヤー             (指定無し)
            );
        }



        /// <summary>
        /// 描画
        /// 切り出し表示
        /// 透過表示
        /// 拡大縮小表示
        /// </summary>
        /// <param name="name"></param>                 名前
        /// <param name="position"></param>             座標
        /// <param name="rect"></param>                 切り出し位置
        /// <param name="blend"></param>                RGBA
        /// <param name="scale"></param>                拡大倍率
        public void DrawTexture( string name, Vector2 position, Rectangle rect, int blend, Vector2 scale )
        {
            spriteBatch.Draw
            (
                textures[ name ],           // テクスチャ
                position,                   // 座標
                rect,                       // 切り出し位置
                new Color(                  // RGBA
                    blend,
                    blend,
                    blend,
                    blend
                ),    
                0,                          // 回転角度             (指定無し)
                new Vector2( 0, 0 ),        // 回転の中心座標       (指定無し)
                scale,                      // 拡大倍率
                SpriteEffects.None,         // エフェクト           (指定無し)
                0.0f                        // レイヤー             (指定無し)
            );
        }



        /// <summary>
        /// 描画
        /// 回転表示
        /// </summary>
        /// <param name="name"></param>                 名前
        /// <param name="position"></param>             座標
        /// <param name="rotate"></param>               回転角度
        /// <param name="center"></param>               回転の中心座標
        public void DrawTexture( string name, Vector2 position, float rotate, Vector2 center )
        {
            spriteBatch.Draw
            (
                textures[ name ],           // テクスチャ
                position,                   // 座標
                null,                       // 切り出し位置         (指定無し)
                Color.White,                // RGBA                 (指定無し)
                rotate,                     // 回転角度             
                center,                     // 回転の中心座標       
                1,                          // 拡大倍率             (指定無し)
                SpriteEffects.None,         // エフェクト           (指定無し)
                0.0f                        // レイヤー             (指定無し)
            );
        }



        /// <summary>
        /// 数値の描画
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="position">座標</param>
        /// <param name="numberSize">数字1文字のサイズ</param>
        /// <param name="number">描画する数値</param>
        /// <param name="digit">桁数</param>
        public void DrawNumber( string name, Vector2 position, Vector2 numberSize, int number, int digit )
        {
            DrawNumber( name, position, numberSize, new Vector2(1, 1), number, digit );
        }



        /// </summary>
        /// 数値の描画
        /// 拡大表示
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="position">座標</param>
        /// <param name="numberSize">数字1文字のサイズ</param>
        /// <param name="scale">拡大倍率</param>
        /// <param name="number">描画する数値</param>
        /// <param name="digit">桁数</param>
        public void DrawNumber( string name, Vector2 position, Vector2 numberSize, Vector2 scale, int number, int digit )
        {
            // 0未満は0に設定
            if ( number < 0 ) number = 0;


            // 桁数に合わせて座標を調整
            position.X += numberSize.X * ( digit - 1 ) * scale.X;

            // 最下位から1文字ずつ描画
            for ( int i = 0; i < digit; ++i )
            {
                int         n       = number % 10;      // 描画する1文字
                Rectangle   rect    =                   // 画像から切り出す位置
                    new Rectangle( 
                        n * (int)numberSize.X, 
                        0, 
                        (int)numberSize.X, 
                        (int)numberSize.Y 
                    );


                // 1文字を描画
                DrawTexture( name, position, rect, scale );

                // 桁上げ
                number /= 10;

                // 座標の調整
                position.X -= numberSize.X * scale.X;
            }
        }
    }
}
