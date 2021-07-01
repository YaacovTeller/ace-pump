Namespace BL.Runtimes
    Friend Class PartInspectionEventModel
        Implements IEventModel

        Public Property PartInspectionID As Integer
        Public Property PumpID As Integer
        Public Property TemplatePartDefID As Integer

        Public Property OldResult As String
        Public Property OldDate As Date?

        Public Property NewResult As String
        Public Property NewDate As Date?
    End Class
End Namespace