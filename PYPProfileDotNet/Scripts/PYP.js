function enableReadonlyInputs() {
    var form = this.form || $(this).next('form').first();
    $(form).find('input[readonly]').removeAttr('readonly');
}

function resetForm() {
    this.form.reset();
}