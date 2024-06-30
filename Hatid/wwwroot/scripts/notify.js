const notifySuccess = (message, timeout) => {
    closeToast();
    var toastMessage = typeof message === 'undefined' ? 'Data has been updated successfully!' : message;
    var toastTimeout = typeof timeout === 'undefined' ? 2500 : timeout;
    iziToast.success({
        title: '',
        message: toastMessage,
        position: 'topCenter',
        color: 'green',
        timeout: toastTimeout,
        progressBar: false
    });
};
const notifyError = (message, timeout) => {
    closeToast();
    var toastMessage = typeof message === 'undefined' ? 'An error occurred!' : message;
    var toastTimeout = typeof timeout === 'undefined' ? 2500 : timeout;
    iziToast.error({
        message: toastMessage,
        position: 'topCenter',
        timeout: toastTimeout,
        progressBar: false
    });
};
const notifyWarning = (message, timeout) => {
    closeToast();
    var toastMessage = typeof message === 'undefined' ? 'An error occurred!' : message;
    var toastTimeout = typeof timeout === 'undefined' ? 2500 : timeout;
    iziToast.warning({
        message: toastMessage,
        position: 'topCenter',
        timeout: toastTimeout,
        progressBar: false
    });
};

const closeToast = () => {
    var toast = document.querySelector('.iziToast');
    if (toast !== null) iziToast.hide({}, toast);
};

const confirmBox = (message, okFn) => {
    iziToast.question({
        close: true,
        overlay: true,
        timeout: false,
        drag: false,
        displayMode: 'once',
        color: 'red',
        title: message,
        position: 'center',
        buttons: [
            ['<button><b>YES</b></button>', function (instance, toast) {
                okFn();
                instance.hide({ transitionOut: 'fadeOut' }, toast, 'button');

            }, true],
            ['<button>NO</button>', function (instance, toast) {

                instance.hide({ transitionOut: 'fadeOut' }, toast, 'button');

            }],
        ]
    });
};
