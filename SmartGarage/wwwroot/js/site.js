// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function custom_back() {
    history.back();
}

var clearForm = function (formId) {
    $("#" + formId + " input[type=text]").val("");
};


// ====================================================================================================
//
// ====================================================================================================

var callback;

function subscribeToRepairActivityRemovedCallback(subscriber) {
    callback = subscriber;
}

// Call this when the achor is clicked to invoke the callback
function onRepairActivityRemoveButtonClicked(activityId) {
    callback(activityId);
}