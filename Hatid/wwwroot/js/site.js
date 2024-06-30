/*modals */
function hideModal(modalId) {
    setTimeout(() => { $(modalId).modal('hide'); }, 100);
}

function showModal(modalId) {
    setTimeout(() => { $(modalId).modal('show'); }, 100);
}

function postData(data) {
    //if (typeof data.__RequestVerificationToken === "undefined") {
    //    data.__RequestVerificationToken = $(
    //        "#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]"
    //    ).val();
    //}
    return data;
}

function validateEmailAlreadyExists(id, email) {
    return validateFromServer('/Account/IsValidEmail', postData({ id: id, email: email }));
}

function validatePhoneNumberAlreadyExists(id, phoneNumber) {
    return validateFromServer('/Account/IsValidPhoneNumber', postData({ id: id, phoneNumber: phoneNumber }));
}

//function getHours(timeIn, timeOut) {
//    const startTime = moment(timeIn);
//    const endTime = moment(timeOut);
//    const duration = moment.duration(endTime.diff(startTime));
//    const hours = duration.hours();
//    return hours < 0 ? 0 : hours;
//}
//function getMinutes(timeIn, timeOut) {
//    const startTime = moment(timeIn);
//    const endTime = moment(timeOut);
//    const duration = moment.duration(endTime.diff(startTime));
//    const minutes = duration.minutes();
//    return minutes < 0 ? 0 : minutes;
//}
//function getSeconds(timeIn, timeOut) {
//    const startTime = moment(timeIn);
//    const endTime = moment(timeOut);
//    const duration = moment.duration(endTime.diff(startTime));
//    const seconds = duration.seconds();
//    return seconds < 0 ? 0 : seconds;
//}
