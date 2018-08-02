using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{

    public Dictionary<string, float> countdowns = new Dictionary<string, float>();

    void Start()
    {
        countdowns = new Dictionary<string, float>();
    }

    void Update()
    {
        List<string> keys = new List<string>(countdowns.Keys);
        foreach (string key in keys)
        {
            if (countdowns[key] > 0)
            {
                countdowns[key] = countdowns[key] - Time.deltaTime;
            }
            else if (countdowns[key] < 0)
            {
                RemoveCountdown(key);
            }
        }
    }

    public void AddCountdown(string name, float time) { if (!ContainsCountdown(name)) countdowns.Add(name, time); }

    public void RemoveCountdown(string name) { countdowns.Remove(name); }

    public bool ContainsCountdown(string name) { return countdowns.ContainsKey(name); }

    public bool ContainsCountdownWithSubstring(string name)
    {
        foreach (string key in countdowns.Keys)
        {
            if (key.Contains(name))
            {
                return true;
            }
        }
        return false;
    }

    public float CountdownValue(string name) { return countdowns[name]; }
}