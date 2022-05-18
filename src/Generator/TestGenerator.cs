using Microsoft.CodeAnalysis;
using System.Linq;

namespace Generator
{
    [Generator(LanguageNames.CSharp)]
    public sealed partial class TestGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            IncrementalValuesProvider<TypeCandidateContext?> typeCandidateContexts = context.SyntaxProvider.CreateSyntaxProvider(
                static (node, _) => TypeCandidateContext.CanCreate(node),
                static (context, _) => TypeCandidateContext.Create(in context)
                )
                .Where(i => i is not null);

            context.RegisterSourceOutput(typeCandidateContexts, (ctx, node) =>
            {
                node?.Generate(ctx);
            });
        }
    }
}
