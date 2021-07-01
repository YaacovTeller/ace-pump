@Code
    ViewData("Title") = "Ace Pump"
End Code

<h2>Ace Pump</h2>

Welcome to the Ace Pump reporting system.  Use the menu above to explore your ticket history
or current information about your pumps.

<br />
<br />
Common Tasks:

<ul>
    <li>@Html.ActionKendoButton("View Pumps", "Index", "Pump")</li>
</ul>