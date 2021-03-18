using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropEnemyChild : MonoBehaviour
{
   void OnTriggerEnter(Collider c)
   {
        gameObject.GetComponentInParent<DropEnemyCtrller>().PullTrigger(c);
   }  
}
