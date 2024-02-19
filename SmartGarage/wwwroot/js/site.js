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

function setFormAsWaitable(formId, submitButtonId, loadingBarId) {
    document.getElementById(formId).addEventListener('submit', function () {

        // Disable the input field
        var formElements = this.elements;
        for (var i = 0; i < formElements.length; i++) {
            console.log(formElements[i]);
            formElements[i].readOnly = true;
        }

        // Disable the submit button
        document.getElementById(submitButtonId).disabled = true;

        // Show the loading spinner
        document.getElementById(loadingBarId).style.display = 'inline-block';
    });
}

function setFormAsWaitableManual(formId, submitButtonId, loadingBarId) {

    if (formId == null || submitButtonId == null || loadingBarId == null)
        return;

    var form = document.getElementById(formId);

    // Disable the input field
    var formElements = form.elements;
    for (var i = 0; i < formElements.length; i++) {
        console.log(formElements[i]);
        formElements[i].readOnly = true;
    }

    // Disable the submit button
    document.getElementById(submitButtonId).disabled = true;

    // Show the loading spinner
    toggleLoagindSpinner(loadingBarId, true);
}

function toggleLoagindSpinner(loadingBarId, state) {

    if (loadingBarId == null)
        return;
    
    if (state == true)
        document.getElementById(loadingBarId).style.display = 'inline-block';
    else
        document.getElementById(loadingBarId).style.display = 'none';

    console.log("Set loading spinner to : ", state);
}