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
    /// デバイスマネージャー
    /// シングルトン
    /// </summary>
    class DeviceManager
    {
        // メンバー変数
        private ContentManager          content;        // コンテンツマネージャー
        private GraphicsDeviceManager   graphics;       // グラフィックデバイスマネージャー

        // 自己インスタンス
        private static readonly DeviceManager instance = new DeviceManager();



        /// <summary>
        /// コンストラクタ
        /// </summary>
        static DeviceManager() { }
        private DeviceManager() { }



        /// <summary>
        /// インスタンスゲッター
        /// </summary>
        public static DeviceManager Instance
        {
            get { return instance; }
        }



        /// <summary>
        /// デバイス設定
        /// </summary>
        /// <param name="content"></param>
        /// <param name="graphics"></param>
        public void SetDevice( ContentManager content, GraphicsDeviceManager graphics ) 
        {
            this.content    = content;      // コンテンツマネージャー
            this.graphics   = graphics;     // グラフィックデバイス
        }



        /// <summary>
        /// コンテンツマネージャーゲッター
        /// </summary>
        public ContentManager GetCM 
        {
            get { return content; }
        }



        /// <summary>
        /// グラフィックデバイスマネージャーゲッター
        /// </summary>
        public GraphicsDeviceManager GetGDM 
        {
            get { return graphics; }
        }



        /// <summary>
        /// 解放
        /// </summary>
        public void Unload() 
        {
            content.Unload();   // コンテンツマネージャー
        }
    }
}
