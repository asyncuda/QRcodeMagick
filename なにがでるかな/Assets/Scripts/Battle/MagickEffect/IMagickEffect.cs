using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMagickEffect
{
    /* 
     * 例えば
     * public class Shot : IMagickEffect
     * {
     *      public void Motion(GameObject from, GameObject target) {}
     * }
     * とかしていきたい
     */
    void Motion(GameObject from, GameObject target);
}
