using Godot;
using System;

public class Player : KinematicBody
{
    // How fast the player moves in meters per second.
    [Export]
    public int speed = 14;
    // The downward acceleration when in the air, in meters per second squared.
    [Export]
    public int FallAcceleration = 75;

    // Vertical impulse applied to the character upon jumping in meters per second.
    [Export]
    public int JumpImpulse = 20;

    // Vertical impulse applied to the character upon bouncing over a mob in meters per second.
    [Export]
    public int BounceImpulse = 16;

    // Emitted when the player was hit by a mob.
    [Signal]
    public delegate void Hit();


    private Vector3 _velocity = Vector3.Zero;

    public override void _PhysicsProcess(float delta)
    {
        // We create a local variable to store the input direction.
        var direction = Vector3.Zero;

        // We check for each move input and update the direction accordingly

        if (Input.IsActionPressed("move_right"))
        {
            direction.x += 1f;
        }

        if (Input.IsActionPressed("move_left"))
        {
            direction.x -= 1f;
        }

        if (Input.IsActionPressed ("move_back"))
        {
            // Notice how we are working with the vector's x and z axes.
            // In 3D, the XZ plane is the ground plane.
            direction.z += 1f;
        }

        if (Input.IsActionPressed("move_forward"))
        {
            direction.z -= 1f;
        }

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            GetNode<Spatial>("Pivot").LookAt(Translation + direction, Vector3.Up);
        }

        if (direction != Vector3.Zero)
        {
            // ...
            GetNode<AnimationPlayer>("AnimationPlayer").PlaybackSpeed = 4;
        }
        else
        {
            GetNode<AnimationPlayer>("AnimationPlayer").PlaybackSpeed = 1;
        }
    

        // Ground velocity
        _velocity.x = direction.x * speed;
        _velocity.z = direction.z * speed;
        // Vertical velocity
        _velocity.y -= FallAcceleration * delta;
        // Jumping.
        if (IsOnFloor() && Input.IsActionPressed("jump"))
        {
            _velocity.y += JumpImpulse;
        }
        // Moving the character
        _velocity = MoveAndSlide(_velocity, Vector3.Up);

        for (int index = 0; index < GetSlideCount(); index++)
        {
            // We check every collision that occurred this frame.
            KinematicCollision collision = GetSlideCollision(index);
            // If we collide with a monster...
            if (collision.Collider is Mob mob && mob.IsInGroup("mob"))
            {
                // ...we check that we are hitting it from above.
                if (Vector3.Up.Dot(collision.Normal) > 0.1f)
                {
                    // If so, we squash it and bounce.
                    mob.Squash();
                    _velocity.y = BounceImpulse;
                }
            }
        }

        var pivot = GetNode<Spatial>("Pivot");
        pivot.Rotation = new Vector3(Mathf.Pi / 6f * _velocity.y / JumpImpulse, pivot.Rotation.y, pivot.Rotation.z);
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
