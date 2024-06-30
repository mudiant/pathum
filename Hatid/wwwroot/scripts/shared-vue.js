Vue.filter('capitalize', function (value) {
    if (!value) return ''
    value = value.toString()
    return value.charAt(0).toUpperCase() + value.slice(1)
})
Vue.filter('formatPrice', function (value, currencyType) {
    let symbol = typeof currencyType === 'undefined' ? '' : currencyType;
    if (!value) return `${symbol}0`;
    value = (value / 1).toFixed(2);
    return `${symbol}${value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}`;
})
Vue.filter('formatQty', function (value) {
    if (!value) return '0'
    value = (value / 1).toFixed(0);
    return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
})
Vue.filter('formatPercentage', function (value) {
    if (!value) return '%'
    value = (value / 1).toFixed(0);
    return value.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + '%';
})
Vue.filter('formatDate', function (value) {
    return value !== null ? moment(value).format('DD/MM/YYYY'): '';
})
Vue.filter('formatDateTime', function (value) {
    return value !== null ? moment(value).format('DD/MM/YYYY hh:mm A') : '';
})
Vue.filter('formatTime', function (value) {
    return (value === value || value.length === 0) ? '' : formatTime(value);
})

Vue.directive("form-validation", {
    bind: function (el, binding) {
        initFormValidation(el);
    }
});

Vue.directive("date-picker", {
    bind: function (el, binding) {
        $(el).datetimepicker({
            timepicker: false,
            format: 'd/m/Y',
        });
    }
});
Vue.filter('formatCurrency', function (value, currencyType) {    
    return value.toLocaleString('en-US', { style: 'currency', currency: currencyType })
})

//https://xdsoft.net/jqplugins/datetimepicker/


Vue.directive("disable-all", {
    bind: function (el, binding) {        
        if (!binding.value) return;
        const tags = ["input", "button", "textarea", "select"];
        tags.forEach(tagName => {
            const nodes = el.getElementsByTagName(tagName);
            for (let i = 0; i < nodes.length; i++) {
                if (nodes[i].getAttribute('ignore-disable') === 'true') continue; //add ignore-disable attribute exclude element from disabling
                nodes[i].disabled = true;
                nodes[i].tabIndex = -1;
            }
        });
    }
});