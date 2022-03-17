using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 来自 https://github.com/aceyan/UnityEffects
/// UV贴图闪电链
/// 使用了一个递归的分治算法来生成闪电链各段的位置。然后设置lineRender的position集合来模拟闪电效果，加入uv动画效果更佳
/// 具体原理见我的OneNote
/// </summary>

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class UVChainLightning : MonoBehaviour
{
    public float Detail = 1;        //增加后，线条数量会减少，每个线条会更长。
    public float Displacement = 8; //位移量，也就是线条数值方向偏移的最大值

    public Transform ChainStart;
    public Transform ChainEnd;

    //public float yOffset = 0;

    private LineRenderer lineRender;
    private List<Vector3> linePosList;

    private void Awake()
    {
        lineRender = GetComponent<LineRenderer>();
        linePosList = new List<Vector3>();
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            linePosList.Clear();
            Vector3 startPos = Vector3.zero;
            Vector3 endPos = Vector3.zero;
            if (ChainEnd != null)
            {
                endPos = ChainEnd.position;// + Vector3.up * yOffset;
            }
            if (ChainStart != null)
            {
                startPos = ChainStart.position;// + Vector3.up * yOffset;
            }

            CollectLinPos(startPos, endPos, Displacement);
            linePosList.Add(endPos);

            //_lineRender.SetVertexCount(_linePosList.Count);
            lineRender.positionCount = linePosList.Count;

            for (int i = 0, n = linePosList.Count; i < n; i++)
            {
                lineRender.SetPosition(i, linePosList[i]);
            }
        }
    }

    //收集顶点，中点分形法插值抖动
    private void CollectLinPos(Vector3 startPos, Vector3 destPos, float displace)
    {
        if (displace < Detail)
        {
            linePosList.Add(startPos);
        }
        else
        {

            float midX = (startPos.x + destPos.x) / 2;
            float midY = (startPos.y + destPos.y) / 2;
            float midZ = (startPos.z + destPos.z) / 2;

            midX += (float)(UnityEngine.Random.value - 0.5) * displace;
            midY += (float)(UnityEngine.Random.value - 0.5) * displace;
            midZ += (float)(UnityEngine.Random.value - 0.5) * displace;

            Vector3 midPos = new Vector3(midX, midY, midZ);

            CollectLinPos(startPos, midPos, displace / 2);
            CollectLinPos(midPos, destPos, displace / 2);
        }
    }

    public void ClearChainLightning()
    {
        enabled = false;
        lineRender = null;
        linePosList.Clear();
        linePosList = null;
    }
}
