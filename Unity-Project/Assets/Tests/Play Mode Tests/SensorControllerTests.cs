using NUnit.Framework;
using UnityEngine;

public class SensorControllerTests
{
    void Setup()
    {

    }

    [Test]
    public void ProcessBigger_Test()
    {
        var gameObject1 = new GameObject();
        gameObject1.AddComponent<CircleCollider2D>();
        gameObject1.transform.position = Vector2.up * 2;
    }

    [Test]
    public void ProcessSmaller_Test()
    {

    }
}
