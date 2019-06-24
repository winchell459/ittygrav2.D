using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class Parameters2D {

    public float Mass = 10;
    public Vector2 Gravity = new Vector2(0, -1);
    public float GravityCoefficient = 9.81f;

}
