
namespace Models
{
    public interface IParserService
    {
        bool Compute(List<bool> values);
        List<string> Decompose(string expr);
    }
}