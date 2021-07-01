@ModelType List(Of String)
    
@Code
    ViewData("Title") = "Edit Drop Downs"
End Code

<style type="text/css">
    .drop-down {
        float: left;
        width: 200px;
    }
    
    .drop-down-container {
        border: 1px solid black;
        padding: 5px;
        maring: 5px;
    }
    
    .clearfix {
        clear: both;
    }
</style>

<h2>Edit Drop Downs</h2>

Click on the drop down you want to edit.

<div class="drop-down-container">
    @For Each dropDownName As String In Model
        @<div class="drop-down">
            <a href="@Url.Content("~/TypeManager/" & dropDownName)">@Text.RegularExpressions.Regex.Replace(dropDownName, "([A-Z])", " $1").Trim()</a>
        </div>
    Next

    <div class="clearfix" />
</div>