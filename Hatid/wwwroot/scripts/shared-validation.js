var initFormValidator = function (ele) {
    $(ele).removeData("validator").removeData("unobtrusiveValidation");
    var rules = {};
    var messages = {};
    var formName = ele.attr('id');

    $("#" + formName + " :input").each(function () {
        if (typeof $(this).attr('required') !== 'undefined') {
            rules[$(this).attr('name')] = {
                required: true
            };
            messages[$(this).attr('name')] = {
                required: typeof $(this).attr('data-msg-required') === 'undefined' ? (typeof $(this).attr('placeholder') === 'undefined' ? 'The field is required.' : 'The ' + $(this).attr('placeholder') + ' is required.') : $(this).attr('data-msg-required')
            };
        }
    });

    $(ele).validate({
        errorClass: 'is-invalid',
        rules: rules,
        messages: messages,
        invalidHandler: function (event, validator) {
            //$('.form-control').removeClass('top-error');
            //$(validator.errorList[0].element).addClass('top-error');
        }
    });

    if (typeof $(ele).attr('ignore-control') !== typeof undefined && $(ele).attr('ignore-control') !== false) {
        $(ele).validate().settings.ignore = '.text-editor *, :hidden:not("#' + $(ele).attr('id') + ' #' + $(ele).attr('ignore-control') + '")';
    } else {
        //$(ele).validate().settings.ignore = ".text-editor *";
        $('form').each(function () {
            if ($(this).data('validator'))
                $(this).data('validator').settings.ignore = ".text-editor *";
        });
    }


    $('#' + formName + ' [select-required]').each(function () {
        $.validator.addMethod('selectRequired', function (value, element) {
            return isValidDropdownSelection(value, $(element).attr('allow-zero'));
        }, function (params, element) { return $(element).attr('select-required').length > 0 ? $(element).attr('select-required') : 'Please select a value.'; });


        $('#' + formName + ' #' + $(this).attr('id')).rules("add", {
            selectRequired: true
        });
    });

    $('#' + formName + ' [unique-required]').each(function () {
        var functionStr = $(this).attr('unique-required');
        var errorMessage = typeof $(this).attr('data-msg-unique-required') === 'undefined' ? 'This field cannot be duplicated' : $(this).attr('data-msg-unique-required');
        $.validator.addMethod('checkDuplicate', function (value, element) {
            switch (functionStr) {
                //case "validateUserName": return vueUser.validateUserName();


            }
        }, errorMessage);

        $('#' + formName + ' #' + $(this).attr('id')).rules("add", {
            checkDuplicate: true
        });
    });

    $('#' + formName + ' [remote-validation]').each(function () {
        var functionStr = $(this).attr('remote-validation');
        var errorMessage = typeof $(this).attr('data-msg-remote-validation') === 'undefined' ? 'This field cannot be duplicated.' : $(this).attr('data-msg-remote-validation');
        $(this).rules("add", {
            remote: function () {
                switch (functionStr) {
                    case "validateEmailAlreadyExists": return validateEmailAlreadyExists(vueUser.editUser.id, vueUser.editUser.email);
                    case "validatePhoneNumberAlreadyExists": return validatePhoneNumberAlreadyExists(vueUser.editUser.id, vueUser.editUser.phoneNumber);
                }
            },
            messages: {
                remote: errorMessage
            }
            //,async: false

        });
    });

    $('#' + formName + ' [valid-email]').each(function () {
        $.validator.addMethod('isValidEmail', function (value, element) {
            if (isStringEmpty(value) && !$(element)[0].hasAttribute("required")) return true;
            return isValidEmail(value);
        }, function (params, element) { return $(element).attr('valid-email').length > 0 ? $(element).attr('valid-email') : 'Invalid Email Address.'; });

        $('#' + formName + ' #' + $(this).attr('id')).rules("add", {
            isValidEmail: true
        });
    });

    $('#' + formName + ' [valid-emails]').each(function () {
        $.validator.addMethod('isValidEmailList', function (value, element) {
            if (isStringEmpty(value) && !$(element)[0].hasAttribute("required")) return true;
            return isValidEmailList(value);
        }, function (params, element) { return $(element).attr('valid-emails').length > 0 ? $(element).attr('valid-emails') : 'Invalid separate emails with ";" or ","'; });

        $('#' + formName + ' #' + $(this).attr('id')).rules("add", {
            isValidEmailList: true
        });
    });

    $('#' + formName + ' [compare-password]').each(function () {
        var passwordElement = '#' + formName + ' #' + $(this).attr('compare-password');
        $.validator.addMethod('comparePassword', function (value, element) {
            return passwordMatch(passwordElement, value);
        }, 'Passwords do not match');

        $('#' + formName + ' #' + $(this).attr('id')).rules("add", {
            comparePassword: true
        });
    });

    $('#' + formName + ' [valid-phone]').each(function () {
        $.validator.addMethod('isValidPhone', function (value, element) {
            if (isStringEmpty(value) && !($(element)[0].hasAttribute("req") || $(element)[0].hasAttribute("required"))) return true;
            return isValidPhone(value);
        }, function (params, element) { return $(element).attr('valid-phone').length > 0 ? $(element).attr('valid-phone') : 'Invalid Phone Number.'; });

        $(this).rules("add", {
            isValidPhone: true
        });
    });

    $('#' + formName + ' [valid-password]').each(function () {
        $.validator.addMethod('isPasswordValid', function (value, element) {
            return isPasswordValid(value);
        }, 'The Password must contain at least 1 upper case, lower case, non-alphanumeric character and password must be at least 8 characters long');

        $('#' + formName + ' #' + $(this).attr('id')).rules("add", {
            isPasswordValid: true
        });
    });
};

function validateFromServer(url, postData) {
    return {
        url: url,
        type: "POST",
        data: postData,
        dataFilter: function (data) {
            var msg = JSON.parse(data);
            if (msg.hasOwnProperty('d'))
                return msg.d;
            else
                return msg;
        }
        , async: false
    };
}

function isValidDropdownSelection(value, allowZero) {
    var ignoreZero = typeof allowZero !== 'undefined';
    if (ignoreZero) {
        if (value == '' || value == null) {
            return false;
        }
    } else {
        if (value == '0' || value == '' || value == null) {
            return false;
        }
    }
    return true;
}

function initFormValidation(frm) {
    setTimeout(() => { initFormValidator($(frm)); }, 100);
}

function isPasswordValid(password) {
    //The Password must contain at least 1 upper case, lower case and non-alphanumeric character.
    var regex = /(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?!.*\s).{8,}$/;
    return regex.test(password);
}