//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemySense : MonoBehaviour
//{
//    public TriggerReactor knowZone;
//    public TriggerReactor vagueZone;

//    public Vector3 getSensedPos()
//    {
//        if (vagueZone.isColliding())
//            return SardineSwim.playerTransform.position;
//        else
//            return vagueZone.lastPosition;
//    }

//    public int getAwareness()
//    {
//        if (knowZone.isColliding())
//            return 2;
//        else if (vagueZone.isColliding())
//            return 1;
//        else
//            return 0;
//    }
//}
