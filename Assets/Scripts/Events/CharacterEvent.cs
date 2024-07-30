using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;



public class CharacterEvents
{
    // Character damaged and damage value 
    public static UnityAction<GameObject, int> characterDamaged;

    // Character healed and amount healed
    public static UnityAction<GameObject, int> characterHealed;
}

