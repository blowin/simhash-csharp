namespace SimhashLib.Abstraction
{
    public interface IHashResult<T>
        where T : IHashResult<T>
    {
        T BitwiseAnd(long mask);
        bool GreatThanZero { get; }
    }
}