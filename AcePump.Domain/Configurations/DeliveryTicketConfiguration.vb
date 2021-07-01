Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Namespace Configurations
    Public Class DeliveryTicketConfiguration
        Inherits EntityTypeConfiguration(Of DeliveryTicket)

        Public Sub New()
            [Property](Function(s) s.PlungerBarrelWear).HasMaxLength(306).HasColumnType("char")
            [Property](Function(x) x.SalesTaxRate).HasPrecision(6, 5)

            HasOptional(Function(x) x.PumpFailed).WithMany().HasForeignKey(Function(x) x.PumpFailedID).WillCascadeOnDelete(False)
            HasOptional(Function(x) x.PumpDispatched).WithMany().HasForeignKey(Function(x) x.PumpDispatchedID).WillCascadeOnDelete(False)
            HasOptional(Function(x) x.Customer).WithMany(Function(x) x.DeliveryTickets).WillCascadeOnDelete(False)
            HasOptional(Function(x) x.Well).WithMany(Function(x) x.DeliveryTickets).HasForeignKey(Function(x) x.WellID).WillCascadeOnDelete(False)

            HasMany(Function(x) x.Inspections).WithRequired(Function(x) x.DeliveryTicket).WillCascadeOnDelete(True)
            HasMany(Function(x) x.LineItems).WithRequired(Function(x) x.DeliveryTicket).WillCascadeOnDelete(True)
        End Sub
    End Class
End Namespace