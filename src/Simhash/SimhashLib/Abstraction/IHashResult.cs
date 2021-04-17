namespace SimhashLib.Abstraction
{
    public interface IHashResult<T>
        where T : IHashResult<T>
    {
        T BitwiseAnd(ulong mask);
        bool GreatThanZero { get; }
    }
}