using Godot;
using System;

public class Mob : KinematicBody
{
    // Don't forget to rebuild the project so the editor knows about the new export variable.

    // Minimum speed of the mob in meters per second
    [Export]
    public int MinSpeed = 10;

    // Maximum speed of the mob in meters per second
    public int MaxSpeed = 18;

    // Emitted when the played jumped on the mob.
    [Signal]
    public delegate void Squashed();

    // Emitted when the player was hit by a mob.
    [Signal]
    public delegate void Hit();


    private Vector3 _velocity = Vector3.Zero;

    public override void _PhysicsProcess(float delta)
    {
        MoveAndSlide(_velocity);
    }

    // We will call this function from the Main scene
    public void Initialize(Vector3 startPosition, Vector3 playerPosition)
    {
        // We position the mob and turn it so that it looks at the player.
        LookAtFromPosition(startPosition, playerPosition, Vector3.Up);
        // And rotate it randomly so it doesn't move exactly toward the player.
        RotateY((float)GD.RandRange(-Mathf.Pi / 4.0, Mathf.Pi / 4.0));

        // We calculate a random speed.
        float randomSpeed = (float)GD.RandRange(MinSpeed, MaxSpeed);

        // We calculate a forward velocity that represents the speed.
        _velocity = Vector3.Forward * randomSpeed;

        // We then rotate the vector based on the mob's Y rotation to move in the direction it's looking.
        _velocity = _velocity.Rotated(Vector3.Up, Rotation.y);
    }

    // We also specified this function name in PascalCase in the editor's connection window
    public void OnVisibilityNotifierScreenExited()
    {
        QueueFree();
    }

    public void Squash()
    {
        EmitSignal(nameof(Squashed));
        QueueFree();
    }

    private void Die()
    {
        EmitSignal(nameof(Hit));
        QueueFree();
    }

    // We also specified this function name in PascalCase in the editor's connection window
    public void OnMobDetectorBodyEntered(Node body)
    {
        Die();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
