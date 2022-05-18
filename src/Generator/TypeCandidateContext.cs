using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Linq;
using System.Text;
using System.Threading;

namespace Generator
{
    internal sealed class TypeCandidateContext
    {
        public static bool CanCreate(SyntaxNode node)
        {
            if (node is TypeDeclarationSyntax ds && ds.AttributeLists.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static TypeCandidateContext? Create(in GeneratorSyntaxContext context)
        {
            var node = (TypeDeclarationSyntax)context.Node;
            if (node.AttributeLists.Any(i => i.Attributes.Any(j => j.Name.ToString().Contains("Generate"))))
            {
                return new TypeCandidateContext(node, context.SemanticModel);
            }
            return null;
        }

        private static int counter;

        public TypeCandidateContext(TypeDeclarationSyntax syntax, SemanticModel semanticModel)
        {
            Syntax = syntax;
            SemanticModel = semanticModel;
            Index = Interlocked.Increment(ref counter);
        }

        public TypeDeclarationSyntax Syntax { get; }

        public SemanticModel SemanticModel { get; }

        public int Index { get; }

        public void Generate(in SourceProductionContext ctx)
        {
            string code = $"// genereted code for {Syntax.Identifier.Text}";

            var source = SourceText.From(code, Encoding.UTF8);
            string hintName = $"{Syntax.Identifier.Text}.{Index}.g";
            ctx.AddSource(hintName, source);
        }
    }
}
