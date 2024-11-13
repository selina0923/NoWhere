using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class Debug : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [HeaderAttribute("Debug")]
    public TextMeshPro renderFPS;
    public TextMeshPro avgRenderFPS;
    public TextMeshPro videoFPS;
    public TextMeshPro trackFPS;
    public TextMeshPro debugStr;
    private float totalRenderDeltaTime = 0;
    private float count = 0;
    void LateUpdate()
    {
        DebugUtils.RenderTick();
        float renderDeltaTime = DebugUtils.GetRenderDeltaTime();        
        float videoDeltaTime = DebugUtils.GetVideoDeltaTime();
        float trackDeltaTime = DebugUtils.GetTrackDeltaTime();
        
        if (renderFPS != null)
        {
            renderFPS.text = string.Format("Render: {0:0.0} fps ({1:0.0} ms)", 1000.0f / renderDeltaTime, renderDeltaTime);
            totalRenderDeltaTime += 1000.0f / renderDeltaTime;
            count += 1;
            avgRenderFPS.text = string.Format("avgRender: {0:0.0} fps", totalRenderDeltaTime / count);
            print(totalRenderDeltaTime / count);
        }
        if (videoFPS != null)
        {
            videoFPS.text = string.Format("Video: {0:0.0} ms ({1:0.} fps)", videoDeltaTime, 1000.0f / videoDeltaTime);
        }
        if (trackFPS != null)
        {
            trackFPS.text = string.Format("Track:   {0:0.0} ms ({1:0.} fps)", trackDeltaTime, 1000.0f / trackDeltaTime);
        }
        if (debugStr != null)
        {
            if (DebugUtils.GetDebugStrLength() > 0)
            {
                if (debugStr.preferredHeight >= debugStr.rectTransform.rect.height)
                    debugStr.text = string.Empty;

                debugStr.text += DebugUtils.GetDebugStr();
                DebugUtils.ClearDebugStr();
            }
        }
    }

    public static class DebugUtils
    {
        private static Queue<long> qRenderTick = new Queue<long>();

        private static Queue<long> qVideoTick = new Queue<long>();

        private static Queue<long> qTrackTick = new Queue<long>();

        private static StringBuilder sb = new StringBuilder(1000);

        public static void RenderTick()
        {
            while (qRenderTick.Count > 49)
            {
                qRenderTick.Dequeue();
            }
            qRenderTick.Enqueue(DateTime.Now.Ticks);
        }

        public static float GetRenderDeltaTime()
        {
            if (qRenderTick.Count == 0)
            {
                return float.PositiveInfinity;
            }
            return (DateTime.Now.Ticks - qRenderTick.Peek()) / 500000.0f;
        }

        public static void VideoTick()
        {
            while (qVideoTick.Count > 49)
            {
                qVideoTick.Dequeue();
            }
            qVideoTick.Enqueue(DateTime.Now.Ticks);
        }

        public static float GetVideoDeltaTime()
        {
            if (qVideoTick.Count == 0)
            {
                return float.PositiveInfinity;
            }
            return (DateTime.Now.Ticks - qVideoTick.Peek()) / 500000.0f;
        }

        public static void TrackTick()
        {
            while (qTrackTick.Count > 49)
            {
                qTrackTick.Dequeue();
            }
            qTrackTick.Enqueue(DateTime.Now.Ticks);
        }

        public static float GetTrackDeltaTime()
        {
            if (qTrackTick.Count == 0)
            {
                return float.PositiveInfinity;
            }
            return (DateTime.Now.Ticks - qTrackTick.Peek()) / 500000.0f;
        }

        public static void AddDebugStr(string str)
        {
            sb.AppendLine(str);
        }

        public static void ClearDebugStr()
        {
            sb.Clear();
        }

        public static string GetDebugStr()
        {
            return sb.ToString();
        }

        public static int GetDebugStrLength()
        {
            return sb.Length;
        }
    }
    
}
