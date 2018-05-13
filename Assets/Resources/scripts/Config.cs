class Config {
    public const int START_BALANCE = 500;
    public const int CONSTRUCT_TOWER_COST = 100;
    public const int AWARD_FOR_KILL_UNIT = 10;
    public const int SEND_UNIT_INTERVAL = 200;
    public const float ADD_MONEY_INTERVAL = 0.333f;
    public const int MAIN_TOWER_HEALTH = 10000;

    public const float AUDIO_SOURCE_PROJECTILE_SOUND_TIME = 5.6f;

    public const string PROJECTILE_CONTAINER_TAG_NAME = "ProjectileContainer";
    public const string ROUTE_CONTAINER_TAG_NAME = "RouteContainer";
    public const string TOWER_CONTAINER_TAG_NAME = "TowerContainer";
    public const string UNIT_CONTAINER_TAG_NAME = "UnitContainer";
    public const string CREATE_TOWER_CANVAS_TAG_NAME = "CreateTowerCanvas";
    public const string MISSING_COLIDERS_CONTAINER_TAG_NAME = "MissingColliders";

    public const string TOWER_POSITION_SELECTED_MODE_MATERIAL = "materials/Location/Gear";
    public const string TOWER_POSITION_SELECTABLE_MODE_MATERIAL = "materials/Location/Teapot Tower Material";
    public const string TOWER_POSITION_NOT_SELECTABLE_MODE_MATERIAL = "materials/DarkRedMaterial";

    public const string TOWER_POSITION_TAP_TAG_NAME = "TowerPositionChild";
}
