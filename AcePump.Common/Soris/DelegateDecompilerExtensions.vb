Imports System.Linq.Expressions
Imports System.Runtime.CompilerServices
Imports DelegateDecompiler

Namespace Soris
    Public Module DelegateDecompilerExtensions
        <Extension()>
        Public Function Decompile(Of T)(expression As Expression(Of T)) As Expression(Of T)
            Return DecompileExpressionVisitor.Decompile(expression)
        End Function
    End Module
End Namespace
