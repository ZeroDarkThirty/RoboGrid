using System.Net;

var currentLocation = new LocationAxes();
var currentDirection = Direction.NORTH;
var currentOrientation = Orientations.NOTSET;

ReadFile();

void ReadFile()
{
    try
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string rootDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(currentDirectory).FullName).FullName).FullName;
        string filePath = Path.Combine(rootDirectory, "commands.txt");
        string[] commands = File.ReadAllLines(filePath);
        
        int counter = 0;
        foreach (var command in commands)
        {
            if (counter == 0 && !command.ToUpper().StartsWith("P"))
            {
                throw new Exception("The first command must be a PLACE command");
            }
            
            if (command.ToUpper().StartsWith("P"))
            {
                // set the location
                string[] parts = commands[counter].Split(",");
                int xAxis = Int32.Parse(parts[0].Split(" ")[1]);
                int yAxis = Int32.Parse(parts[1]);
                var direction = (Direction) Enum.Parse(typeof(Direction), parts[2]);
                
                if (xAxis > 4 || yAxis > 4)
                {
                    throw new Exception(
                        "Invalid grid values. Make sure the grid dimension does not exceed 4x4, starting with position 0x0.");
                }
                
                SetCurrentLocationInGrid(xAxis, yAxis, direction);
                counter++;
            }
            else if (command.ToUpper().Contains("LEFT") || command.ToUpper().StartsWith("RIGHT"))
            {
                // set orientation
                currentOrientation = (Orientations) Enum.Parse(typeof(Orientations), command);
                SetCurrentDirectionInGrid(currentDirection, currentOrientation);
            }
            else if (command.ToUpper().StartsWith("M"))
            {
                // move the robot
                MoveRobot(currentLocation, currentDirection, currentOrientation);
            }
            else if (command.ToUpper().Contains("REPORT"))
            {
                Console.WriteLine($"OUTPUT {currentLocation.X}, {currentLocation.Y}, {currentDirection}");
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}

void SetCurrentLocationInGrid(int x, int y, Direction direction)
{
    currentLocation.X = x;
    currentLocation.Y = y;
    currentDirection = direction;
}

void SetCurrentDirectionInGrid(Direction direction, Orientations orientation)
{
    switch (direction)
    {
        case Direction.NORTH when orientation == Orientations.LEFT:
            currentDirection = Direction.WEST;
            break;
        case Direction.NORTH when orientation == Orientations.RIGHT:
            currentDirection = Direction.EAST;
            break;
        case Direction.WEST when orientation == Orientations.LEFT:
            currentDirection = Direction.SOUTH;
            break;
        case Direction.WEST when orientation == Orientations.RIGHT:
            currentDirection = Direction.NORTH;
            break;
        case Direction.EAST when orientation == Orientations.LEFT:
            currentDirection = Direction.NORTH;
            break;
        case Direction.EAST when orientation == Orientations.RIGHT:
            currentDirection = Direction.SOUTH;
            break;
        case Direction.SOUTH when orientation == Orientations.LEFT:
            currentDirection = Direction.WEST;
            break;
        case Direction.SOUTH when orientation == Orientations.RIGHT:
            currentDirection = Direction.EAST;
            break;
    }
};

void MoveRobot(LocationAxes axes, Direction direction, Orientations orientation)
{
    if (axes.X > 5 || axes.Y > 5)
    {
        Console.WriteLine("Invalid move");
        throw new Exception("Invalid move, cannot step outside the grid.");
    }

    switch (direction)
    {
        case Direction.WEST when axes.X - 1 < 0:
            Console.WriteLine("Invalid move, unable to move any further left on the X axis");
            break;
        case Direction.EAST when axes.X + 1 > 4:
            Console.WriteLine("Invalid move, unable to move any further right on the X axis");
            break;
        case Direction.WEST when axes.X - 1 < 0:
            Console.WriteLine("Invalid move, unable to move any further left on the X axis");
            break;
        case Direction.SOUTH when axes.Y - 1 < 0:
            Console.WriteLine("Invalid move, unable to move any further up on the Y axis");
            break;
        case Direction.NORTH when axes.Y + 1 > 4:
            Console.WriteLine("Invalid move, unable to move any further up on the Y axis");
            break;
        default:
            switch (direction)
            {
                case Direction.WEST:
                    currentLocation.X -= 1;
                    break;
                case Direction.EAST:
                    currentLocation.X += 1;
                    break;
                case Direction.NORTH:
                    currentLocation.Y += 1;
                    break;
                case Direction.SOUTH:
                    currentLocation.Y -= 1;
                    break;
            }

            break;
    }
}