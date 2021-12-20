$(document).ready(function () {
    function courseSelected(courseFee) {
        $("#txtCourseFee").val(courseFee);
    }
    $("form").on('submit', function () {
        event.preventDefault();
        // TODO do something here to show user that form is being submitted
        fetch(event.target.action, {
            method: 'POST',
            body: new URLSearchParams(new FormData(event.target)) // event.target is the form
        }).then((resp) => {
            console.log(resp);
            return resp.json(); // or resp.text() or whatever the server sends
        }).then((body) => {
            // TODO handle body
            console.log(body);
            $("#txtContact, #CourseId, #txtAmountPaid").val('');
            $("#txtCourseFee").val('0');
            $("#divLoading").hide();
        }).catch((error) => {
            // TODO handle error
            console.log(error);
            $("#divLoading").hide();

        });
        $("#divLoading").show();
    });
    //$("form").on('success', function (data) {
        //$("#divLoading").hide();
    //});
});