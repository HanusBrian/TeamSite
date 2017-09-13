// Write your Javascript code.
$(document).ready(function () {
    $('.datepicker').datepicker({
        todayBtn: "linked"
    });
});

//$('#splitHelpText').on('click', (e) => {
//    e.preventDefault();
//    var url = $("#splitHelpText").parent()[0].action;
//    var fileUpload = $("#DocToZipFiles").get(0);

//    var files = fileUpload.files;

//    var formData = new FormData();
//    for (var i = 0; i < files.length; i++) {
//        formData.append(files[i].name, files[i]);
//    }

//    console.log("This is just for a pause.");

//    $.ajax({
//        url: url,
//        type: "POST",
//        contentType: false,
//        processData: false,
//        data: formData
//    })
//        //.done(alert("DocToZip completed"))
//        //    $.ajax({
//        //        url: 'UploadFileToUser',
//        //        method: "POST",
//        //        data: formData
//        //    })
//        .fail();
//});
