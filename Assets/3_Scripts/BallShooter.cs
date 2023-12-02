using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShooter : MonoBehaviour
{
    [SerializeField]
    float speed = 60;
    [SerializeField]
    BallProjectile projectilePrefab;

    BallProjectile currentProjectile;

    public System.Action OnBallShot;

    int lastColor;

    private void OnEnable()
    {
        InstantiateProjectile();
    }

    private void OnDisable()
    {
        if (currentProjectile)
            currentProjectile.Explode();
        currentProjectile = null;
    }

    void InstantiateProjectile()
    {
        if (currentProjectile)
            currentProjectile.Explode();
        currentProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity, transform);
        int colorIndex = Random.Range(0, TileColorManager.Instance.ColorCount);
        if (lastColor == colorIndex)
            colorIndex = (colorIndex + 1) % TileColorManager.Instance.ColorCount;
        currentProjectile.SetColorIndex(colorIndex);
        lastColor = colorIndex;
    }

    public void ShootTarget(Vector3 targetPosition, TowerTile target)
    {

        Vector3 fromTo2D = new Vector3(targetPosition.x - transform.position.x, 0, targetPosition.z - transform.position.z);
        float angle = 0;
        bool success = LaunchAngle(speed, fromTo2D.magnitude, targetPosition.y - transform.position.y, Physics.gravity.magnitude, out angle);
        if (!success) {
            fromTo2D = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
            success  = LaunchAngle(speed, fromTo2D.magnitude, target.transform.position.y - transform.position.y, Physics.gravity.magnitude, out angle);
        }
        if (success) {
            angle *= Mathf.Rad2Deg;
            currentProjectile.SetTarget(target);
            currentProjectile.SetVelocity(Quaternion.AngleAxis(angle, -transform.right) * fromTo2D.normalized * speed);
            currentProjectile = null;
            InstantiateProjectile();
            OnBallShot?.Invoke();
        }
    }

    // Taken from: https://github.com/IronWarrior/ProjectileShooting
    public bool LaunchAngle(float speed, float distance, float yOffset, float gravity, out float angle)
    {
        angle = 0;

        float speedSquared = speed * speed;

        float operandA = Mathf.Pow(speed, 4);
        float operandB = gravity * (gravity * (distance * distance) + (2 * yOffset * speedSquared));

        // Target is not in range
        if (operandB > operandA)
            return false;

        float root = Mathf.Sqrt(operandA - operandB);

        angle = Mathf.Atan((speedSquared - root) / (gravity * distance));

        return true;
    }
}
