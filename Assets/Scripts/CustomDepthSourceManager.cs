using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class CustomDepthSourceManager : MonoBehaviour {

    private KinectSensor _Sensor;       // キネクトのコントロール
    private DepthFrameReader _Reader;   // 深度センサーのクラスを取得
    private ushort[] _Data;             // 現在の深度情報

    public ushort[] GetData()
    {
        return _Data;
    }

    // 開始処理
    void Start () {
        // Kinectの操作を取得
        _Sensor = KinectSensor.GetDefault();
        
        if (_Sensor != null)
        {
            // センサーを起動する
            _Reader = _Sensor.DepthFrameSource.OpenReader();

            // フレームの初期値退避
            _Data = new ushort[_Sensor.DepthFrameSource.FrameDescription.LengthInPixels];
        }
	}

    // 毎フレームごとの処理
	void Update () {
        if (_Reader != null)
        {
            // 最新の深度情報を取得
            var frame = _Reader.AcquireLatestFrame();

            // 現在のフレーム情報更新
            if(frame != null)
            {
                frame.CopyFrameDataToArray(_Data);
                frame.Dispose();
                frame = null;
            }
        }
	}
    

    // 終了処理
    private void OnApplicationQuit()
    {
        // メモリ開放
        if (_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }

        // kinectをクローズ
        if(_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }
            _Sensor = null;
        }
    }
}
