using System;
using UnityEngine;

public class ResultSenbeiModel
{
    public readonly SenbeiLength senbeiLength;
    public readonly Vector3 Center;
    public readonly BakeTime FrontBakedTime;
    public readonly BakeTime BackBakedTime;
    public readonly float[] BulgeHeghts;

    public ResultSenbeiModel(SenbeiLength senbeiLength,Vector3 center,BakeTime frontBakedTime,BakeTime backBakedTime,float[] bulgeHeghts)
    {
        object[] parameters = new object[] { senbeiLength,frontBakedTime,backBakedTime};
        Type[] paramTypes = new Type[] { typeof(SenbeiLength),typeof(BakeTime),typeof(BakeTime) };
        for(int i =0;i<parameters.Length;i++)
        {
            if (parameters[i] == null)
            {
                throw new ArgumentException($"不正な値です。{i+1}番目のパラメータ:{paramTypes[i]}");
            }
        }

        if (bulgeHeghts.Length ==0)
        {
            throw new ArgumentException($"不正な値です。{4}番目のパラメータ:{typeof(float[])}");
        }
        this.senbeiLength = senbeiLength;
        Center = center;
        FrontBakedTime = frontBakedTime;
        BackBakedTime = backBakedTime;
        BulgeHeghts = bulgeHeghts;
    }
}
