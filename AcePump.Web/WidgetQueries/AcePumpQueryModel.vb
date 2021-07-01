Imports Yesod.Widgets.Models

Namespace WidgetQueries
    Public Class AcePumpQueryModel
        Private Property InternalQuery As QueryModel

        Public Property StartDate As Date
            Get
                Return InternalQuery.StartDate
            End Get
            Set(value As Date)
                InternalQuery.StartDate = value
            End Set
        End Property

        Public Property EndDate As Date
            Get
                Return InternalQuery.EndDate
            End Get
            Set(value As Date)
                InternalQuery.EndDate = value
            End Set
        End Property

        Private Const CustomerAccessIDsKey As String = "CustomerID"
        Public Property CustomerAccessIDs As List(Of Integer)
            Get
                If Not AdditionalParameters.ContainsKey(CustomerAccessIDsKey) Then
                    AdditionalParameters(CustomerAccessIDsKey) = New List(Of Integer)

                ElseIf TypeOf AdditionalParameters(CustomerAccessIDsKey) Is String Then
                    Dim ids As New List(Of Integer)

                    If Not String.IsNullOrEmpty(AdditionalParameters(CustomerAccessIDsKey)) Then
                        Dim buffer As Integer

                        Dim idList As String() = AdditionalParameters(CustomerAccessIDsKey).ToString().Split(","c)
                        For Each id As String In idList
                            If Integer.TryParse(id, buffer) Then
                                ids.Add(buffer)
                            End If
                        Next
                    End If

                    AdditionalParameters(CustomerAccessIDsKey) = ids
                End If

                Return AdditionalParameters(CustomerAccessIDsKey)
            End Get
            Set(value As List(Of Integer))
                AdditionalParameters(CustomerAccessIDsKey) = value
            End Set
        End Property

        Private Const LeaseIDKey As String = "LeaseID"
        Public Property LeaseID As Integer?
            Get
                If Not AdditionalParameters.ContainsKey(LeaseIDKey) Then
                    Return Nothing

                ElseIf TypeOf AdditionalParameters(LeaseIDKey) Is String Then
                    If String.IsNullOrEmpty(AdditionalParameters(LeaseIDKey)) Then
                        AdditionalParameters(LeaseIDKey) = New Nullable(Of Integer)

                    Else
                        AdditionalParameters(LeaseIDKey) = Integer.Parse(AdditionalParameters(LeaseIDKey))
                    End If
                End If

                Return AdditionalParameters(LeaseIDKey)
            End Get
            Set(value As Integer?)
                AdditionalParameters(LeaseIDKey) = value
            End Set
        End Property

        Private Const WellIDKey As String = "WellID"
        Public Property WellID As Integer?
            Get
                If Not AdditionalParameters.ContainsKey(WellIDKey) Then
                    Return Nothing

                ElseIf TypeOf AdditionalParameters(WellIDKey) Is String Then
                    If String.IsNullOrEmpty(AdditionalParameters(WellIDKey)) Then
                        AdditionalParameters(WellIDKey) = New Nullable(Of Integer)

                    Else
                        AdditionalParameters(WellIDKey) = Integer.Parse(AdditionalParameters(WellIDKey))
                    End If
                End If

                Return AdditionalParameters(WellIDKey)
            End Get
            Set(value As Integer?)
                AdditionalParameters(WellIDKey) = value
            End Set
        End Property

        Private Const ReasonRepairedKey As String = "ReasonRepaired"
        Public Property ReasonRepaired As String
            Get
                If AdditionalParameters.ContainsKey(ReasonRepairedKey) Then
                    Return AdditionalParameters(ReasonRepairedKey)
                Else
                    Return ""
                End If
            End Get
            Set(value As String)
                AdditionalParameters(ReasonRepairedKey) = value
            End Set
        End Property

        Private Const CategoryIDKey As String = "CategoryID"
        Public Property CategoryID As Integer?
            Get
                If AdditionalParameters.ContainsKey(CategoryIDKey) Then
                    If TypeOf AdditionalParameters(CategoryIDKey) Is String Then
                        If String.IsNullOrEmpty(AdditionalParameters(CategoryIDKey)) Then
                            AdditionalParameters(CategoryIDKey) = New Nullable(Of Integer)

                        Else
                            AdditionalParameters(CategoryIDKey) = Integer.Parse(AdditionalParameters(CategoryIDKey))

                        End If
                    End If

                    Return AdditionalParameters(CategoryIDKey)
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Integer?)
                AdditionalParameters(CategoryIDKey) = value
            End Set
        End Property

        Private Const PartTemplateIDKey As String = "PartTemplateID"
        Public Property PartTemplateID As Integer?
            Get
                If AdditionalParameters.ContainsKey(CategoryIDKey) Then
                    If TypeOf AdditionalParameters(PartTemplateIDKey) Is String Then
                        If String.IsNullOrEmpty(AdditionalParameters(PartTemplateIDKey)) Then
                            AdditionalParameters(PartTemplateIDKey) = New Nullable(Of Integer)

                        Else
                            AdditionalParameters(PartTemplateIDKey) = Integer.Parse(AdditionalParameters(PartTemplateIDKey))

                        End If
                    End If

                    Return AdditionalParameters(PartTemplateIDKey)
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Integer?)
                AdditionalParameters(PartTemplateIDKey) = value
            End Set
        End Property

        Public Property AdditionalParameters As Dictionary(Of String, Object)
            Get
                Return InternalQuery.AdditionalParameters
            End Get
            Set(value As Dictionary(Of String, Object))
                InternalQuery.AdditionalParameters = value
            End Set
        End Property

        Public Sub New(fromQuery As QueryModel)
            InternalQuery = fromQuery
        End Sub
    End Class
End Namespace