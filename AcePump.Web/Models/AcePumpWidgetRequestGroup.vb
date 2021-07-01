Imports Yesod.Widgets.Models

Namespace Models
    Public Class AcePumpWidgetRequestGroup
        Inherits WidgetRequestGroup

        Private Const CustomerIDsKey As String = "CustomerID"
        Public Property CustomerIDs As List(Of Integer)
            Get
                If Not GlobalFilters.ContainsKey(CustomerIDsKey) Then
                    GlobalFilters(CustomerIDsKey) = New List(Of Integer)

                ElseIf TypeOf GlobalFilters(CustomerIDsKey) Is String Then
                    Dim ids As New List(Of Integer)

                    If Not String.IsNullOrEmpty(GlobalFilters(CustomerIDsKey)) Then
                        Dim buffer As Integer

                        Dim idList As String() = GlobalFilters(CustomerIDsKey).ToString().Split(","c)
                        For Each id As String In idList
                            If Integer.TryParse(id, buffer) Then
                                ids.Add(buffer)
                            End If
                        Next
                    End If

                    GlobalFilters(CustomerIDsKey) = ids
                End If

                Return GlobalFilters(CustomerIDsKey)
            End Get
            Set(value As List(Of Integer))
                GlobalFilters(CustomerIDsKey) = value
            End Set
        End Property

        Private Const LeaseIDKey As String = "LeaseID"
        Public Property LeaseID As Integer?
            Get
                If Not GlobalFilters.ContainsKey(LeaseIDKey) Then
                    Return Nothing

                ElseIf TypeOf GlobalFilters(LeaseIDKey) Is String Then
                    If String.IsNullOrEmpty(GlobalFilters(LeaseIDKey)) Then
                        GlobalFilters(LeaseIDKey) = New Nullable(Of Integer)

                    Else
                        GlobalFilters(LeaseIDKey) = Integer.Parse(GlobalFilters(LeaseIDKey))
                    End If
                End If

                Return GlobalFilters(LeaseIDKey)
            End Get
            Set(value As Integer?)
                GlobalFilters(LeaseIDKey) = value
            End Set
        End Property

        Private Const WellIDKey As String = "WellID"
        Public Property WellID As Integer?
            Get
                If Not GlobalFilters.ContainsKey(WellIDKey) Then
                    Return Nothing

                ElseIf TypeOf GlobalFilters(WellIDKey) Is String Then
                    If String.IsNullOrEmpty(GlobalFilters(WellIDKey)) Then
                        GlobalFilters(WellIDKey) = New Nullable(Of Integer)

                    Else
                        GlobalFilters(WellIDKey) = Integer.Parse(GlobalFilters(WellIDKey))
                    End If
                End If

                Return GlobalFilters(WellIDKey)
            End Get
            Set(value As Integer?)
                GlobalFilters(WellIDKey) = value
            End Set
        End Property

        Private Const ReasonRepairedKey As String = "ReasonRepaired"
        Public Property ReasonRepaired As String
            Get
                If GlobalFilters.ContainsKey(ReasonRepairedKey) Then
                    Return GlobalFilters(ReasonRepairedKey)
                Else
                    Return ""
                End If
            End Get
            Set(value As String)
                GlobalFilters(ReasonRepairedKey) = value
            End Set
        End Property

        Private Const CategoryIDKey As String = "CategoryID"
        Public Property CategoryID As Integer?
            Get
                If GlobalFilters.ContainsKey(CategoryIDKey) Then
                    If TypeOf GlobalFilters(CategoryIDKey) Is String Then
                        If String.IsNullOrEmpty(GlobalFilters(CategoryIDKey)) Then
                            GlobalFilters(CategoryIDKey) = New Nullable(Of Integer)

                        Else
                            GlobalFilters(CategoryIDKey) = Integer.Parse(GlobalFilters(CategoryIDKey))

                        End If
                    End If

                    Return GlobalFilters(CategoryIDKey)
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Integer?)
                GlobalFilters(CategoryIDKey) = value
            End Set
        End Property

        Private Const PartTemplateIDKey As String = "PartTemplateID"
        Public Property PartTemplateID As Integer?
            Get
                If GlobalFilters.ContainsKey(CategoryIDKey) Then
                    If TypeOf GlobalFilters(PartTemplateIDKey) Is String Then
                        If String.IsNullOrEmpty(GlobalFilters(PartTemplateIDKey)) Then
                            GlobalFilters(PartTemplateIDKey) = New Nullable(Of Integer)

                        Else
                            GlobalFilters(PartTemplateIDKey) = Integer.Parse(GlobalFilters(PartTemplateIDKey))

                        End If
                    End If

                    Return GlobalFilters(PartTemplateIDKey)
                Else
                    Return Nothing
                End If
            End Get
            Set(value As Integer?)
                GlobalFilters(PartTemplateIDKey) = value
            End Set
        End Property
    End Class
End Namespace
