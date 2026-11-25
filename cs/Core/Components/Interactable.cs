struct Interactable
{
    public Vector2d Position;
    public Direction Facing;
    public Role Role;
    public int Index;
    public InteractableType Type;

    public Interactable(Vector2d position, Direction facing, Role role = default(Role), int index = 0, InteractableType type = InteractableType.SalesFloor)
    {
        Position = position;
        Facing = facing;
        Role = role;
        Index = index;
        Type = type;
    }
}
