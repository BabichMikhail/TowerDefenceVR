using System.Collections.Generic;

public class MyIntTuple
{
    public int first;
    public int second;
    public int third;

    public MyIntTuple(int first, int second, int third) { this.first = first; this.second = second; this.third = third; }
}

class Config {
    public const int START_BALANCE = 500;
    public const int CONSTRUCT_TOWER_COST = 100;
    public const int AWARD_FOR_KILL_UNIT = 10;
    public const int SEND_UNIT_INTERVAL = 200;
    public const float ADD_MONEY_INTERVAL = 0.333f;
    public const int MAIN_TOWER_HEALTH = 10000;

    // time in milliseconds, unit count, respawn index
    public static List<MyIntTuple> UNIT_WAVES = new List<MyIntTuple>() {
        new MyIntTuple(1, 1, 2),
        new MyIntTuple(3000, 2, 0),
        new MyIntTuple(23000, 1, 1),
        new MyIntTuple(24000, 1, 2),
        new MyIntTuple(30000, 1, 2),
        new MyIntTuple(40000, 2, 0),
        new MyIntTuple(43000, 3, 1),
        new MyIntTuple(43000, 1, 2),
        new MyIntTuple(80000, 5, 0),
        new MyIntTuple(85000, 5, 1),
        new MyIntTuple(120000, 8, 0),
        new MyIntTuple(121000, 7, 1),
        new MyIntTuple(122000, 4, 2),
        new MyIntTuple(200000, 50, 0),
        new MyIntTuple(200000, 50, 1),
    };
}
