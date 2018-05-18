using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace XNA2DGame
{
    /// <summary>
    /// サウンドクラス
    /// シングルトン
    /// </summary>
    class Sound
    {
        // メンバー変数
        private uint cloneSENo = 0;     // SE複製用

        private ContentManager  contentManager = DeviceManager.Instance.GetCM;  // コンテンツマネージャー
        private SoundEffect     soundEffect;                                    // サウンドエフェクト

        // サウンドコレクション
        private Dictionary< string, SoundEffectInstance > sounds 
            = new Dictionary< string,SoundEffectInstance >();

        // 自己インスタンス
        private static readonly Sound instance = new Sound();



        /// <summary>
        /// コンストラクタ
        /// </summary>
        static Sound() { }
        private Sound() { }



        /// <summary>
        /// インスタンスゲッター
        /// </summary>
        public static Sound Instance 
        {
            get { return instance; }
        }



        /// <summary>
        /// 解放
        /// </summary>
        public void Unload()
        {
            sounds.Clear();             // サウンドコレクション
            contentManager.Unload();    // コンテンツマネージャー
        }



        /// <summary>
        /// コンテンツコレクションのクリア
        /// </summary>
        public void ClearContentCollection()
        {
            sounds.Clear();             // サウンドコレクション
        }



        /// <summary>
        /// サウンドのロード
        /// </summary>
        /// <param name="name"></param>  名前
        /// <param name="loop"></param>  ループ
        public void LoadSound( string name, bool loop )
        {
            // サウンドのロード
            soundEffect = contentManager.Load< SoundEffect >( name );

            // サウンドのインスタンス化
            sounds[ name ] = soundEffect.CreateInstance();

            // ループの設定
            sounds[ name ].IsLooped = loop;
        }


        
        /// <summary>
        /// サウンドのロード
        /// </summary>
        /// <param name="name"></param>         名前
        /// <param name="folderName"></param>   フォルダー名( \\付き )
        /// <param name="loop"></param>         ループ
        public void LoadSound( string name, string folderName, bool loop )
        {
            // サウンドのロード
            soundEffect = contentManager.Load< SoundEffect >( folderName + name );

            // サウンドのインスタンス化
            sounds[ name ] = soundEffect.CreateInstance();

            // ループの設定
            sounds[ name ].IsLooped = loop;
        }



        /// <summary>
        /// サウンドの再生
        /// </summary>
        /// <param name="name"></param>
        public void Play( string name )
        {
            // サウンドが再生状態でない場合
            if ( sounds[ name ].State != SoundState.Playing ) 
            {
                sounds[ name ].Play();  // サウンドの再生
            }

            // SEの二重再生
            else if ( !sounds[ name ].IsLooped ) 
            {
                // サウンドのロード
                soundEffect = contentManager.Load< SoundEffect >( name );

                // サウンドのインスタンス化
                string tmpName = name + "_clone_" + ++cloneSENo; //cloneSENo++;
                sounds[ tmpName ] = soundEffect.CreateInstance();

                // ループの設定
                sounds[ tmpName ].IsLooped = false;
                


                // 再生
                sounds[ tmpName ].Play();
            }



            // 不要なSEの削除
            foreach ( var pair in sounds )
            {
                if ( !pair.Value.IsLooped ) 
                {
                    if ( pair.Key.Contains("_clone_") ) 
                    {
                        if ( pair.Value.State == SoundState.Stopped ) 
                        {
                            sounds.Remove( pair.Key );
                            break;
                        }
                    }
                }
            }
        }



        /// <summary>
        /// サウンドの停止
        /// </summary>
        /// <param name="name"></param>
        public void Stop( string name )
        {
            sounds[ name ].Stop();  // サウンドの停止
        }
    }
}
