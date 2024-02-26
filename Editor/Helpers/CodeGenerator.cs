using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace H2V.ExtensionsCore.Editor.Helpers
{
    /// <summary>
    /// Helper class for generating code.
    /// </summary>
    public static class CodeGenerator
    {
        private const int _spacesPerIndentLevel = 4;

        public delegate string GenerateCodeDelegate(string savePath);

        public struct Writer
        {
            public StringBuilder Buffer;
            public int IndentLevel;

            public void BeginBlock()
            {
                WriteIndent();
                Buffer.Append("{\n");
                ++IndentLevel;
            }

            public void EndBlock()
            {
                --IndentLevel;
                WriteIndent();
                Buffer.Append("}\n");
            }

            public void WriteLine()
            {
                Buffer.Append('\n');
            }

            public void WriteLine(string text)
            {
                if (!text.All(char.IsWhiteSpace))
                {
                    WriteIndent();
                    Buffer.Append(text);
                }
                Buffer.Append('\n');
            }

            public void Write(string text)
            {
                Buffer.Append(text);
            }

            public void WriteIndent()
            {
                for (var i = 0; i < IndentLevel; ++i)
                {
                    for (var n = 0; n < _spacesPerIndentLevel; ++n)
                        Buffer.Append(' ');
                }
            }
        }

        public static bool GenerateCode(string savePath, GenerateCodeDelegate generateCodeFunc)
        {
            if (!Path.HasExtension(savePath))
                savePath += ".cs";

            var code = generateCodeFunc(savePath);
            if (File.Exists(savePath))
            {
                var existingCode = File.ReadAllText(savePath);
                if (existingCode == code || existingCode.Replace(" ", string.Empty) == code.Replace(" ", string.Empty))
                    return false;
            }

            AssetDatabase.MakeEditable(savePath);
            File.WriteAllText(savePath, code);
            return true;
        }
    }
}