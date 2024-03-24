//Every 5 minute update Parking Lot
setInterval(() => { getParkingCars(); }, 300000);

//Every 1 minute update Elapsed Time
setInterval(() => { updateElapsedTime(); }, 60000);

async function checkIn() {

    const tagNumber = document.getElementById('tagNumber').value;

    if (tagNumber == '') {

        showError('Please Input a Tag Number');

        return;
    }

    try {

        let obj = { tagNumber };

        let setting = {
            method: "POST",
            body: JSON.stringify(obj),
            headers: {
                "Content-Type": 'application/json'
            }
        }

        let response = await fetch("/home/checkin", setting)

        let json = await response.json();

        if (response.ok) {

            showSuccess(json.message);

            document.getElementById('tagNumber').value = '';

            showError('');

            getParkingCars();
        }

        else {
            showAlert(json.message);

            showError('');
        }
    }
    catch (error) {

        showError(error);
    }
}

async function checkOut() {

    const tagNumber = document.getElementById('tagNumber').value;

    if (tagNumber == '') {

        showError('Please Input a Tag Number');

        return;
    }
    try {

        let obj = { tagNumber };
        let setting = {
            method: "PUT",
            body: JSON.stringify(obj),
            headers: {
                "Content-Type": 'application/json'
            }
        }
        let response = await fetch("/home/checkout", setting)

        let json = await response.json();

        if (response.ok) {

            showSuccess(json.message);

            document.getElementById('tagNumber').value = '';

            showError('');

            getParkingCars();
        }

        else {
            showAlert(json.message);

            showError('');
        }

    }
    catch (error) {

        showError(error);
    }
}

async function getParkingCars() {
    try {
        let setting = {
            method: "GET",
            headers: {
                "Content-Type": 'application/json'
            }
        }

        let response = await fetch("/home/parkingcars", setting);

        let json = await response.json();

        if (response.ok) {
            
            document.getElementById('availableSpots').textContent = parseInt(document.getElementById('totalSpots').textContent) - json.length;

            document.getElementById('spotsTaken').textContent = json.length;

            showParkingLots(json);

            showError('');
        }
    } catch (error) {

        showError(error);
    }
}

async function showStats() {

    try {
        let setting = {
            method: "GET",
            headers: {
                "Content-Type": 'application/json'
            }
        }

        let response = await fetch("/home/parkingstats", setting);

        let json = await response.json();

        if (response.ok) {

            showParkingStats(json);

            var showStatsModal = new bootstrap.Modal(statsModal);

            showStatsModal.show();

            showError('');
        }
    } catch (error) {

        showError(error);
    }
}

function formatElapsedTime(minutes) {

    const hours = Math.floor(minutes / 60);

    const remainingMinutes = minutes % 60;

    let formattedTime = '';

    if (hours > 0) {

        formattedTime += hours + ' hours ';
    }
    if (remainingMinutes > 0) {

        formattedTime += remainingMinutes + ' minutes';
    }
    return formattedTime;
}

function showParkingLots(json) {

    let table = document.getElementById("tblParkingLotsBody");

    table.innerHTML = '';

    for (let e of json) {

        let tr = document.createElement('tr');

        tr.id = e.id;

        table.appendChild(tr);

        tr.appendChild(createTD(e.tagNumber));

        var localCheckInDateTime = convertUTCDateToLocalDate(new Date(e.checkInTime));

        let tdCheckInDateTime = document.createElement('td');
        let spanCheckInDateTime = document.createElement('span');
        spanCheckInDateTime.id = 'checkInDateTime' + e.id;
        spanCheckInDateTime.textContent = localCheckInDateTime;
        tdCheckInDateTime.appendChild(spanCheckInDateTime);
        tdCheckInDateTime.style.display = 'none';
        tr.appendChild(tdCheckInDateTime);

        var checkInTime = localCheckInDateTime.toLocaleTimeString('en-US', { hour: 'numeric', minute: '2-digit', hour12: true });
        tr.appendChild(createTD(checkInTime));

        let tdElapsedTime = document.createElement('td');
        let spanElapsedTime = document.createElement('span');
        spanElapsedTime.id = 'elapsedTime' + e.id;
        spanElapsedTime.textContent = calculateElapsedTime(localCheckInDateTime);
        tdElapsedTime.appendChild(spanElapsedTime);
        tr.appendChild(tdElapsedTime);

    }
}

function showParkingStats(json) {

    let table = document.getElementById("tblParkingStats");

    table.innerHTML = '';

    for (let e of json) {

        let tr = document.createElement('tr');

        tr.id = e.id;

        table.appendChild(tr);

        tr.appendChild(createTD(e.statsText));

        tr.appendChild(createTD(e.statsValue));
    }
}

function updateElapsedTime() {

    document.querySelectorAll(document.querySelectorAll(document.querySelectorAll('#tblParkingLotsBody tr'))).forEach(row => {

        const checkInDateTime = new Date(row.querySelector('span[id^="checkInDateTime"]').textContent);

        const elapsedTime = calculateElapsedTime(checkInDateTime);

        row.querySelector('span[id^="elapsedTime"]').textContent = elapsedTime;
    });
}

function calculateElapsedTime(checkInTime) {

    const differenceInMilliseconds = new Date() - checkInTime;

    const elapsedTimeInMinutes = Math.floor(differenceInMilliseconds / (1000 * 60));

    const hours = Math.floor(elapsedTimeInMinutes / 60);

    const minutes = elapsedTimeInMinutes % 60;

    let elapsedTime = '';

    if (hours === 1) {

        elapsedTime += hours + ' hour ';

    } else if (hours > 0) {

        elapsedTime += hours + ' hours ';
    }
    if (minutes === 1) {

        elapsedTime += minutes + ' minute ';

    } else if (minutes > 0) {

        elapsedTime += minutes + ' minutes ';
    }

    if (elapsedTime === '') {

        elapsedTime = '1 minute';
    }

    return elapsedTime;

}
