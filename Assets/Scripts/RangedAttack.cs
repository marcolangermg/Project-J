using UnityEngine;

[System.Serializable]
public class RangedAttack {
    public GameObject projectilePrefab;
    public float shootForce = 10f;
    public float maxDistance = 10f;
    public float shootDelay = 0.5f;
    public bool volta = false;
    public float voltaSpeed;
    public bool seDivide = false;

    private float shootTime;
}