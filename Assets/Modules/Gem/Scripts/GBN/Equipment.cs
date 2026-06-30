using BlacksmithModule;

public interface Equipment : IForgeable
{
    string Name { get; }

    void UpdateStat();
}