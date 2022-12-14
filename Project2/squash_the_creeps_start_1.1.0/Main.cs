using Godot;
using System;

public class Main : Node
{
#pragma warning disable 649
    // We assign this in the editor, so we don't need the warning about not being assigned.

    [Export]
    public PackedScene MobScene;

#pragma warning restore 649



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Randomize();
    }


    // We also specified this function name in PascalCase in the editor's connection window
    public void OnMobTimerTimeout()
    {
        // Create a new instance of the Mob scene.
        Mob mob = (Mob)MobScene.Instance();

        // Choose a random location on the SpawnPath.
        // We store the reference to the SpawnLocation node.

        var mobSpawnLocation = GetNode<PathFollow>("SpawnPath/SpawnLocation");
        // And give it a random offset.
        mobSpawnLocation.UnitOffset = GD.Randf();

        Vector3 playerPosition = GetNode<Player>("Player").Transform.origin;
        mob.Initialize(mobSpawnLocation.Translation, playerPosition);

        AddChild(mob);

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //      
        //  }
    }
}