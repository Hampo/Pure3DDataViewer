using NetP3DLib.P3D;
using System.Text;

namespace Pure3DDataViewer;
public static class Extensions
{
    public static string GetPathText(this TreeNode node)
    {
        if (node.Tag is not Chunk chunk)
            return node.Text;

        var sb = new StringBuilder($"{chunk}");

        var parent = node.Parent;
        while (parent?.Tag is Chunk parentChunk)
        {
            sb.Append($"|{parentChunk}");
            parent = parent.Parent;
        }

        return sb.ToString();
    }
}
