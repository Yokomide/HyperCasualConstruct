using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationAdder : MonoBehaviour
{
    [SerializeField] MobHolder _holder;
    private bool _active = true;
    private void Update()
    {
        if (_active)
        {

            if (Input.GetKeyDown(KeyCode.F))
            {
               // _holder.AddMob(gameObject);
                _active = false;

            }
        }
    }
}
