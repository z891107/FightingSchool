using System;
using System.Collections.Generic;
using UnityEngine;

public struct Resource {
    public int food;
    public int iron;
    public int book;

    public Resource(int newResourceNum) : this(newResourceNum, newResourceNum, newResourceNum) {}

    public Resource(int newFood, int newIron, int newBook) {
        food = newFood;
        iron = newIron;
        book = newBook;
    }

    static public Resource operator+(Resource lhs, Resource rhs) {
        return new Resource(lhs.food + rhs.food, lhs.iron + rhs.iron, lhs.book + rhs.book);
    }
    static public Resource operator-(Resource lhs, Resource rhs) {
        return new Resource(lhs.food - rhs.food, lhs.iron - rhs.iron, lhs.book - rhs.book);
    }
    static public bool operator>=(Resource lhs, Resource rhs) {
        return (lhs.food >= rhs.food &&
                lhs.iron >= rhs.iron &&
                lhs.book >= rhs.book);
    }
    static public bool operator<=(Resource lhs, Resource rhs) {
        return (lhs.food <= rhs.food &&
                lhs.iron <= rhs.iron &&
                lhs.book <= rhs.book);
    }
}

public class ResourceHandler : MonoBehaviour
{
    public TurnHandler mTurnHandler;
    public PieceManager mPieceManager;

    Resource[] mTeamsResource = new Resource[4];

    public Resource mCurrentTeamResource {
        get {
            return mTeamsResource[mTurnHandler.mCurrentTeamNum];
        }
        set {
            mTeamsResource[mTurnHandler.mCurrentTeamNum] = value;
        }
    }

    Dictionary<Type, Resource> mSpawningTypeCostTable = new Dictionary<Type, Resource> {
        { typeof(Powerful),      new Resource(3, 3, 0) },
        { typeof(Communicative), new Resource(1, 1, 4) },
        { typeof(Charming),      new Resource(2, 0, 4) },
        { typeof(Rich),          new Resource(2, 2, 2) },
        { typeof(Speedy),        new Resource(1, 4, 1) },
        { typeof(Energetic),     new Resource(0, 3, 3) },
        { typeof(Healthy),       new Resource(3, 1, 2) },
    };

    Dictionary<AttackRange, Resource> mSpawningAttackRangeCostTable = new Dictionary<AttackRange, Resource> {
        { AttackRange.Melee,  new Resource(1, 0, 0) },
        { AttackRange.Ranged, new Resource(0, 1, 0) },
    };

    void Awake() {
        mPieceManager.Spawned += OnSpawn;
        mTurnHandler.NextRound += OnNextRound;
    }

    public void Init() {
        for (int teamNum = 0; teamNum < 4; teamNum++) {
            mTeamsResource[teamNum] = new Resource(5);
        }
    }

    public bool IsEnoughResourceToSpawn(Type type) {
        Resource resource = mSpawningTypeCostTable[type];

        return mCurrentTeamResource >= resource;
    }

    public bool IsEnoughResourceToSpawn(Type type, AttackRange range) {
        Resource resource = mSpawningTypeCostTable[type] + mSpawningAttackRangeCostTable[range];

        return mCurrentTeamResource >= resource;
    }

    public void OnSpawn(object sender, PieceManagerEventData e) {
        Resource resource = mSpawningTypeCostTable[e.mPieceType] + mSpawningAttackRangeCostTable[e.mPieceRange];

        mCurrentTeamResource -= resource;
    }

    public void OnNextRound(object sender, TurnHandlerEventData e) {
        for (int teamNum = 0; teamNum < 4; teamNum++) {
            mTeamsResource[teamNum] += new Resource(2);
        }
    }
}
