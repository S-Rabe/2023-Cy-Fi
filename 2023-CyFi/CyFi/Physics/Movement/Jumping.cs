using CyFi.Physics.Movement;
using CyFi.Physics.Utils;
using Domain.Enums;

namespace CyFi.Physics.Movement;

public class Jumping : BaseState
{
    private MovementSM movementSm;

    private int jumpHeight;
    private int maxHeight = 3;

    public Jumping(MovementSM stateMachine) : base("Jumping", stateMachine)
    {
        movementSm = stateMachine;
    }

    public override void UpdateInput(InputCommand inputCommand)
    {
        base.UpdateInput(inputCommand);
        switch (inputCommand)
        {
            case InputCommand.UP:
                break;
            case InputCommand.DOWN:
                break;
            case InputCommand.LEFT:
                movementSm.GameObject.deltaX = -1;
                break;
            case InputCommand.RIGHT:
                movementSm.GameObject.deltaX = 1;
                break;
            case InputCommand.UPLEFT:
                break;
            case InputCommand.UPRIGHT:
                break;
            case InputCommand.DOWNLEFT:
                break;
            case InputCommand.DOWNRIGHT:
                break;
            case InputCommand.DIGDOWN:
            case InputCommand.DIGLEFT:
            case InputCommand.DIGRIGHT:
            case InputCommand.STEAL:
            case InputCommand.RADAR:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(inputCommand), inputCommand, null);
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        movementSm.GameObject.deltaY = 1;

        var maxHeightReached = jumpHeight >= maxHeight;

        // Attempt to perform the original move (diagonals / upward).
        var successfulMove = Movements.AttemptMove(movementSm);

        if (successfulMove && !maxHeightReached)
        {
            Movements.UpdateHeroPositions(movementSm);
            movementSm.GameObject.deltaY = 0;
            jumpHeight++;
            return;
        }

        // Attempt to only perform upward move in case diagonals were blocked.
        movementSm.GameObject.deltaX = 0;
        successfulMove = Movements.AttemptMove(movementSm);

        if (successfulMove && !maxHeightReached)
        {            
            Movements.UpdateHeroPositions(movementSm); 
            movementSm.GameObject.deltaY = 0;
            jumpHeight++;
            return;
        }

        // Upward Move was blocked or max height achieved (start falling)
        movementSm.ChangeState(movementSm.Falling);   
        movementSm.GameObject.deltaY = 0;
        jumpHeight = 0;  
    }
}
