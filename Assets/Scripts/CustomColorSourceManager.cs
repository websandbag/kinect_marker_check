using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;


/// <summary>
/// Kinectで読み込んだ映像をカラーで出力するためのクラス
/// </summary>
public class CustomColorSourceManager : MonoBehaviour {

    public int ColorWidth
    {
        get;
        private set;
    }

    public int ColorHeihgt
    {
        get;
        private set;
    }

    private KinectSensor _Sensor;           // センサーのコントロール
    private ColorFrameReader _Reader;       // 
    private Texture2D _Texture;
    private byte[] _Data;

	// Use this for initialization
	void Start () {

        // kinectのコントローラを取得
        _Sensor = KinectSensor.GetDefault();

        if(_Sensor != null)
        {
            // 色のリーダーを取得する
            _Reader = _Sensor.ColorFrameSource.OpenReader();

            // RGBのフォーマットで、カラー情報を取得
            var frameDesc = _Sensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);

            // テクスチャのサイズ指定
            ColorWidth = frameDesc.Width;
            ColorHeihgt = frameDesc.Height;

            // 2Dの色のテクスチャ作成
            _Texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.RGBA32, false);

            // RGBのバイトピクセル
            _Data = new byte[frameDesc.BytesPerPixel * frameDesc.LengthInPixels];
            

            // kinectが起動していなければ起動する
            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }

        }
        
	}
	
	// 更新時の処理
	void Update () {
		if(_Reader != null)
        {
            // 最新のカラー情報を取得
            var frame = _Reader.AcquireLatestFrame();

            // フレーム情報を更新
            if(frame != null)
            {
                // テクスチャの色を更新
                frame.CopyConvertedFrameDataToArray(_Data, ColorImageFormat.Rgba);
                _Texture.LoadRawTextureData(_Data);
                _Texture.Apply();

                // 退避用のカラー情報初期化
                frame.Dispose();
                frame = null;
            }
        }
	}

    // kinect閉じる
    void OnApplicationQuit()
    {
        if (_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }

        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }

            _Sensor = null;
        }
    }
}
