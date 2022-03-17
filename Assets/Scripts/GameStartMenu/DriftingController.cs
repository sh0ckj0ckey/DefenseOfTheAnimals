using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主要原理是把物体的position和rotation变化用Sin或Cos运动曲线来表示，实现随机效果。
/// 
/// 正弦曲线可以表示为y=Asin(ωx+φ)+k，定义为函数y=Asin(ωx+φ)+k在直角坐标系上的图象，
/// 其中sin为正弦符号，x是直角坐标系x轴上的数值，y是在同一直角坐标系上函数对应的y值，k、ω和φ是常数（k、ω、φ∈R且ω≠0）。
/// 
/// 可以这样理解，A是运动曲线的振幅，ω是角速度，大小为2π*f（f=1/T），控制正弦周期，φ为x=0时的相位，k为偏距，即曲线沿y轴上下移动的值。这样就可以用这个公式来表示曲线运动了。
/// </summary>
public class DriftingController : MonoBehaviour
{
    //X、Y、Z轴上正弦曲线的ω值
    public float wX = 1;
    public float wY = 1;
    public float wZ = 1;
    //X、Y、Z轴上正弦曲线的A值
    public float aX = 0.2f;
    public float aY = 0.2f;
    public float aZ = 0.2f;
    public float speed = 1f;
    public float range = 1;
    private float t = 0;
    private Vector3 originPos;
    private Vector3 originRotation;

    void Awake()
    {
        originPos = transform.position;
        originRotation = transform.eulerAngles;
    }

    void Update()
    {
        t += Time.deltaTime * speed;
        transform.position = originPos + new Vector3(aX * Mathf.Sin(wX * t), aY * Mathf.Sin(wY * t), aZ * Mathf.Sin(wZ * t)) * range;
        transform.rotation = Quaternion.Euler(originRotation + new Vector3(aX * 20 * Mathf.Sin(wX * t), aY * 20 * Mathf.Sin(wY * t), aZ * 20 * Mathf.Sin(wZ * t)));
    }
}
