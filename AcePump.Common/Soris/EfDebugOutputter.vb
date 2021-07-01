Imports System.Data.Entity
Imports System.Text
Imports System.Data.Entity.Infrastructure

Namespace Soris
    Public Class EfDebugOutputter
        Public Shared Function GetPendingChanges(context As DbContext) As List(Of EfDebugPendingChange)
            Dim pendingChanges As New List(Of EfDebugPendingChange)
            Dim tracker As DbChangeTracker = context.ChangeTracker
            For Each entry In tracker.Entries().Where(Function(x) x.State = EntityState.Modified Or x.State = EntityState.Deleted Or x.State = EntityState.Added)
                If entry.Entity IsNot Nothing Then
                    Dim entityName As String = entry.Entity.GetType().Name
                    If entityName.IndexOf("_"c) >= 0 Then
                        entityName = entityName.Substring(0, entityName.IndexOf("_"c))
                    End If
                    Dim recordIdPropertyName As String = entityName & "ID"

                    Select Case entry.State
                        Case EntityState.Modified
                            For Each propertyName As String In entry.OriginalValues.PropertyNames
                                Dim currentValue As String = Convert.ToString(entry.CurrentValues(propertyName))
                                Dim originalValue As String = Convert.ToString(entry.OriginalValues(propertyName))
                                Dim recordId As String = Convert.ToString(entry.OriginalValues(recordIdPropertyName))

                                If Not currentValue.Equals(originalValue) Then
                                    pendingChanges.Add(New EfDebugPendingChange With {
                                                            .TableName = entityName,
                                                            .ColumnName = propertyName,
                                                            .OriginalValue = originalValue,
                                                            .NewValue = currentValue,
                                                            .State = "Modified",
                                                            .RecordID = recordId
                                                        })
                                End If
                            Next

                        Case EntityState.Added
                            For Each propertyName As String In entry.CurrentValues.PropertyNames
                                Dim currentValue As String = Convert.ToString(entry.CurrentValues(propertyName))
                                Dim recordId As String = Convert.ToString(entry.CurrentValues(recordIdPropertyName))

                                pendingChanges.Add(New EfDebugPendingChange With {
                                                        .TableName = entityName,
                                                        .ColumnName = propertyName,
                                                        .OriginalValue = "-- none --",
                                                        .NewValue = currentValue,
                                                        .State = "Added",
                                                        .RecordID = recordId
                                                    })
                            Next

                        Case EntityState.Deleted
                            For Each propertyName As String In entry.OriginalValues.PropertyNames
                                Dim originalValue As String = Convert.ToString(entry.OriginalValues(propertyName))
                                Dim recordId As String = Convert.ToString(entry.OriginalValues(recordIdPropertyName))

                                pendingChanges.Add(New EfDebugPendingChange With {
                                                        .TableName = entityName,
                                                        .ColumnName = propertyName,
                                                        .OriginalValue = originalValue,
                                                        .NewValue = "-- deleted --",
                                                        .State = "Deleted",
                                                        .RecordID = recordId
                                                    })
                            Next
                    End Select
                End If
            Next

            Return pendingChanges
        End Function

        Public Shared Function GetPendingChangeText(context As DbContext) As String
            Dim pendingChanges As List(Of EfDebugPendingChange) = GetPendingChanges(context)
            Dim currentTable As String = Nothing, currentRecId As String = Nothing
            Dim stringBuilder As New StringBuilder()
            For Each change As EfDebugPendingChange In pendingChanges
                If currentTable <> change.TableName Or currentRecId <> change.RecordID Then
                    stringBuilder.AppendLine()
                    stringBuilder.AppendFormat("{0} (id {1}): {2}{3}", change.TableName, change.RecordID, change.State, vbCrLf)
                    currentTable = change.TableName
                    currentRecId = change.RecordID
                End If

                stringBuilder.AppendFormat("{0}{1}: ({2}) -> ({3}){4}", vbTab, change.ColumnName, change.OriginalValue, change.NewValue, vbCrLf)
            Next

            Return stringBuilder.ToString()
        End Function
    End Class
End Namespace