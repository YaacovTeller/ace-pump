Namespace BL.Runtimes
    <AttributeUsage(AttributeTargets.Class, allowMultiple:=False, Inherited:=False)> _
    Public Class HandlesEventModelAttribute
        Inherits Attribute

        Public Property TypeHandled As Type

        Public Sub New(typeHandled As Type)
            If Not GetType(IEventModel).IsAssignableFrom(typeHandled) Then
                Throw New ArgumentException(String.Format("{0} is not a valid {1}.", typeHandled.Name, GetType(IEventModel).Name))
            End If

            Me.TypeHandled = typeHandled
        End Sub
    End Class
End Namespace