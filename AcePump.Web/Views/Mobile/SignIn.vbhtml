@ModelType AcePump.Web.Models.MobileLoginViewModel

@Code
    ViewData("Title") = "Mobile Sign In"
    Layout = "~/Views/Shared/_Layout.mobile.vbhtml"
End Code

<h2>Log In</h2>

<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>

<style type="text/css">
    .button {
        width: 7em;
        height: 5em;   
    }
    
    .check 
    {
        width: 3em;
        height: 3em;   
    }
</style>
@Html.ValidationSummary(True, "Login was unsuccessful. Please correct the errors and try again.")

@Using Html.BeginForm()
        @<fieldset>
            <ol>
                <li>
                    <div class="editor-label">
                        @Html.LabelFor(Function(m) m.UserName)
                    </div>
                    <div class="editor-field">
                        @Html.TextBoxFor(Function(m) m.UserName)
                        @Html.ValidationMessageFor(Function(m) m.UserName)
                    </div>
                </li>
                    
                <li>
                    <div class="editor-label">
                        @Html.LabelFor(Function(m) m.Password)
                    </div>
                    <div class="editor-field">
                        @Html.PasswordFor(Function(m) m.Password)
                        @Html.ValidationMessageFor(Function(m) m.Password)
                    </div>
                </li>
                    
                <li>
                    <div class="editor-label">
                        @Html.CheckBoxFor(Function(m) m.RememberMe, New With {.class = "check"})
                        @Html.LabelFor(Function(m) m.RememberMe)
                    </div>
                </li>
            </ol>

            <p>
                <input type="submit" value="Login" class="button"/>
            </p>
        </fieldset>
End Using
