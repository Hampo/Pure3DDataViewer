using System.Text;

namespace ConvertToLua.Extensions;

internal static class StringBuilderExtensions
{
    internal static StringBuilder AddIndent(this StringBuilder sb, int indent) => sb.Append(new string('\t', indent));
}
