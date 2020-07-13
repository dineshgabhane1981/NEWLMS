

$(document).ready(function () {
    if ($("#UserId").val() == 0)
        $("#UserRoles").val(3);

    if ($("#UserId").val() == 0) {
        $("#TenantUserRoles").val(2);
        $("#TenantUserRoles").prop("disabled", true);

    }

    $('#btnUserSubmit').on("click", function () {
        var status = UserValidation();
        return status;
    });

    $('#btnTenantSubmit').on("click", function () {
        var status = TenantValidation();
        return status;
    });

    $('#btnCourseSubmit').on("click", function () {
        var status = CourseValidation();
        return status;
    });
    $('#btnUserUpload').on("click", function () {
        var status = UserUploadValidation();
        return status;
    });
    $('#ForumTypeList').change(function () {
        console.log(111)
        if ($("#ForumTypeList").val() == "2") {
            $("#dvIsBrodcast").hide();
        } else {
            $("#dvIsBrodcast").show();
        }
    });

    $('#btnForumSubmit').on("click", function () {
        var status = CreateForumValidation();
        return status;
    });
});

function UserValidation() {
    if ($("#FirstName").val() == "") {
        alert("Please enter First Name");
        $("#FirstName").focus();
        return false;
    }

    if ($("#EmailId").val() == "") {
        alert("Please enter Email Id");
        $("#EmailId").focus();
        return false;
    }
    if ($("#Password").val() != "") {
        if ($("#OldPassword").val() == "") {
            alert("Please enter Current Password");
            $("#OldPassword").focus();
            return false;
        }
        if ($("#Password").val() != $("#ConfirmPassword").val()) {
            alert("New Password and Current Password is not matching");
            return false;
        }
    }

    return true;
}

function TenantValidation() {

    if ($("#TenantName").val() == "") {
        alert("Please enter Client Name");
        return false;
    }

    if ($("#TenantDomain").val() == "") {
        alert("Please enter Client Domain");
        return false;
    }

    if ($("#DtActivitionFrom").val() == "") {
        alert("Please enter Activation From Date");
        return false;
    }

    if ($("#DtActivitionTo").val() == "") {
        alert("Please enter Activation To Date");
        return false;
    }

    if ($("#NoOfUserAllowed").val() == "") {
        alert("Please enter Number of User");
        return false;
    }
    return true;
}

function CourseValidation() {
    var returnStatus = false;
    if ($("#ContentModuleName").val() == "") {
        alert("Please enter Course Name");
        $("#ContentModuleName").focus();
        returnStatus= false;
    }

    if ($("#ContentModuleURL").val() == "") {
        if ($("#file").val() == "") {
            alert("Please upload Course");
            returnStatus= false;
        }
        else {
            if ($("#CourseTypeList :selected").text() == "Scorm1.2") {
                var index = $("#file").val().indexOf(".zip");
                if (index <= 0) {
                    alert("Please upload ZIP file only");
                    returnStatus= false;
                }
            }
            else {
                var status = false;
                var index = $("#file").val().indexOf(".pdf");
                if (index >= 0) {
                    //alert("Please upload .PDF, .PPT, .PPS, .DOC, .DOCX, .XLS, .XLSX files only");
                    returnStatus = true;
                }
                index = $("#file").val().indexOf(".ppt");
                if (index >= 0) {                    
                    returnStatus = true;
                }
                index = $("#file").val().indexOf(".pps");
                if (index >= 0) {
                    returnStatus = true;
                }
                index = $("#file").val().indexOf(".doc");
                if (index >= 0) {
                    returnStatus = true;
                }               
                index = $("#file").val().indexOf(".xls");
                if (index >= 0) {
                    returnStatus = true;
                }
                index = $("#file").val().indexOf(".mp4");
                if (index >= 0) {
                    returnStatus = true;
                }
                if (!returnStatus) {
                    alert("Please upload .PDF, .PPT, .PPS, .DOC, .DOCX, .XLS, .XLSX, .MP4 files only");
                    return false;
                }
            }
        }
    }
    if (returnStatus) {
        

        $("#divLoading").show();
         
        return true;
    }
    else {
        return false;
    }
}

function UserUploadValidation() {

    if ($("#file").val() == "") {
        alert("Please select file");
        return false;
    }
    else {
        var index = $("#file").val().indexOf(".csv");
        if (index <= 0) {
            alert("Please upload Csv file only");
            return false;
        }
    }

    return true;
}

function CreateForumValidation() {

}