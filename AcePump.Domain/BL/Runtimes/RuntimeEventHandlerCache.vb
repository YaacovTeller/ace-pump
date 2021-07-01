Imports System.Reflection
Imports System.Linq.Expressions

Namespace BL.Runtimes
    Friend Class RuntimeEventHandlerCache
        Private Shared _Loader As New Lazy(Of RuntimeEventHandlerCache)(Function() New RuntimeEventHandlerCache())
        Public Shared ReadOnly Property Instance As RuntimeEventHandlerCache
            Get
                Return _Loader.Value
            End Get
        End Property

        Private Delegate Function RuntimeEventHandlerConstructor([event] As Object) As Object
        Private Property HandlerConstructors As Dictionary(Of Type, List(Of RuntimeEventHandlerConstructor))

        Private Property RuntimeEventHandlerType As Type = GetType(IRuntimeEventHandler)

        Private Sub New()
            InitializeHandlers()
        End Sub

        Public Function GetEventHandlers(Of TEvent)([event] As TEvent) As IEnumerable(Of IRuntimeEventHandler)
            Return New CacheEnumerable(Of TEvent)([event])
        End Function

        Private Sub InitializeHandlers()
            HandlerConstructors = New Dictionary(Of Type, List(Of RuntimeEventHandlerConstructor))

            Dim asm As Assembly = Assembly.GetAssembly(GetType(RuntimeEventHandlerCache))
            Dim allTypes() As Type = asm.GetTypes()
            For Each t As Type In allTypes
                If RuntimeEventHandlerType.IsAssignableFrom(t) And RuntimeEventHandlerType IsNot t Then
                    CacheAllCtorsForHandlerType(t)
                End If
            Next
        End Sub

        Private Sub CacheAllCtorsForHandlerType(handlerType As Type)
            Dim handledTypesAttributes As Object() = handlerType.GetCustomAttributes(GetType(HandlesEventModelAttribute), False)
            If handledTypesAttributes.Length = 0 Then
                Throw New InvalidOperationException(String.Format("'{0}' implements '{1}', but it does not specify any {2}.",
                                                                  handlerType.Name,
                                                                  RuntimeEventHandlerType.Name,
                                                                  GetType(HandlesEventModelAttribute).Name)
                                                    )

            Else
                For Each handledTypesAttribute As HandlesEventModelAttribute In handledTypesAttributes
                    CacheCtor(handledTypesAttribute.TypeHandled, handlerType)
                Next
            End If
        End Sub

        Private Sub CacheCtor(typeHandled As Type, handlerType As Type)
            If Not HandlerConstructors.ContainsKey(typeHandled) Then
                HandlerConstructors.Add(typeHandled, New List(Of RuntimeEventHandlerConstructor))
            End If

            Dim ctor As RuntimeEventHandlerConstructor = BuildCtor(typeHandled, handlerType)
            HandlerConstructors(typeHandled).Add(ctor)
        End Sub

        Private Function BuildCtor(typeHandled As Type, handlerType As Type) As RuntimeEventHandlerConstructor
            Dim handlerCtor As ConstructorInfo = FindCtor(typeHandled, handlerType)
            Dim untypedEvent As ParameterExpression = Expression.Parameter(GetType(Object), "event")
            Dim stronglyTypeEvent As Expression = Expression.Convert(untypedEvent, typeHandled)
            Dim newTypedEventHandler As NewExpression = Expression.[New](handlerCtor, {stronglyTypeEvent})
            Dim untypedHandler As Expression = Expression.Convert(newTypedEventHandler, GetType(Object))
            Dim lambda As LambdaExpression = Expression.Lambda(untypedHandler, untypedEvent)

            Dim compiled As Func(Of Object, Object) = lambda.Compile()
            Dim [delegate] As New RuntimeEventHandlerConstructor(AddressOf compiled.Invoke)

            Return [delegate]
        End Function

        Private Function FindCtor(typeHandled As Type, handlerType As Type)
            Dim ctor As ConstructorInfo = handlerType.GetConstructor({typeHandled})
            If ctor Is Nothing Then
                Throw New InvalidOperationException(String.Format("No valid constructor found for '{0}', must take one argument of type '{1}'",
                                                                  handlerType.Name,
                                                                  RuntimeEventHandlerType.Name)
                                                    )

            Else
                Return ctor
            End If
        End Function

        Private Class CacheEnumerable(Of TEvent)
            Implements IEnumerable(Of IRuntimeEventHandler)

            Private Property [Event] As Object

            Public Sub New([event] As Object)
                Me.Event = [event]
            End Sub

            Public Function GetEnumerator() As IEnumerator(Of IRuntimeEventHandler) Implements IEnumerable(Of IRuntimeEventHandler).GetEnumerator
                Return New CacheEnumerator(Of TEvent)([Event])
            End Function

            Public Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
                Return GetEnumerator()
            End Function
        End Class

        Private Class CacheEnumerator(Of TEvent)
            Implements IEnumerator(Of IRuntimeEventHandler)

            Private Property IndexInCache As Integer
            Private Property Cache As List(Of RuntimeEventHandlerConstructor)
            Private Property [Event] As Object

            Private _Current As IRuntimeEventHandler
            Public ReadOnly Property Current As IRuntimeEventHandler Implements IEnumerator(Of IRuntimeEventHandler).Current
                Get
                    If _Current Is Nothing Then
                        If IndexInCache < 0 Or IndexInCache >= Cache.Count Then
                            Throw New InvalidOperationException("EOF or BOF")
                        End If

                        Dim ctor As RuntimeEventHandlerConstructor = Cache(IndexInCache)

                        _Current = ctor([Event])
                    End If

                    Return _Current
                End Get
            End Property

            Public ReadOnly Property Current1 As Object Implements IEnumerator.Current
                Get
                    Return Current
                End Get
            End Property

            Public Sub New([event] As Object)
                Me.Event = [event]
                Cache = Instance.HandlerConstructors(GetType(TEvent))
                IndexInCache = -1
            End Sub

            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                _Current = Nothing
                IndexInCache += 1

                Return IndexInCache < Cache.Count
            End Function

            Public Sub Reset() Implements IEnumerator.Reset
                IndexInCache = -1
                _Current = Nothing
            End Sub

            Public Sub Dispose() Implements IDisposable.Dispose
            End Sub
        End Class
    End Class
End Namespace