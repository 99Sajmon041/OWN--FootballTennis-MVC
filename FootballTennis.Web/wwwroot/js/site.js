
document.addEventListener("DOMContentLoaded", function () {
    const alerts = document.querySelectorAll(".alert-dismissible");

    if (alerts.length === 0) return;

    setTimeout(function () {

        alerts.forEach(function (alert) {

            const bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
            bsAlert.close();
        });
    }, 4000);
});


document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("[data-player-select-form]");
    if (!form) {
        return;
    }

    const selects = form.querySelectorAll(".player-select");

    function updateOptions() {
        const selectedValues = Array.from(selects)
            .map(s => s.value)
            .filter(v => v !== "");

        selects.forEach(select => {
            Array.from(select.options).forEach(option => {
                if (option.value === "") return;

                const isAssigned = option.dataset.assigned === "1";
                const isSelectedElsewhere = selectedValues.includes(option.value) && option.value !== select.value;

                option.disabled = isAssigned || isSelectedElsewhere;
            });
        });
    }

    selects.forEach(select => {
        select.addEventListener("change", updateOptions);
    });

    updateOptions();
});