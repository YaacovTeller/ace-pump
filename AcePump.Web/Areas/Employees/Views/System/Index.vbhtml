@Code
    ViewData("Title") = "System Administration Tools"
End Code

<h2>System Administration Tools</h2>

<script type="text/javascript">
    window.view = (function ($, undefined) {
        var WebBackgroundTask = function (taskName, taskUrl, taskData, progressUrl) {
            this._TaskName = taskName;
            this._TaskUrl = taskUrl;
            this._TaskData = taskData;
            this._TaskId = null;

            this._ProgressUrl = progressUrl || "@Url.Action("GetTaskProgress", "System")";
            this._QueryInterval = 2000;

            this._PercentComplete = 0;
            this._TotalOperations = 0;
            this._OperationsComplete = 0;
            this._InitializeProgressDisplay();
        };

        WebBackgroundTask.prototype._InitializeProgressDisplay = function() {
            var lblName = $("<span class=\"s-webbg-name\">" + this._TaskName + "</span>");
            var bar = $("<span>");

            this._LblTotalOperations = $("<span>");
            this._LblOperationsComplete = $("<span>");
            this._ProgressDisplay = $("<li>").append(lblName).append(bar).append(this._LblOperationsComplete).append($("<span> of </span>")).append(this._LblTotalOperations);
            $(".s-webbg-tasklist").append(this._ProgressDisplay);

            this._ProgressBar = bar.kendoProgressBar({
                type: "percent"
            }).data("kendoProgressBar");
        };

        WebBackgroundTask.prototype.Start = function () {
            var task = this;

            $.ajax({
                url: this._TaskUrl,
                dataType: "json",
                type: "POST",
                data: this._TaskData,
                success: function (data, status, xhr) {
                    task._TaskId = data.Id;
                    task._QueryProgress();
                }
            });
        };

        WebBackgroundTask.prototype._QueryProgress = function () {
            var task = this;

            $.ajax({
                url: this._ProgressUrl,
                dataType: "json",
                type: "POST",
                data: { id: this._TaskId },
                success: function (data, status, xhr) {
                    if( data.PercentComplete < 100 ) {
                        task._UpdateProgress( data );
                        setTimeout(function() { task._QueryProgress(); }, task._QueryInterval);
                    
                    } else {
                        task._Complete();
                    }
                }
            });
        };

        WebBackgroundTask.prototype._UpdateProgress = function( newProgressInfo ) {
            this._PercentComplete = newProgressInfo.PercentComplete;
            this._TotalOperations = newProgressInfo.TotalOperations;
            this._OperationsComplete = newProgressInfo.OperationsComplete;

            this._ProgressBar.value(this._PercentComplete);
            this._LblOperationsComplete.text(this._OperationsComplete);
            this._LblTotalOperations.text(this._TotalOperations);
        };

        WebBackgroundTask.prototype._Complete = function() {
            this._ProgressBar.value(100);
            this._AnimateOutProgressDisplay();

            this._ProgressBar = null;
            this._ProgressDisplay = null;
        };

        WebBackgroundTask.prototype._AnimateOutProgressDisplay = function() {
            this._ProgressDisplay.fadeOut(400, function() { $(this).remove(); });
        };

        var HelperClass = function () {
        };

        HelperClass.prototype.Initialize = function () {
            this._AttachHandlers();
        };

        HelperClass.prototype._AttachHandlers = function() {
            $(".s-webbg-button").each(function(){
                $(this).click(function(){
                    var btn = $(this);
                    var taskName = btn.text();
                    var taskUrl = btn.data("taskurl");
                    var taskData = btn.data("taskdata");
                    var progressUrl = undefined;

                    var task = new WebBackgroundTask(taskName, taskUrl, taskData, progressUrl);
                    task.Start();
                });
            });
        };

        return {
            Helper: new HelperClass()
        };
    })(jQuery);

    jQuery(window).load(function () {
        window.view.Helper.Initialize();
    });
</script>

<p>Do <strong>NOT</strong> use these tools unless you have been trained exactly how to use them.
Improper use can destroy the system.</p>

<h2>In Progress Task List</h2>
<ul class="s-webbg-tasklist"></ul>

<a class="k-button s-webbg-button" data-taskurl="@Url.Action("RecalculateRuntimes", "System")">Recalculate Runtimes</a>