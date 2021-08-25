using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPSourceGen
{
    [Generator]
    class BPGenerator : ISourceGenerator
    {
        const string BpAttribute = @"
using System;

namespace BPSourceGen
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    sealed class BPCreationAttribute : Attribute
    {
        public string PropertyName { get; set; }
        
        public Type ReturnType { get; set; }
        
        public Type OwnerType { get; set; }

        public string DefaultValue{ get; set; }

        public string PropertyChangedMethodName { get; set; }

        public BPCreationAttribute()
        {
        }
    }
}";
        const string BpAttributeName = "BPSourceGen.BPCreationAttribute";
        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not BPSyntaxReceiver receiver)
                return;

            var options = (context.Compilation as CSharpCompilation).SyntaxTrees[0].Options as CSharpParseOptions;
            var compilation = context.Compilation;

            var attributeSymbol = compilation.GetTypeByMetadataName(BpAttributeName);

            var classes = GetClassesCandidates(receiver, compilation, attributeSymbol).ToList();

            foreach (var item in classes)
            {
                var classSource = ProcessClass(context, attributeSymbol, item);
                FormatText(ref classSource, options);
                context.AddSource($"{item.classDeclaration.Identifier.Value}.g.cs", classSource);
            }

            static void FormatText(ref string classSource, CSharpParseOptions options)
            {
                var mysource = CSharpSyntaxTree.ParseText(SourceText.From(classSource, Encoding.UTF8), options);
                var formattedRoot = (CSharpSyntaxNode)mysource.GetRoot().NormalizeWhitespace();
                classSource = CSharpSyntaxTree.Create(formattedRoot).ToString();
            }
        }

        string ProcessClass(GeneratorExecutionContext context, INamedTypeSymbol attributeSymbol, (ClassDeclarationSyntax classDeclaration, ITypeSymbol classSymbol) classCandidate)
        {
            var (classDeclaration, classSymbol) = classCandidate;
            var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            var sb = new StringBuilder(@$"
using Xamarin.Forms;
using System;

namespace {namespaceName}
{{
    partial class {classSymbol.Name}
    {{
");
            ProcessAttribute(classDeclaration, classSymbol, attributeSymbol, sb);
            sb.AppendLine("     }");
            sb.AppendLine("   }");
            return sb.ToString();
        }

        void ProcessAttribute(ClassDeclarationSyntax classDeclaration, ITypeSymbol classSymbol, INamedTypeSymbol attributeSymbol, StringBuilder sb)
        {
            var attributeData = classSymbol.GetAttributes().Where(x => x.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));

            foreach (var attribute in attributeData)
            {
                var propName = attribute.GetAttributeValueByName("PropertyName").Value;

                //public static readonly BindableProperty TextProperty = 
                sb.Append($"public static readonly BindableProperty {propName}Property = ");

                var returnType = attribute.GetAttributeValueByName("ReturnType").Value;
                var ownerType = attribute.GetAttributeValueByName("OwnerType").Value;
                var defaultValue = attribute.GetAttributeValueByName("DefaultValue");
                string defaultText = defaultValue.Value is null ? $"default({returnType})" : $"\"{defaultValue.Value}\"";

                var propertyChangedDelegateName = attribute.GetAttributeValueByName("PropertyChangedMethodName").Value;

                //BindableProperty.Create(nameof(Text), typeof(string), typeof(MyCustomView), default, propertyChanged: Invalidate);
                sb.Append($"BindableProperty.Create(\"{propName}\", typeof({returnType}), typeof({ownerType}), {defaultText}, propertyChanged: {propertyChangedDelegateName});");
                sb.AppendLine(Environment.NewLine);

                /*
                 * 
                 * public string Text
                   {
                       get => (string)GetValue(TextProperty);
                       set => SetValue(TextProperty, value);
                   }
                 */

                sb.AppendLine(@$"public {returnType} {propName}
                 {{")
                    .AppendLine($@"      get => ({returnType})GetValue({propName}Property);")
                    .AppendLine($@"      set => SetValue({propName}Property, value);")
                    .AppendLine("   }");
            }

            var z = sb.ToString();
        }

        //TODO: Criar um tipo/model pra isso
        IEnumerable<(ClassDeclarationSyntax classDeclaration, ITypeSymbol classSymbol)> GetClassesCandidates(BPSyntaxReceiver receiver, Compilation compilation, INamedTypeSymbol attributeSymbol)
        {
            foreach (var classDeclaration in receiver.ClassesCandidates)
            {
                var model = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
                var classSymbol = model.GetDeclaredSymbol(classDeclaration) as ITypeSymbol;

                if (classSymbol.GetAttributes().Any(a => a.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default)))
                    yield return (classDeclaration, classSymbol);
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization(i => i.AddSource("BPCreationAttribute.g.cs", SourceText.From(BpAttribute, Encoding.UTF8)));
            context.RegisterForSyntaxNotifications(() => new BPSyntaxReceiver());
        }
    }
}
