// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function toggleEditingVisible(itemID) {
    var editForm = $('#EditForm-' + itemID.toString());
    if (editForm.css('display') == 'none' || editForm.css('visiblity') == 'hidden') {
        $('#EditForm-' + itemID.toString()).show();
        $('#DisplayForm-' + itemID.toString()).hide();
        $('#btnEditCancel-' + itemID).text("Cancel");
        $('#btnSubmit-' + itemID).show();
    } else {
        $('#EditForm-' + itemID.toString()).hide();
        $('#DisplayForm-' + itemID.toString()).show();
        $('#btnEditCancel-' + itemID).text("Edit");
        $('#btnSubmit-' + itemID).hide();
    }
}

function toggleEditMode() {
    fetch('/Index?handler=ToggleEditMode', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify({ })
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to update server.');
            }
            return;
        })
        .then(data => console.log('Server updated:', data))
        .catch(error => console.error(error));
}
