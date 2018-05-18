using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace XNA2DGame
{
    /// <summary>
    /// ムービークラス
    /// シングルトン
    /// </summary>
    class Movie
    {
        // メンバー変数
        private ContentManager  contentManager      = DeviceManager.Instance.GetCM;         // コンテンツマネージャー
        private SpriteBatch     spriteBatch                                                 // スプライトバッチ
            = new SpriteBatch( DeviceManager.Instance.GetGDM.GraphicsDevice );
        private VideoPlayer     videoPlayer         = new VideoPlayer();                    // ビデオプレイヤー

        // ムービーコレクション
        private Dictionary< string, Video > movies  = new Dictionary< string, Video >();

        // 自己インスタンス
        private static readonly Movie instance      = new Movie();



        /// <summary>
        /// コンストラクタ
        /// </summary>
        static Movie() { }
        private Movie() { }



        /// <summary>
        /// インスタンスゲッター
        /// </summary>
        public static Movie Instance 
        {
            get { return instance; }
        }



        /// <summary>
        /// ビデオプレイヤーゲッター
        /// </summary>
        public VideoPlayer VideoPlayer
        {
            get { return videoPlayer; }
        }



        /// <summary>
        /// 解放
        /// </summary>
        public void Unload() 
        {
            movies.Clear();             // コレクション
            contentManager.Unload();    // コンテンツマネージャー
        }



        /// <summary>
        /// ムービーのロード
        /// </summary>
        /// <param name="name"></param>         名前
        /// <param name="loop"></param>         ループ
        public void LoadMovie( string name, bool loop )
        {
            // ムービーのロード
            movies[ name ] = contentManager.Load< Video >( name );

            // ループの設定
            videoPlayer.IsLooped = loop;
        }



        /// <summary>
        /// ムービーのロード
        /// </summary>
        /// <param name="name"></param>         名前
        /// <param name="folderName"></param>   フォルダー名( \\付き )
        /// <param name="loop"></param>         ループ
        public void LoadMovie( string name, string folderName, bool loop )
        {
            // ムービーのロード
            movies[ name ] = contentManager.Load< Video >( folderName + name );

            // ループの設定
            videoPlayer.IsLooped = loop;
        }



        /// <summary>
        /// ムービーの開始
        /// </summary>
        public void Begin() { spriteBatch.Begin(); }



        /// <summary>
        /// ムービーの終了
        /// </summary>
        public void End() { spriteBatch.End(); }



        /// <summary>
        /// ムービーの再生
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position"></param>
        public void Play( string name, Vector2 position )
        {
            // ムービーの再生
            videoPlayer.Play( movies[ name ] );

            // 再生中のムービーの現在のフレームを含むテクスチャの取得
            Texture2D texture = videoPlayer.GetTexture();

            // テクスチャの描画
            spriteBatch.Draw( texture, position, Color.White );
        }



        /// <summary>
        /// ムービーの停止
        /// </summary>
        public void Stop() { videoPlayer.Stop(); }


        
        /// <summary>
        /// ムービーの一時停止
        /// </summary>
        public void Pause() { videoPlayer.Pause(); }



        /// <summary>
        /// ムービーの再開
        /// </summary>
        public void Resume() { videoPlayer.Resume(); }
    }
}
