using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subsystem : MonoBehaviour
{
    protected RobotContainer robotContainer;

    public void SetRobotContainer(RobotContainer robotContainer)
    {
        this.robotContainer = robotContainer;
    }
}
