
public enum E_SkillUpgradeType 
{
    None,

    // Dash Tree
    Dash, // Dash to avoid damage
    Dash_CloneOnStart, // Create a clone when dash starts
    Dash_CloneOnStartAndArrival, // Create a clone when dash starts and ends
    Dash_ShardOnStart, // Create a shard when dash starts
    Dash_ShardOnStartAndArrival, // Crearte a shard when dash starts and ends

    // Shard Tree
    Shard, // The shard explodes when touched by an enemy or when time goes up  
    Shard_MoveToEnemy, // Shard will move towards nearest enemy
    Shard_Multicast, // Shard ability can have up to N charges. You can cast them all in a row.
    Shard_Teleport, // You can swap places with the last shard you created
    Shard_TeleportHPRewind // When you swap places with shard, your HP% is the same as it was when you created shard

}
