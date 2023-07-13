//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TrialScores : MonoBehaviour
//{
//    public static TrialData[] Trials;
//    public int numTrials;
//    public static int numTrialsWon = 8;
//    public static int totalTrials = 60;

//    [System.Serializable]
//    public struct TrialData
//    {
//        public float time;
//        public bool hasWon;
//    }

//    private void Awake()
//    {
//        // Get this from Save Data
//        Trials = new TrialData[numTrials];
//    }

//    public static bool isTrialAlreadyWon(int id)
//    {
//        return Trials[id].hasWon;
//    }

//    public static bool winTrial(int id, float time)
//    {
//        if (Trials[id].hasWon)
//        {
//            if (Trials[id].time < time)
//            {
//                setNewRecord(id, time);
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        else
//        {
//            setNewRecord(id, time);
//            return true;
//        }
//    }

//    static void setNewRecord(int id, float time)
//    {
//        Trials[id].hasWon = true;
//        Trials[id].time = time;
//        numTrialsWon++;
//        // Save this to Save Data!
//    }

//    public static float getTime(int id)
//    {
//        return Trials[id].time;
//    }
//}
