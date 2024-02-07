// Use to display matching names
function displayMatchingNames(namesArray, inputTagId, matchingNamesContainerId) {
    // Get the input value
    var inputValue = $(inputTagId).val().toLowerCase();

    // Filter names based on the input value
    var matchingNames = namesArray.filter(function (name) {
        return name.toLowerCase().includes(inputValue);
    });

    // Display matching names below the input element
    var matchingNamesContainer = $(matchingNamesContainerId);
    matchingNamesContainer.empty();

    var inputField = $(inputTagId);
    var inputPosition = inputField.position();

    if (matchingNames.length > 0) {
        matchingNamesContainer.css({
            "position": "absolute",
            "top": inputPosition.top + inputField.outerHeight(),
            "left": inputPosition.left,
            "width": inputField.outerWidth()
        });

        $(matchingNamesContainerId).show();
        matchingNames.forEach(function (name) {
            matchingNamesContainer.append("<div><a style='text-decoration:none' onclick='onMatchingNameEntryClicked(\"" + name + "\", \"" + inputTagId + "\", \"" + matchingNamesContainerId + "\")' href='#'>" + name + "</a></div>");
        });
    } else {
        matchingNamesContainer.append("<div>No matching names found</div>");
        matchingNamesContainer.hide();
    }
}

function onMatchingNameEntryClicked(name, inputTagName, matchingNamesContainerId) {
    $(inputTagName).val(name);
    $(matchingNamesContainerId).hide();
}