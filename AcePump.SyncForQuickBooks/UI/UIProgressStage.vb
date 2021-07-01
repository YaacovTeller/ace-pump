Namespace UI
    Public Enum UIProgressStage
        ConnectingToApi
        DownloadingTicketsFromApi
        ReadingTicketsFromApi
        ValidatingTickets
        LoadingCustomerList
        LoadingPartList
        ConnectingToQb
        ProcessingNewInvoices
        ProcessingModifiedInvoices
        SavingTickets
        Failure
        Canceling
        Canceled
        NoTicketsToProcess
        Complete
        Start

        ProcessingTicketAdditionalData

        BuildingInvoiceListsFromTickets
    End Enum
End Namespace