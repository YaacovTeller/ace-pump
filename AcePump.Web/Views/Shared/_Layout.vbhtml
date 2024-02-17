@Imports Soris.Mvc.Modules.Account.AccountExtensions
@Imports AcePump.Web

<!DOCTYPE html>
<html>
<head>
    <title>@ViewData("Title")</title>

    @RenderSection("PreloadLibs", False)

    <script src="https://code.jquery.com/jquery-1.9.1.min.js"></script>
    @If ViewData.ContainsKey("ContainsAngularApp") AndAlso ViewData("ContainsAngularApp") = True Then
        @<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/angularjs/1.6.1/angular.min.js"></script>
        @<script src=https://cdnjs.cloudflare.com/ajax/libs/angular.js/1.6.1/angular-cookies.js></script>
        @<script type="text/javascript" src="@Url.Content("~/Scripts/angular-migration-helper.js")"></script>
    End If
    <script src="https://kendo.cdn.telerik.com/2015.2.805/js/kendo.all.min.js"></script>
    <script src="https://kendo.cdn.telerik.com/2015.2.805/js/kendo.aspnetmvc.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/modernizr/1.7/modernizr-1.7.min.js"></script>

    <link href="@Url.Content("~/Content/bootstrap.custom.css")" rel="stylesheet" type="text/css" />
    <link href="@Url.Content("~/Content/Site.css")" rel="stylesheet" type="text/css" />
    <link href="https://kendo.cdn.telerik.com/2015.2.805/styles/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="https://kendo.cdn.telerik.com/2015.2.805/styles/kendo.dataviz.min.css" rel="stylesheet" type="text/css" />
    <link href="https://kendo.cdn.telerik.com/2015.2.805/styles/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <link href="https://kendo.cdn.telerik.com/2015.2.805/styles/kendo.dataviz.default.min.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="Stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <link rel="stylesheet" type="text/css" href="//fonts.googleapis.com/css?family=Open+Sans" />
</head>

<body>
    <div class="page">
        <header>
            <div id="et-info">
                <span id="et-info-phone">(805) 925-7570</span>
                <a href="mailto:"><span id="et-info-email"></span></a>
            </div>
            <div id="title">
                <img src="@Url.Content("~/Content/Ace-Pump-Logo-250.png")" alt="Ace Pump" />
                <h1>
                    @Context.AcePumpUser.Profile.Customer.CustomerName
                    @If Not String.IsNullOrEmpty(AcePump.Common.AcePumpEnvironment.Environment.Configuration.ApplicationSubtitle) Then
                        @<text>- @AcePump.Common.AcePumpEnvironment.Environment.Configuration.ApplicationSubtitle</text>
                    End If
                </h1>

                <div id="logindisplay">
                @Code
                    Html.RenderYesodLoginPartial()
                End Code
                </div>
            </div>
            <div style="clear:both;"></div>
            @Html.Partial("_Menu")
        </header>
        <section id="main">
            @RenderBody()
        </section>
        <footer>
        </footer>
    </div>
</body>
</html>
