function showSuccess(message) {

    var successMessage = document.getElementById('successMessage');

    if (successMessage) {

        successMessage.textContent = message;

        var successModal = new bootstrap.Modal(document.getElementById('successModal'));

        successModal.show();
    }
}
function showAlert(message) {

    var alertMessage = document.getElementById('alertMessage');

    if (alertMessage) {

        alertMessage.textContent = message;

        var showAlertModal = new bootstrap.Modal(document.getElementById('showAlert'));

        showAlertModal.show();
    }
}
function showError(message) {

    var errorElement = document.getElementById('error');

    if (errorElement) {

        errorElement.textContent = message;
    }
}
function createTD(value) {

    let td = document.createElement('td');

    td.innerHTML = value;

    return td;
}

function convertUTCDateToLocalDate(dateTime) {

    var localDateTime = new Date(dateTime.getTime() + dateTime.getTimezoneOffset() * 60 * 1000);

    var offset = dateTime.getTimezoneOffset() / 60;

    var hours = dateTime.getHours();

    localDateTime.setHours(hours - offset);

    return localDateTime;
}