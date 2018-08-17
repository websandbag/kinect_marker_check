using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class CustomDepthSourceManager : MonoBehaviour {

    private KinectSensor _Sensor;
    private DepthFrameReader _Reader;   // 深度センサーのクラスを取得
    private ushort[] _Data;             // 


    public ushort[] GetData()
    {
        return _Data;
    }


	void Start () {
        Debug.Log("start");
        _Sensor = KinectSensor.GetDefault();
        if (_Sensor != null)
        {
            // センサーを起動する
            _Reader = _Sensor.DepthFrameSource.OpenReader();

            // ?
            _Data = new ushort[_Sensor.DepthFrameSource.FrameDescription.LengthInPixels];
        }
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("update");
        if (_Reader != null)
        {
            var frame = _Reader.AcquireLatestFrame();
            if(frame != null)
            {
                frame.CopyFrameDataToArray(_Data);
                frame.Dispose();
                frame = null;
            }
        }	
	}
}
