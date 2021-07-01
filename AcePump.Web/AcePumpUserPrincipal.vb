Imports System.Security.Principal
Imports AcePump.Domain.Models
Imports AcePump.Domain.DataSource
Imports System.Data.Entity
Imports Yesod.LinqProviders
Imports AcePump.Domain

Public Class AcePumpUserPrincipal
    Implements IPrincipal

    Private ReadOnly _InternalPrincipal As IPrincipal
    Public ReadOnly Property Identity As IIdentity Implements IPrincipal.Identity
        Get
            Return _InternalPrincipal.Identity
        End Get
    End Property

    Private _Profile As AcePumpProfile
    Public ReadOnly Property Profile As AcePumpProfile
        Get
            Return _Profile
        End Get
    End Property

    Private _User As AccountDataStoreUser
    Public ReadOnly Property User As AccountDataStoreUser
        Get
            Return _User
        End Get
    End Property

    Public Function IsInRole(role As String) As Boolean Implements IPrincipal.IsInRole
        Return User.Roles.Any(Function(x) x.RoleName = role)
    End Function

    Public Sub New(user As AccountDataStoreUser, profile As AcePumpProfile, identity As IIdentity)
        Me._User = user
        Me._Profile = profile
        Me._InternalPrincipal = New GenericPrincipal(identity, {})
    End Sub

    Friend Sub New(originalPrincipal As IPrincipal)
        _InternalPrincipal = originalPrincipal

        If originalPrincipal.Identity.IsAuthenticated Then
            LoadUserInfoFromDb()

        Else
            CreateEmptyUserInfo()
        End If
    End Sub

    Private Sub LoadUserInfoFromDb()
        Dim dataSource As AcePumpContext = DataSourceFactory.GetAcePumpDataSource()
        Dim users = From u In dataSource.Users.Include("Roles")
                    Group Join p In dataSource.AcePumpProfiles.Include("Customer") On p.UserID Equals u.UserID Into ps = Group
                    From p In ps.DefaultIfEmpty()
                    Select New With {
                        .User = u,
                        .Profile = p
                    }

        Dim currentUser = users.SingleOrDefault(Function(x) x.User.Username = _InternalPrincipal.Identity.Name)

        If currentUser Is Nothing Then
            Throw New ArgumentException(String.Format("'{0}' is not recognized as a valid username.", _InternalPrincipal.Identity.Name))
        Else
            _User = currentUser.User
            _Profile = If(currentUser.Profile, New AcePumpProfile())

            If Profile.Customer Is Nothing Then
                Profile.Customer = New Customer()
            End If
        End If
    End Sub

    Private Sub CreateEmptyUserInfo()
        _User = New AccountDataStoreUser() With {
            .Roles = New List(Of AccountDataStoreRole)
        }

        _Profile = New AcePumpProfile() With {
            .Customer = New Customer()
        }
    End Sub
End Class