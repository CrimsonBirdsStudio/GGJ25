using System;

public class BubblerEnums
{
    public enum Family
    {
        feather
        ,plant
        ,robot
    }

    public enum Shape
    {
        circle
        ,stick
        ,triangle
        ,square
        ,star
    }

    [Flags]
    public enum SpawnType
    {
        None = 0
        ,GoodBubbler = 1
        ,ObstacleRock = 2
        ,ObstacleStopper = 4
        ,BadOscillingGroup = 8
        ,BadFollower = 16
    }

    public enum SpawnAreaType
    {
        FrontOfPlayer
        ,AllAroundView
        ,BehindPlayer
    }

    public enum DespawnMechanic
    {
        None
        ,TimeAndDistance
    }

    public enum RespawnMechanic
    {
        None
        ,RespawnInmediate
    }
}
