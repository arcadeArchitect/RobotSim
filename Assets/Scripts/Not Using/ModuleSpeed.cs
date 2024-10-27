using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ModuleSpeed
{
    public Module module;
    public float speed;
    public float angle;
    public Vector2 direction;
    public Vector3 vector;

    public ModuleSpeed(Module module)
    {
        this.module = module;
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
        speed = dir.magnitude;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // if (dir.x < 0 != dir.y < 0) angle += 180f;
        // if (dir.y * dir.x < 0) angle += 180f;
        // module.GetRotation();
        float degAngle = angle * Mathf.Deg2Rad;
        vector = new Vector3(speed * Mathf.Cos(degAngle), 0, speed * Mathf.Sin(degAngle));
        // Debug.DrawRay(module.transform.position, vector, Color.magenta);
        // Debug.DrawRay(module.transform.position + Vector3.up, new Vector3(dir.x, 0, dir.y), Color.red);
    }

    public void RunModule()
    {
        // module.SetInputAndRotation(speed, new Vector3(direction.x, 0, direction.y));
    }

    public Vector3 GetWorldDirection()
    {
        Vector3 v = new Vector3(direction.x, 0f, direction.y);
        return module.transform.TransformDirection(v);
    }

    public void Normalize(float mult)
    {
        speed /= mult;
        direction /= mult;
    }

    public void Draw()
    {
        // Debug.DrawRay(module.transform.position, module.GetForwardVector() * speed, Color.cyan);
    }
}
