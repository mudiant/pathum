function handleErrorResults(result) {    
    if (result.errors.length) {
        notifyError(result.errors[0]); 
    } else  {
        notifyError("An error occurred while processing your request. Please contact the site administrator.");
    }    
}

function redirectToHome() {
    window.location.href = '/Home';
}

/*modals */
function hideModal(modalId) {
    setTimeout(() => { $(modalId).modal('hide'); }, 100);
}

function showModal(modalId) {
    setTimeout(() => { $(modalId).modal('show'); }, 100);
}

var isModalActive = false;
$(document).on('shown.bs.modal', function (e) {
    isModalActive = true;
    curEle = $(e.target).attr('id');
    $.each($('.modal'), function (idx, item) {
        if (!$(item).hasClass("temp-hide")) {
            if (curEle != $(item).attr("id") && typeof ($(item).attr("id")) != "undefined") {
                if (($(item).hasClass("in") || $(item).hasClass("show"))) {
                    $(item).addClass('temp-hide');
                    $(item).attr('modal-order', $('.modal.temp-hide').length + 1);
                    $(item).modal('hide');
                }
            }
        }
    });
    isModalActive = false;
});

$(document).on('hidden.bs.modal', function () {
    if (isModalActive) return;
    if ($('.modal.show,.modal.in').length > 0) { return; }
    var order = $('.modal').map(function () {
        return parseInt($(this).attr('modal-order'));
    });
    order = order.sort((a, b) => b - a);

    $.each($('.modal'), function (idx, item) {
        if ($(item).hasClass("temp-hide") && order[0] === parseFloat($(item).attr('modal-order'))) {
            $(item).modal('show');
            $(item).removeClass('temp-hide');
            $(item).removeAttr('modal-order');
            return false;
        }
    });
});

function showThumbnail(btnHasClicked){
    const imgTag = btnHasClicked.parentNode.querySelector('.img-thumbnail');
    const file = btnHasClicked.files[0];
    const reader = new FileReader();

    reader.onloadend = function () {
        imgTag.src = reader.result;
    }

    if (file) {
        reader.readAsDataURL(file);
    } else {
        imgTag.src = "";
    }
}
$(document).ready(function () {
    $('[data-bs-toggle="tooltip"]').tooltip();
});

function numbersOnly(ele) {
    ele.value = ele.value.replace(/[^0-9]/g, '');
}

function decimalOnly(el) {
    el.value = el.value.replace(/[^\d.]/g, '');
}

function eventIntegerInput(evt) {
    var charCode = (evt.which) ? evt.which : window.event.keyCode;
    if (charCode != 8 && charCode != 0 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function eventDecimalInput(evt) {
    var charCode = (evt.which) ? evt.which : window.event.keyCode;
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function decimalInputValidation(evt, element, decimalPlaces) {
    var charCode = (evt.which) ? evt.which : window.event.keyCode;
    if (!eventDecimalInput(evt)) {
        return false;
    }

    var number = element.value.split('.');
    if (number.length > 1 && charCode == 46) {
        return false;
    }

    if (decimalPlaces > 0) {
        var caratPos = getSelectionStartIndex(element);
        var dotPos = element.value.indexOf(".");
        if (caratPos > dotPos && dotPos > -1 && number[1].length >= decimalPlaces) {
            return false;
        }
    }

    return true;
}

function getSelectionStartIndex(element) {
    if (element.createTextRange) {
        var selection = window.document.selection.createRange().duplicate();
        selection.moveEnd("character", element.value.length);
        if (selection.text == "") return element.value.length;
        return element.value.lastIndexOf(selection.text);
    } else return element.selectionStart;
}

function validateMax(value, max) {
    if (value > max && event.keyCode !== 46 && event.keyCode !== 8) {
        return false;
    }
    return true;
}


$(document).on("keypress", "[validate-decimal]", function (event) {
    //.validate-decimal or [validate-decimal] or [validate-decimal]="0" will just ensure input is numeric with a decimal point
    //[validate-decimal]="2" will limit the decimal entry to 2 significant digits 
    var sigDigits = ($(this).attr("validate-decimal") == null || !$.isNumeric($(this).attr("validate-decimal"))) ? 0 : $(this).attr("validate-decimal");
    if (!decimalInputValidation(event, $(this)[0], sigDigits)) {
        event.preventDefault();
    }
});

$(document).on("keypress", "[validate-integer]", function (event) {
    if (!eventIntegerInput(event)) {
        event.preventDefault();
    }
});

$(document).on("keyup keydown change", "[validate-max]", function (event) {
    var maxValue = ($(this).attr("validate-max") == 99 || !$.isNumeric($(this).attr("validate-max"))) ? 99 : $(this).attr("validate-max");
    console.log($(this).val());
    if (!validateMax($(this).val(), maxValue)) {
        console.log('invalid');
        event.preventDefault();
        $(this).val(maxValue);
    }
});


function updateUrlState(data) {
    window.history.pushState(null, null, data);
}

function isStringEmpty(str) {
    return !str || (isTypeOfString(str) && str.length === 0);
}
function isTypeOfString(obj) {
    return (typeof obj === 'string' || obj instanceof String);
}

function isPasswordValid(password) {
    //The Password must contain at least 1 upper case, lower case and non-alphanumeric character.
    var regex = /(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?!.*\s).*$/;
    return regex.test(password);
}

function passwordMatch(passwordElement, confirmPassword) {
    if ($(passwordElement).val() != confirmPassword) {
        return false;
    }
    return true;
}

function isValidZip(inputText) {
    return /^\d{5}(-\d{4})?$/.test(inputText);
}

function isValidPhone(inputText) {
    return /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/.test(inputText);
}

function isValidEmail(email) {
    //var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    //return regex.test(email);
    //var regex = /^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$/;
    var regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return regex.test(email);
}

function isValidEmailList(emailList) {

    var emails = emailList.split(/[,|;]/);
    var bValid = true;
    $.each(emails, function (idx, item) {
        if (item.length > 0 && !isValidEmail(item)) {
            bValid = false;
            return false;
        }
    });
    return bValid;
}

function validateEmailAlreadyExists(id, email) {
    return validateFromServer('/Account/IsValidEmail', postData({ id: id, email: email }));
}

function validateUserNameAlreadyExists(id, userName) {
    return validateFromServer('/Account/IsValidUserName', postData({ id: id, userName: userName }));
}

function validatePhoneNumberAlreadyExists(id, phoneNumber) {
    return validateFromServer('/Account/IsValidPhoneNumber', postData({ id: id, phoneNumber: phoneNumber }));
}

