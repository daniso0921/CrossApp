namespace Core.Events;

public sealed class CatalogChangedEventArgs(
    string id,
    CatalogChangeKind kind,
    int delta,
    bool isIssuedAfter) : EventArgs
{
    public string Id { get; } = id;
    public CatalogChangeKind Kind { get; } = kind;
    public int Delta { get; } = delta;
    public bool IsIssuedAfter { get; } = isIssuedAfter;
    public DateTimeOffset At { get; } = DateTimeOffset.Now;
}