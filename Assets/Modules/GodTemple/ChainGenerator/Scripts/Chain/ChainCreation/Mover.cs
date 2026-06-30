namespace Chain
{
    public interface Mover
    {
        float MachinerySpeed { get; set; }
        int   MachineryId    { get; set; }

        ChainEnums.ChainDirection MachineryDirection { get; set; }

        void StartMotion();

        void StopMotion();

        void MachinerySetup(float                     machinerySpeed, int machineryId, IMachinePartData data,
                            ChainEnums.ChainDirection direction)
        {
        }
    }
}