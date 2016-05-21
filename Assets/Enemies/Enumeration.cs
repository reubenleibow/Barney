public enum PersonState
{
    Nothing,

    AtWorkOrBench,
    FindingWorkSpot,
    GoToBench,
    GoToHouse,
    GoToPlay,
    GoToWork,
    LookForChildren,
    SpottedPlayer,
    WalkSpotFound,
    Searching,
    Terror,
    Sleeping,
    GoingHome,
}

public enum PersonType
{
    Walker,

    Mom,
    Man,
    Bagie,
    Child,
}

public enum PersonGender
{
    Male,
    Female,
}

public enum MovementState
{
    NotMoving,

    Moving,
    NearDestination,
    AtDestination,
}

public enum MovementType
{
    Stopped,

    Wandering,
    RunToMom,
    GoHome,
    Searching,
}
