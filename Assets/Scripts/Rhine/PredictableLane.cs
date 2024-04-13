using System.Collections.Generic;

namespace Rhine
{
    public class PredictableLane
    {
        public float distancePerTime = 1;
        public List<LaneChange> changes = new List<LaneChange>();

        public virtual float DistanceUntilPoint(float time) =>
            DistanceCount(0, time);

        public virtual float DistanceCount(float initialTime, float time)
        {
            float startTime = initialTime;
            float distance = 0;

            int changeIndex = GetChangeIndex(initialTime);
            if (changeIndex < 0)
            {
                if (changes.Count > 0)
                {
                    if (time > changes[0].time)
                    {
                        distance += distancePerTime * (changes[0].time - initialTime);
                        startTime = changes[0].time;
                    }
                    else
                    {
                        return distancePerTime * (time - initialTime);
                    }
                }
                else
                {
                    return distancePerTime * (time - initialTime);
                }
            }

            for (int i = changeIndex; i < changes.Count; i++)
            {
                // The change is not the last change
                if (i < changes.Count - 1)
                {
                    if (time > changes[i + 1].time)
                    {
                        distance += changes[i].DistanceCount(startTime, changes[i + 1].time);
                        startTime = changes[i + 1].time;
                    }
                    else
                    {
                        distance += changes[i].DistanceCount(startTime, time);
                        return distance;
                    }
                }
                // The change is the last change
                else
                {
                    distance += distancePerTime * (time - startTime);
                }
            }

            return distance;
        }

        public virtual int GetChangeIndex(float time)
        {
            for (int i = 0; i < changes.Count; i++)
            {
                if (time < changes[i].time)
                    return i - 1;
            }
            return changes.Count - 1;
        }
    }

    public class LaneChange
    {
        public float time;
        public float initialDistancePerTime;
        public float distancePerTime;

        public virtual float DistanceCount(float initialTime, float time)
        {
            float distance = time - initialTime;
            return distancePerTime * distance;
        }
    }

    public class LinearLaneChange : LaneChange
    {
        public override float DistanceCount(float initialTime, float time)
        {
            float distance = (time - initialTime);
            return (initialDistancePerTime * distance) + (distance * (distancePerTime - initialDistancePerTime) / 2);
        }
    }
}
