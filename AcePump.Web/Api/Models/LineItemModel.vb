Namespace Api.Models
    Public Class LineItemModel
        Public Property LineItemID As Integer

        Public Property PartTemplateID As Integer?
        Private Property PartTemplateNumberInternal As String
        Public Property PartTemplateNumber As String
            Get
                Return PartTemplateNumberInternal.Trim
            End Get
            Set(value As String)
                PartTemplateNumberInternal = value
            End Set
        End Property
        Public Property PartTemplateQuickbooksID As String

        Public Property Quantity As Decimal
        Public Property Description As String
        Public Property UnitPrice As Decimal
        Public Property LineIsTaxable As Boolean
    End Class
End Namespace
