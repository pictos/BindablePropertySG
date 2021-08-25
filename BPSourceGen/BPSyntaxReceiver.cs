using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace BPSourceGen
{
    sealed class BPSyntaxReceiver : ISyntaxReceiver
    {
        public HashSet<ClassDeclarationSyntax> ClassesCandidates = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax
                && classDeclarationSyntax.AttributeLists.Count > 0)
                ClassesCandidates.Add(classDeclarationSyntax);
        }
    }
}
