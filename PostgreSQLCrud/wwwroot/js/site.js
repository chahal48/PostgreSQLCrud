// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    // show the alert
    setTimeout(function () {
        $(".alert").alert('close');
    }, 1500);
});
$(document).ready(function () {
    $('#DOB').datepicker({
        dateFormat: "yy-mm-dd",
        changeMonth: true,
        changeYear: true,
        yearRange: '1980:2018',
        maxDate: '-5Y',
        //minDate: '-40Y',
        defaultDate: '1980-01-01',
        onSelect: function (date, datepicker) {
            if (date != "") {
                var DateChangeValue = document.getElementById('DOB');
                $(DateChangeValue).attr('value', date);
            }
        }
    });
})
function fileValidation() {
    const alertMessage = "Invalid file type!! \nOnly files with the following extension are allowed :  jpg jpeg png gif.";
    var baseUrl = document.location.origin;
    var DefaultImagePath = baseUrl + '/Upload/blank.jpg';
    var fileInput = document.getElementById('Image');
    var filePath = fileInput.value;
    // Allowing file type
    var allowedExtensions =
        /(\.jpg|\.jpeg|\.png|\.gif)$/i;
    if (!allowedExtensions.exec(filePath)) {
        alert(alertMessage);
        fileInput.value = null;
        var DefaultImage = document.getElementById('imagePreview');
        $(DefaultImage).attr('src', DefaultImagePath);
        return false;
    }
    else {
        // Image preview
        if (fileInput.files && fileInput.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                //document.getElementById(
                //    'imagePreview').innerHTML =
                //    '<img src="' + e.target.result
                //    + '"/>';
                var Preview = document.getElementById('imagePreview');
                //var src = ($(Preview).attr('src') === e.target.result) ? e.target.result : e.target.result;
                $(Preview).attr('src', e.target.result);
            };
            reader.readAsDataURL(fileInput.files[0]);
        }
    }
}