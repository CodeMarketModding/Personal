class FloorNavigator
{
    public System.Action BeforeAction;

    private void CheckBeforeAction()
    {
        if (BeforeAction != null) BeforeAction();
    }

    public bool Go(Interactable target)
    {
        if (!GoTo(target.Position)) return false;
        if (robot.GetFacing() != target.Facing)
        {
            CheckBeforeAction();
            if (robot.GetX() != target.Position.X || robot.GetY() != target.Position.Y) return false;
            robot.Turn(target.Facing);
        }
        return robot.GetX() == target.Position.X && robot.GetY() == target.Position.Y
            && robot.GetFacing() == target.Facing;
    }

    public bool GoTo(Vector2d position)
    {
        if (!ValidMap()) return false;
        if (!Inside(position.X, position.Y)) return false;
        int target = position.Y * PathMap.Width + position.X;
        if (!PathMap.StepsByTarget.ContainsKey(target)) return false;
        return Follow(PathMap.StepsByTarget[target], position, false);
    }

    public bool MoveAside()
    {
        if (!ValidMap()) return false;
        return Follow(PathMap.RestSteps, new Vector2d(0, 0), true);
    }

    public void WaitTurn()
    {
        CheckBeforeAction();
        robot.Turn(robot.GetFacing());
    }

    private bool ValidMap()
    {
        // An expansion keeps existing mapped routes usable until the next Remap.
        if (PathMap.World != FloorMap.World || PathMap.Width != FloorMap.Width
            || PathMap.Height != FloorMap.Height || store.GetWidth() < PathMap.Width
            || store.GetHeight() < PathMap.Height)
        {
            return false;
        }
        return true;
    }

    private bool Inside(int x, int y)
    {
        return x >= 0 && y >= 0 && x < PathMap.Width && y < PathMap.Height;
    }

    private bool Follow(string steps, Vector2d target, bool aside)
    {
        int x = robot.GetX();
        int y = robot.GetY();
        int left = PathMap.Width * PathMap.Height;
        while (left-- > 0)
        {
            if (!Inside(x, y)) return false;
            int index = y * PathMap.Width + x;
            char step = steps[index];
            if (step == 'S')
                return aside ? PathMap.Reserved[index] == '0' : x == target.X && y == target.Y;
            if (step == 'X') return false;
            int mask = step <= '9' ? step - '0' : step - 'A' + 10;
            int nx = x;
            int ny = y;
            Direction direction = Direction.Up;
            if ((mask & 1) != 0 && robot.CanMove(Direction.Up)) ny++;
            else if ((mask & 2) != 0 && robot.CanMove(Direction.Right)) { nx++; direction = Direction.Right; }
            else if ((mask & 4) != 0 && robot.CanMove(Direction.Down)) { ny--; direction = Direction.Down; }
            else if ((mask & 8) != 0 && robot.CanMove(Direction.Left)) { nx--; direction = Direction.Left; }
            else
            {
                return false;
            }
            if (!Inside(nx, ny) || !store.IsFree(nx, ny)) return false;
            CheckBeforeAction();
            if (robot.GetX() != x || robot.GetY() != y || !robot.CanMove(direction)) return false;
            // Native Move also turns to the movement direction; a separate Turn costs a full action.
            if (!robot.Move(direction)) return false;
            if (robot.GetX() != nx || robot.GetY() != ny || robot.GetFacing() != direction) return false;
            x = nx;
            y = ny;
        }
        return false;
    }
}
