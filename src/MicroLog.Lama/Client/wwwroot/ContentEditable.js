function ContentEditable(DivID, Instance, TextToDisplay) {
    SetText(DivID, Instance, TextToDisplay)
    SetHeight(DivID);

    document.getElementById(DivID).addEventListener("input", function () {
        Instance.invokeMethodAsync("GetUpdatedText", document.getElementById(DivID).value);
        SetHeight(DivID);
    });

}

function SetText(DivID, Instance, TextToDisplay) {
    document.getElementById(DivID).value = TextToDisplay;
    Instance.invokeMethodAsync("GetUpdatedText", TextToDisplay);
    SetHeight(DivID);
}

function SetHeight(DivID) {
    // Reset to original height
    document.getElementById(DivID).style.height = 'auto';

    // Get the computed styles for the element
    var computed = window.getComputedStyle(document.getElementById(DivID));

    // Calculate the height
    var height = parseInt(computed.getPropertyValue('border-top-width'), 10)
        + document.getElementById(DivID).scrollHeight
        + parseInt(computed.getPropertyValue('border-bottom-width'), 10);

    // Set the height
    document.getElementById(DivID).style.height = (height + 'px');
}