using UnityEngine;

public class CommandSource : MonoBehaviour
{
    [SerializeField] private Recorder recorder;
    private bool inputEnabled = true;
    public void ExecuteCommand(Command cmd)
    {
        switch (cmd.Type)
        {
            case "Move":
                var movePayload = cmd.GetPayload<MovePayload>();
                if (movePayload != null)
                    EventBus.Trigger("OnMoveCommand", movePayload);
                break;
            case "Jump":
                EventBus.Trigger("OnJumpCommand");
                break;
            case "Attack":
                var attackPayload = cmd.GetPayload<AttackPayload>();
                EventBus.Trigger("OnAttackCommand", attackPayload);
                break;
        }
    }

    public void SendMoveCommand(Vector3 direction)
    {
        if (!inputEnabled) return;
    
        var payload = new MovePayload { x = direction.x, y = direction.y, z = direction.z };
        recorder?.RecordCommand("Move", payload);
        EventBus.Trigger("OnMoveCommand", payload);
    }

    public void SendJumpCommand()
    {
        if (!inputEnabled) return;

        recorder?.RecordCommand("Jump", new JumpPayload { Force = 10f });
        EventBus.Trigger("OnJumpCommand");
    }

    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;
    }
}