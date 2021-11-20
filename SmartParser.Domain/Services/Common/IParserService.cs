
namespace SmartParser.Domain.Services.Common
{
    public interface IParserService
    {
        bool Compute(List<bool> values);
        List<string> Decompose(string expr);
    }
}