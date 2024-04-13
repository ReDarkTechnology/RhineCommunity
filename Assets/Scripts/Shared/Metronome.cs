using System.Collections.Generic;
using System;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    public List<BPMInfo> bpms = new List<BPMInfo>() {
        new BPMInfo()
    };

    public void AddBeat(BPMInfo change)
    {
        BPMInfo similar = bpms.Find(val => val.time == change.time);
        if (similar == null)
            bpms.Add(change);
        /*else
            throw new Exception("Beat change with similar time is found, please choose a different time");*/

        SortAllBeats();
    }

    public bool IsTimeTakenByBeat(BeatTime time)
    {
        return bpms.Find(val => val.time == time) != null;
    }

    public void SortAllBeats()
    {
        bpms.Sort((a, b) => a.time.GetWholeValue().CompareTo(b.time.GetWholeValue()));
    }

    public void ResetBeats()
    {
        bpms.Clear();
        bpms.Add(new BPMInfo());
    }

    public void SetBeat(int index, BPMInfo info)
    {
        bpms.RemoveAt(index);
        bpms.Insert(index, info);
    }

    public int GetBPMIndex(BeatTime time)
    {
        for (int i = 1; i < bpms.Count; i++)
        {
            if (time < bpms[i].time)
                return i - 1;
        }
        return bpms.Count - 1;
    }

    public void GetSecondsRangeFromBeat(BeatTime beatStart, BeatTime beatEnd, out float start, out float end)
    {
        start = GetSecondsFromBeat(beatStart, 0, 0, out float tr, out int iR);
        end = beatStart == beatEnd ? start : GetSecondsFromBeat(beatEnd, iR, tr, out float r, out int g);
    }

    public float GetSecondsFromBeat(BeatTime beat) => GetSecondsFromBeat(beat, 0, 0, out float r, out int g);

    /// <summary>
    /// Get seconds from the beat with the correct BPM information
    /// </summary>
    /// <param name="beat">The beat</param>
    /// <param name="index">Index to start from</param>
    /// <param name="t">T to check from</param>
    /// <param name="tR">Last T after checking</param>
    /// <param name="iR">Last index after checking</param>
    /// <returns></returns>
    public float GetSecondsFromBeat(BeatTime beat, int index, float t, out float tR, out int iR)
    {
        float bpm = bpms[index].bpm;
        BeatTime tP = bpms[index].time;

        for(int i = index + 1; i < bpms.Count; i++)
        {
            if(beat > bpms[i].time)
            {
                float decr = (bpms[i].time - tP).ToSeconds(bpm);
                bpm = bpms[i].bpm;
                tP = bpms[i].time;
                t += decr;
            }
            else
            {
                tR = t;
                iR = i;

                float decr = (beat - tP).ToSeconds(bpm);
                t += decr;
                return t;
            }
        }
        tR = t;
        iR = bpms.Count - 1;
        float final = (beat - tP).ToSeconds(bpm);
        t += final;
        return t;
    }

    public BeatTime GetBeatFromSeconds(float time) => GetBeatFromSeconds(time, 0, new BeatTime(), out BeatTime r, out int g);
    public BeatTime GetBeatFromSeconds(float time, int index, BeatTime t, out BeatTime tR, out int iR)
    {
        if (time == 0)
        {
            tR = new BeatTime();
            iR = 0;
            return tR;
        }

        float bpm = bpms[index].bpm;
        float s = bpms[index].time.ToSeconds(bpm);

        for (int i = index + 1; i < bpms.Count; i++)
        {
            var bpt = s + bpms[i].time.ToSeconds(bpm);
            if (time > bpt)
            {
                s = bpt;
                t = bpms[i].time;
                bpm = bpms[i].bpm;
            }
            else
            {
                tR = t;
                t = t + BeatTime.FromNumber(time - s, bpm);
                iR = i;
                return t;
            }
        }

        tR = t;
        t = t + BeatTime.FromNumber(time - s, bpm);
        iR = bpms.Count - 1;
        return t;
    }

    public BeatTime GetPreviousBeat(BeatTime time)
    {
        int index = GetBPMIndex(time);
        time.denominator = bpms[index].signature;
        if (time.numerator > 0)
        {
            time.numerator--;
        }
        else
        {
            time.beat--;
            time.numerator = time.denominator - 1;
        }

        if(time < bpms[index].time && index > 0)
        {
            time = SnapToBPMGrid(bpms[index - 1], time);
        }
        return time;
    }

    public BeatTime GetNextBeat(BeatTime time)
    {
        int index = GetBPMIndex(time);
        time.denominator = bpms[index].signature;
        if(time.numerator < time.denominator - 1)
        {
            time.numerator++;
        }
        else
        {
            time.beat++;
            time.numerator = 0;
        }

        if(index < bpms.Count - 1)
        {
            if (bpms[index + 1].time < time)
                time = bpms[index + 1].time;
        }
        return time;
    }

    public BeatTime SnapTime(BeatTime time)
    {
        int index = GetBPMIndex(time);
        return SnapToBPMGrid(bpms[index], time);
    }

    public BeatTime SnapToBPMGrid(BPMInfo info, BeatTime time)
    {
        var sig = time.GetSignature();
        float sg = sig * info.signature;
        int num = (int)sg;
        time.numerator = num;
        time.denominator = info.signature;
        return time;
    }
}