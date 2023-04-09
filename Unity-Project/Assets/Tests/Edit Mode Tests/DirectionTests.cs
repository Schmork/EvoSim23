using NUnit.Framework;
using UnityEngine;

public class DirectionTests
{
    [Test]
    public void BiggerThanZero()
    {
        var myHeading = Vector3.up;
        var myVelocity = new Vector3(10, 5, 0);
        var angle = Vector2.SignedAngle(myVelocity, myHeading);
        Debug.Assert(angle > 0);
    }

    [Test]
    public void SmallerThanZero() {
        var myHeading = Vector3.up;
        var myVelocity = new Vector3(-10, 1, 0);
        var angle = Vector2.SignedAngle(myVelocity, myHeading);
        Debug.Assert(angle < 0);
    }

    [Test]
    public void EqualToZero() {
        var myHeading = Vector3.up;
        var myVelocity = Vector3.up * 5;
        var angle = Vector2.SignedAngle(myVelocity, myHeading);
        Debug.Assert(angle == 0);
    }

    [Test]
    public void EqualTo90()
    {
        var myHeading = Vector3.up;
        var myVelocity = Vector3.left * 5;
        var angle = Vector2.SignedAngle(myVelocity, myHeading);
        Debug.Assert(angle == -90);
    }

    [Test]
    public void EqualToMinus90()
    {
        var myHeading = Vector3.up;
        var myVelocity = Vector3.right * 15;
        var angle = Vector2.SignedAngle(myVelocity, myHeading);
        Debug.Assert(angle == 90);
    }

    [Test]
    public void EqualTo180() { 
        var myHeading = Vector3.up;
        var myVelocity = Vector3.down * 51;
        var angle = Vector2.SignedAngle(myVelocity, myHeading);
        Debug.Assert(angle == 180);
    }
}
