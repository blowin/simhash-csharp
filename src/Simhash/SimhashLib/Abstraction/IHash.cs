namespace SimhashLib.Abstraction
{
    public interface IHash<out TRes>
        where TRes : IHashResult<TRes>
    {
        TRes ComputeHash(string feature);
    }
}