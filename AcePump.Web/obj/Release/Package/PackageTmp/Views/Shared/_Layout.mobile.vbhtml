<!DOCTYPE html>
<html>
<head>
	<title>@ViewData("Title")</title>
    <link href="@Url.Content("~/Content/Mobile.css")" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="initial-scale=1.0, target-densitydpi=device-dpi" /><!-- this is for mobile (Android) Chrome -->
    <meta name="viewport" content="initial-scale=1.0, width=device-height" /><!--  mobile Safari, FireFox, Opera Mobile. "device-HEIGHT" is intentional  -->
    <meta name="viewport" content="width=device-width, initial-scale=1" />
</head>

<body>
	<div class="page">
		<header>
			<div>
				<img class="logo" src="@Url.Content("~/Content/AcePumpLogoSm.jpg")" alt="Ace Pump" />
			</div>
		</header>
		<section id="main">
			@RenderBody()
		</section>
		<footer>
		</footer>
	</div>
</body>
</html>
