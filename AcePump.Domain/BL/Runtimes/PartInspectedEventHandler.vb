Imports AcePump.Domain.Models
Imports AcePump.Domain.DataSource

Namespace BL.Runtimes
    <HandlesEventModel(GetType(PartInspectionEventModel))> _
    Friend Class PartInspectedEventHandler
        Implements IRuntimeEventHandler

        Private Property Model As PartInspectionEventModel
        Private Property DataSource As AcePumpContext
        Private Property QueryHelper As RuntimeQueryHelper
        
        Public Sub New(model As PartInspectionEventModel)
            Me.Model = model
        End Sub

        Public Sub UpdateDataSource(dataSource As AcePumpContext) Implements IRuntimeEventHandler.UpdateDataSource
            Me.DataSource = dataSource
            Me.QueryHelper = New RuntimeQueryHelper(dataSource)

            Dim resultChanged As Boolean = (Model.OldResult <> Model.NewResult) _
                                           OrElse Not Model.OldDate.Equals(Model.NewDate)
            If resultChanged Then
                If ResultEndsRuntime(Model.OldResult) Then
                    RemoveOldFailEvent()
                End If

                If ResultEndsRuntime(Model.NewResult) Then
                    StoreNewEvent()
                End If
            End If

            Me.DataSource = Nothing
            Me.QueryHelper = Nothing
        End Sub

        Private Function ResultEndsRuntime(result As String) As Boolean
            result = result.Trim()

            Return result = "Convert" Or result = "Replace"
        End Function

        Private Sub RemoveOldFailEvent()
            Dim partRuntime As IRuntimeManager = QueryHelper.ManageRuntimeEventOccuredIn(Model.PumpID, Model.TemplatePartDefID, Model.NewDate)
            If partRuntime.Exists() Then
                partRuntime.RemoveEndDate()
            End If
        End Sub

        Private Sub StoreNewEvent()
            Dim partRuntime As IRuntimeManager = QueryHelper.ManageRuntimeEventOccuredIn(Model.PumpID, Model.TemplatePartDefID, Model.NewDate)
            If ResultEndsRuntime(Model.NewResult) Then
                PartRuntime.SetEndDate(Model.NewDate, Model.PartInspectionID)
            Else
                partRuntime.RemoveEndDate()
            End If
        End Sub
    End Class
End Namespace