using System;

[Serializable]
public class BPMInfo
{
    public BeatTime time;
    public float bpm = 120;
    public float signature = 4;

    public BPMInfo() { }
    public BPMInfo(BeatTime time, float bpm = 120, float signature = 4)
    {
        this.time = time;
        this.bpm = bpm;
        this.signature = signature;
    }

    public override string ToString()
    {
        return $"{time}-{bpm}:{signature}";
    }
}
