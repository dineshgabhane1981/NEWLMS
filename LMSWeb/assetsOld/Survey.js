var VideoAudioLink = "http://localhost:7777/QuizMedia/";
var count = 9999999;
var optionCount = 2;
var questionObj = [];
var base64StringArray = [];
var statisQueId = 0;
var quizQueIds = [];
var currentIndex = 0;

$(document).ready(function () {
    if ($('#hdnEditData').val() != null && $('#hdnEditData').val() != "") {   
        SetEditData(JSON.parse($("#hdnEditData").val()));        
    }
    
    $('#btnAddQuestion').on("click", function () {
        AddQuestion();
        $("#btnSaveQuestion").show();
    });

    $('#btnSaveQuestion').on("click", function () {
        var returnStatus = SaveQuestion();
        if (returnStatus) {
            //swal.fire("Question Added Successfully !!!");
            Swal.fire({
                icon: 'success',
                title: 'Saved',
                text: 'Question Added Successfully !!!'
            })
            $("#btnSaveQuestion").hide();
            $("#questionDetails").empty();
            DrawTable();
        }
    });

    $("#btnPreview").on("click", function () {
        if ($("#hdnData").val() != "" && $("#hdnData").val() != "[]") {
            console.log($("#hdnData").val());
            $("#dvQuizDetails").hide(1000);
            $("#dvQuizQuestions").hide(1000);
            $("#dvPreviewQuiz").show(1000);
            $("#dvPublishQuiz").hide();
            $("#stepPreview").addClass("step-active");
            //View Quiz for Admin
            var QuizViewData = JSON.parse($("#hdnData").val());
            console.log(QuizViewData);
            //$("#hdnViewData").val("");
            ViewQuiz(QuizViewData);
        }
        else {
            Swal.fire({
                icon: 'warning',
                title: 'Oops...',
                text: 'Please add Questions!'
            })
            return false;
        }
        

    });
    
});

function AddQuestion() {
    statisQueId = 0;
    $("#questionDetails").empty();
    console.log(count)
    count++;
    optionCount = 2;
    var queHTML = "<div class=\"que-containerNew container-fluid mt-4 mb-4 pl-4 pr-4 pt-4 pb-4\" id=queContainer" + count + ">";

    queHTML += "<div class=\"row col-12\" id=dvQues" + count + ">";
    queHTML += "<div class=\"row col-12\" style=margin-top:5px;>";
    queHTML += "<label class=\"col-2 p-0\">Question Type </label>";
    queHTML += "<select onchange=\"ChangeType(" + count + ")\" id=queType" + count + " class=\"col-4 ml-0 form-control\"><option value=\"1\" >Single Select</option><option value=\"2\">Multiple Select</option><option value=\"3\">Paragraph</option><option value=\"4\">Video</option><option value=\"5\">Audio</option></select>";
    queHTML += "<div id=dvRandom class=\"col-3\">";
    queHTML += "&nbsp;<input type=\"checkbox\" id=chkIsRandom" + count + " >";
    queHTML += "&nbsp;<b>Random Options</b>";
    queHTML += "</div>";

    queHTML += "<div class=\"col-3 p-0\" >";
    queHTML += "<input  type=text class=\"col-3 ml-0 form-control\" style=\"float:left;\"  id=que" + count + "points></input> <b>&nbsp;&nbsp;pts</b>";
    queHTML += "</div>";

    queHTML += "</div>";

    queHTML += "<div class=\"row col-12 mb-4\" style=margin-top:5px;>";
    queHTML += "<label class=\"col-2 p-0\">Question Text </label>";
    queHTML += "<div class=\"col-9 p-0\">";
    queHTML += "<textarea class=\"col-6 ml-0 form-control\" id=que" + count + "></textarea>";
    queHTML += "</div>";
    queHTML += "</div>";

    queHTML += "<div class=\"row col-12\" id=dvQue" + count + "Media>";
    queHTML += "</div>";

    queHTML += "<div class=\"row col-12\" id=dvQue" + count + "Options>";
    queHTML += "<label class=\"col-2 p-0 \">Options </label>";
    queHTML += "<div class=\"row col-12\">";
    queHTML += "<label class=\"col-2 p-0\"></label>";
    queHTML += "<input type=\"radio\" class=\"radio-margin\" value=1 name=Options" + count + " id=que" + count + "rbtnOption1 name=que" + count + "rbtnOption1 /><input type=text class=\"col-6 ml-0 form-control\"   id=que" + count + "optionText1></input>";
    queHTML += "</div>";

    queHTML += "<div class=\"row col-12\" style=margin-top:5px; id=que" + count + "option1FeedbackDiv  >";
    queHTML += "<div class=\"col-9 offset-2 p-0\">";
    queHTML += "<label class=\"col-3 p-0\" style=\"cursor:pointer; color:blue; \" data-toggle=\"collapse\" data-target=#option1Feedback><a>Option Feedback</a></label>";
    queHTML += "<div id=option1Feedback class=\"collapse in\">";
    queHTML += "<textarea class=\"col-6 ml-0 form-control\"   id=que" + count + "option1Feedback></textarea>";
    queHTML += "</div>";
    queHTML += "</div>";
    queHTML += "</div>";

    queHTML += "<div class=\"row col-12\" style=margin-top:5px;>";
    queHTML += "<label class=\"col-2 p-0\"></label>";
    queHTML += "<input type=\"radio\" class=\"radio-margin\" value=2 name=Options" + count + " id=que" + count + "rbtnOption2 name=que" + count + "rbtnOption2 /><input type=text class=\"col-6 ml-0 form-control\"   id=que" + count + "optionText2></input>";
    queHTML += "<button onclick=\"addOption(" + count + ")\" id=que" + count + "btnOption2 type=\"button\" class=\"col question-btn-for-option btn text-center btn-primary profle-submit-btn-for-option\" style=\"margin-left: 5px;\"> + </button>";
    queHTML += "</div>";

    queHTML += "<div class=\"row col-12\" style=margin-top:5px;\" id=que" + count + "option2FeedbackDiv >";
    queHTML += "<div class=\"col-9 offset-2 p-0\">";
    queHTML += "<label class=\"col-3 p-0\" style=\"cursor:pointer; color:blue; \" data-toggle=\"collapse\" data-target=#option2Feedback><a>Option Feedback</a></label>";
    queHTML += "<div id=option2Feedback class=\"collapse in\">";
    queHTML += "<textarea class=\"col-6 ml-0 form-control\"   id=que" + count + "option2Feedback></textarea>";
    queHTML += "</div>";
    queHTML += "</div>";
    queHTML += "</div>";
    queHTML += "</div>";

    queHTML += "<div class=\"row col-12\" style=margin-top:5px;\" id=que" + count + "CorrectFeedbackDiv>";
    queHTML += "<div class=\"col-9 offset-2 p-0\">";
    queHTML += "<label class=\"col-6 p-0\" style=\"cursor:pointer; color:blue; \" data-toggle=\"collapse\" data-target=#OptionCorrectFeedbackDiv>Feedback if correctly answered</label>";
    queHTML += "<div id=OptionCorrectFeedbackDiv class=\"collapse in\">";
    queHTML += "<textarea class=\"col-6 ml-0 form-control\"   id=que" + count + "CorrectFeedback></textarea>";
    queHTML += "</div>";
    queHTML += "</div>";
    queHTML += "</div>";

    queHTML += "<div class=\"row col-12\" style=margin-top:5px;\" id=que" + count + "InCorrectFeedbackDiv>";
    queHTML += "<div class=\"col-9 offset-2 p-0\">";
    queHTML += "<label class=\"col-6 p-0\" style=\"cursor:pointer; color:blue; \" data-toggle=\"collapse\" data-target=#OptionInCorrectFeedbackDiv>Feedback if incorrectly answered</label>";
    queHTML += "<div id=OptionInCorrectFeedbackDiv class=\"collapse in\">";
    queHTML += "<textarea class=\"col-6 ml-0 form-control\"   id=que" + count + "InCorrectFeedback></textarea>";
    queHTML += "</div>";
    queHTML += "</div>";
    queHTML += "</div>";

    queHTML += "</div>";
    queHTML += "</div>";

    $('#questionDetails').append(queHTML);

    $('#que' + count).summernote();
    $('#que' + count + 'option1Feedback').summernote();
    $('#que' + count + 'option2Feedback').summernote();

    $('#que' + count + 'CorrectFeedback').summernote();
    $('#que' + count + 'InCorrectFeedback').summernote();

    $('#que' + count + 'CorrectFeedbackDiv').hide();
    $('#que' + count + 'InCorrectFeedbackDiv').hide();

}

function editQuestion(queId) {
    statisQueId = queId;
    $("#questionDetails").empty();
    var item = questionObj.find(x => x.QuestionId == queId);
    //console.log(item);

    var queHTML = "<div class=\"que-containerNew container-fluid mt-4 mb-4 pl-4 pr-4 pt-4 pb-4\" id=queContainer" + item.QuestionId + ">";

    queHTML += "<div class=\"row col-12\" id=dvQues" + item.QuestionId + ">";
    queHTML += "<div class=\"row col-12\" style=\"margin-top:5px;\">";
    queHTML += "<label class=\"col-2 p-0\">Question Type </label>";
    queHTML += "<select onchange=\"ChangeType(" + item.QuestionId + ")\" id=queType" + item.QuestionId + " class=\"col-4 ml-0 form-control\"><option value=\"1\" >Single Select</option><option value=\"2\">Multiple Select</option><option value=\"3\">Paragraph</option><option value=\"4\">Video</option><option value=\"5\">Audio</option></select>";

    if (item.QuestionTypeId == 1 || item.QuestionTypeId == 2) {
        //queHTML += "&nbsp;&nbsp;&nbsp;<input type=\"checkbox\" id=chkIsRandom" + item.QuestionId + ">";
        //queHTML += "&nbsp;<b>Random Options</b>";

        queHTML += "<div id=dvRandom class=\"col-3\">";
        queHTML += "&nbsp;<input type=\"checkbox\" id=chkIsRandom" + item.QuestionId + " >";
        queHTML += "&nbsp;<b>Random Options</b>";
        queHTML += "</div>";
    }
    queHTML += "<div class=\"col-3 p-0\" >";
    queHTML += "<input  type=text class=\"col-3 ml-0 form-control\" style=\"float:left;\"  id=que" + count + "points></input> <b>&nbsp;&nbsp;pts</b>";
    queHTML += "</div>";

    queHTML += "</div>";

    queHTML += "<div class=\"row col-12\" style=\"margin-top:5px;\">";
    queHTML += "<label class=\"col-2 p-0 \">Question Text </label>";
    queHTML += "<div class=\"col-9 p-0\">";
    queHTML += "<textarea class=\"col-6 ml-0 form-control\" id=que" + item.QuestionId + "></textarea>";
    queHTML += "</div>";
    queHTML += "</div>";
    if (item.QuestionTypeId == 1 || item.QuestionTypeId == 2) {
        queHTML += "<label class=\"col-2 p-0 hello\">Options </label>";
    }

    queHTML += "<div class=\"row col-12\" style=\"margin-top:5px;\" id=dvQue" + item.QuestionId + "Options>";
    $.each(item.Options, function (indexOption, valueOption) {

        queHTML += "<div class=\"row col-12\" style=\"margin-top:5px;\" id=dvQue" + item.QuestionId + "Option" + valueOption.OptionId + ">";
        queHTML += "<label class=\"col-2 p-0\"></label>";
        if (indexOption == 0) {
            if (item.QuestionTypeId == 1) {
                queHTML += "<input type=\"radio\" class=\"radio-margin\" value=1 name=Options" + item.QuestionId + " id=que" + item.QuestionId + "rbtnOption" + valueOption.OptionId + " name=que" + item.QuestionId + "rbtnOption" + valueOption.OptionId + " /><input type=text class=\"col-6 ml-0 form-control\"   id=que" + item.QuestionId + "optionText" + valueOption.OptionId + "></input>";
            }
            else {
                queHTML += "<input type=\"checkbox\" class=\"radio-margin\" value=1 name=Options" + item.QuestionId + " id=que" + item.QuestionId + "rbtnOption" + valueOption.OptionId + " name=que" + item.QuestionId + "rbtnOption" + valueOption.OptionId + " /><input type=text class=\"col-6 ml-0 form-control\"   id=que" + item.QuestionId + "optionText" + valueOption.OptionId + "></input>";
            }
        }
        if (indexOption == 1) {
            if (item.QuestionTypeId == 1) {
                queHTML += "<input type=\"radio\" class=\"radio-margin\" value=1 name=Options" + item.QuestionId + " id=que" + item.QuestionId + "rbtnOption" + valueOption.OptionId + " name=que" + item.QuestionId + "rbtnOption" + valueOption.OptionId + " /><input type=text class=\"col-6 ml-0 form-control\"   id=que" + item.QuestionId + "optionText" + valueOption.OptionId + "></input>";
                queHTML += "<button onclick=\"addOption(" + item.QuestionId + ")\" id=que" + item.QuestionId + "btnOption" + valueOption.OptionId + " type=\"button\" class=\"col question-btn-for-option text-center btn-primary profle-submit-btn-for-option\" style=\"margin-left: 5px;\"> + </button>";
            }
            else {
                queHTML += "<input type=\"checkbox\" class=\"radio-margin\" value=1 name=Options" + item.QuestionId + " id=que" + item.QuestionId + "rbtnOption" + valueOption.OptionId + " name=que" + item.QuestionId + "rbtnOption" + valueOption.OptionId + " /><input type=text class=\"col-6 ml-0 form-control\"   id=que" + item.QuestionId + "optionText" + valueOption.OptionId + "></input>";
                queHTML += "<button onclick=\"addOption(" + item.QuestionId + ")\" id=que" + item.QuestionId + "btnOption" + valueOption.OptionId + " type=\"button\" class=\"col question-btn-for-option btn text-center btn-primary profle-submit-btn-for-option\" style=\"margin-left: 5px;\"> + </button>";
            }
        }
        if (indexOption > 1) {
            if (item.QuestionTypeId == 1) {
                queHTML += "<input type=\"radio\" class=\"radio-margin\" value=1 name=Options" + item.QuestionId + " id=que" + item.QuestionId + "rbtnOption" + valueOption.OptionId + " name=que" + item.QuestionId + "rbtnOption" + valueOption.OptionId + " /><input type=text class=\"col-6 ml-0 form-control\"   id=que" + item.QuestionId + "optionText" + valueOption.OptionId + "></input>";
                queHTML += "<button onclick=\"addOption(" + item.QuestionId + ")\" id=que" + item.QuestionId + "btnOption" + valueOption.OptionId + " type=\"button\" class=\"col question-btn-for-option btn text-center btn-primary profle-submit-btn-for-option\" style=\"margin-left: 5px;\"> + </button>";
                queHTML += "<button onclick=\"removeOption(" + item.QuestionId + "," + valueOption.OptionId + ")\" id=que" + item.QuestionId + "btnOption" + valueOption.OptionId + " type=\"button\" class=\"col question-btn-for-option btn text-center btn-primary profle-submit-btn-for-option\" style=\"margin-left: 5px;\"> - </button>";
            }
            else {
                queHTML += "<input type=\"checkbox\" class=\"radio-margin\" value=1 name=Options" + item.QuestionId + " id=que" + item.QuestionId + "rbtnOption" + valueOption.OptionId + " name=que" + item.QuestionId + "rbtnOption" + valueOption.OptionId + " /><input type=text class=\"col-6 ml-0 form-control\"   id=que" + item.QuestionId + "optionText" + valueOption.OptionId + "></input>";
                queHTML += "<button onclick=\"addOption(" + item.QuestionId + ")\" id=que" + item.QuestionId + "btnOption" + valueOption.OptionId + " type=\"button\" class=\"col question-btn-for-option btn text-center btn-primary profle-submit-btn-for-option\" style=\"margin-left: 5px;\"> + </button>";
                queHTML += "<button onclick=\"removeOption(" + item.QuestionId + "," + valueOption.OptionId + ")\" id=que" + item.QuestionId + "btnOption" + valueOption.OptionId + " type=\"button\" class=\"col question-btn-for-option btn text-center btn-primary profle-submit-btn-for-option\" style=\"margin-left: 5px;\"> - </button>";
            }
        }

        queHTML += "<div class=\"row col-12\" style=\"margin-top:5px;\" id=que" + item.QuestionId + "option" + valueOption.OptionId + "FeedbackDiv>";
        queHTML += "<div class=\"col-9 offset-2 p-0\" >";
        queHTML += "<label class=\"col-3 p-0\" style=\"cursor:pointer; color:blue; \" data-toggle=\"collapse\" data-target=#option" + valueOption.OptionId + "Feedback>Option Feedback</label>";
        queHTML += "<div id=\"option" + valueOption.OptionId + "Feedback\" class=\"collapse in\">";
        queHTML += "<textarea class=\"col-6 ml-0 form-control\"   id=que" + item.QuestionId + "option" + valueOption.OptionId + "Feedback></textarea>";
        queHTML += "</div>";
        queHTML += "</div>";

        queHTML += "</div>";
        queHTML += "</div>";
    });
    queHTML += "</div>";
    
    if (item.QuestionTypeId == 4 || item.QuestionTypeId == 5) {
        var type = "video/*";
        if (item.QuestionTypeId == 5) {
            type = "audio/*";
        }
        queHTML += "<input type=\"file\" name=\"file" + item.QuestionId + "\" id=\"file" + item.QuestionId + "\" style=\"width:100%;\" accept=" + type + " />";

        queHTML += "<span><b>" + item.MediaType + "</b></span>";
    }
    queHTML += "<div class=\"row col-12\" style=\"margin-top:5px;\" id=que" + item.QuestionId + "CorrectFeedbackDiv>";
    queHTML += "<div class=\"col-9 offset-3 p-0\">";
    queHTML += "<label class=\"col-6 p-0\">Feedback if correctly answered</label>";
    queHTML += "<textarea class=\"col-6 ml-0 form-control\"   id=que" + item.QuestionId + "CorrectFeedback></textarea>";
    queHTML += "</div>";
    queHTML += "</div>";

    queHTML += "<div class=\"row col-12\" style=\"margin-top:5px;\" id=que" + item.QuestionId + "InCorrectFeedbackDiv>";
    queHTML += "<div class=\"col-9 offset-3 p-0\">";
    queHTML += "<label class=\"col-6 p-0\">Feedback if incorrectly answered</label>";
    queHTML += "<textarea class=\"col-6 ml-0 form-control\"   id=que" + item.QuestionId + "InCorrectFeedback></textarea>";
    queHTML += "</div>";
    queHTML += "</div>";

    queHTML += "</div>";
    queHTML += "</div>";

    queHTML += "</div>";
    queHTML += "</div>";
    $('#questionDetails').append(queHTML);
    //console.log(queHTML);
    $("#btnSaveQuestion").show();

    $('#queType' + item.QuestionId).val(item.QuestionTypeId);
    if (item.isRandomOption)
        $('#chkIsRandom' + item.QuestionId).attr('checked', 'checked');

    $('#que' + item.QuestionId).val(item.QuestionText);
    $('#que' + item.QuestionId + "points").val(item.QuestionPoints);
    $('#que' + item.QuestionId).summernote();

    $('#que' + item.QuestionId + 'CorrectFeedback').val(item.CorrectFeedback);
    $('#que' + item.QuestionId + 'CorrectFeedback').summernote();
    $('#que' + item.QuestionId + 'InCorrectFeedback').val(item.InCorrectFeedback);
    $('#que' + item.QuestionId + 'InCorrectFeedback').summernote();
    if (item.QuestionTypeId == 1 || item.QuestionTypeId == 3 || item.QuestionTypeId == 4 || item.QuestionTypeId == 5) {
        $('#que' + item.QuestionId + 'CorrectFeedbackDiv').hide();
        $('#que' + item.QuestionId + 'InCorrectFeedbackDiv').hide();
    }
    $.each(item.Options, function (indexOption, valueOption) {
        $('#que' + item.QuestionId + 'optionText' + valueOption.OptionId).val(valueOption.OptionText);

        if (valueOption.CorrectOption)
            $("#que" + item.QuestionId + "rbtnOption" + valueOption.OptionId).attr('checked', 'checked');

        if (item.QuestionTypeId == 1) {
            $('#que' + item.QuestionId + 'option' + valueOption.OptionId + 'Feedback').val(valueOption.OptionFeedback);
            $('#que' + item.QuestionId + 'option' + valueOption.OptionId + 'Feedback').summernote();

            $('#que' + item.QuestionId + 'option' + valueOption.OptionId + 'FeedbackDiv').show();
        }
        else {
            $('#que' + item.QuestionId + 'option' + valueOption.OptionId + 'Feedback').summernote();
            $('#que' + item.QuestionId + 'option' + valueOption.OptionId + 'FeedbackDiv').hide(); //que1042option1076FeedbackDiv
        }
        optionCount = valueOption.OptionId;
    });

    //count = item.QuestionId;

    item1 = {}
    item1["QId"] = item.QuestionId;
    item1["mediaFile"] = item.MediaURL;
    item1["qTypeId"] = item.MediaType;
    base64StringArray.push(item1);
    if (item.QuestionTypeId == 4 || item.QuestionTypeId == 5) {        
        document.getElementById("file" + item.QuestionId).addEventListener('change', handleFileSelect, false);
    }


    //console.log(item);
}

function addOption(queCount) {
    var selectedType = $("#queType" + queCount + " option:selected").val();
    optionCount++;
    var newHTML = "<div class=\"row col-12\" id=dvQue" + queCount + "Option" + optionCount + " style=margin-top:5px;>";
    newHTML += "<label class=\"col-2 p-0\"></label>";

    if (selectedType == 1) {
        newHTML += "<input type=\"radio\" class=\"radio-margin\" name=Options" + queCount + " id=que" + queCount + "rbtnOption" + optionCount + " name=que" + queCount + "rbtnOption" + optionCount + " /><input type=text class=\"col-6 ml-0 form-control\"  id=que" + queCount + "optionText" + optionCount + "></input>";
    }
    else {
        newHTML += "<input type=\"checkbox\" class=\"radio-margin\"  name=Options" + queCount + " id=que" + queCount + "rbtnOption" + optionCount + " name=que" + queCount + "rbtnOption" + optionCount + " /><input type=text class=\"col-6 ml-0 form-control\"  id=que" + queCount + "optionText" + optionCount + "></input>";
    }
    newHTML += "<button onclick=\"addOption(" + queCount + ")\" id=que" + queCount + "btnOption" + optionCount + " type=\"button\" class=\"col question-btn-for-option btn text-center btn-primary profle-submit-btn-for-option\" style=\"margin-left: 5px; \"> + </button>";
    newHTML += "<button onclick=\"removeOption(" + queCount + "," + optionCount + ")\" id=que" + queCount + "btnOption" + optionCount + " type=\"button\" class=\"col question-btn-for-option btn text-center btn-primary profle-submit-btn-for-option\" style=\"margin-left: 5px;\"> - </button>";

    newHTML += "<div class=\"col-9 offset-2 p-0\" style=margin-top:5px; id=que" + queCount + "option" + optionCount + "FeedbackDiv>";
    newHTML += "<label class=\"col-3 p-0\" style=\"cursor:pointer; color:blue; \" data-toggle=\"collapse\" data-target=#option" + optionCount + "Feedback><a>Option Feedback</a></label>";
    newHTML += "<div id=option" + optionCount + "Feedback class=\"collapse in\">";
    newHTML += "<textarea class=\"col-6 ml-0 form-control\"   id=que" + queCount + "option" + optionCount + "Feedback></textarea>";
    newHTML += "</div>";
    newHTML += "</div>";

    newHTML += "</div>";
    $("#dvQue" + queCount + "Options").append(newHTML);
    $('#que' + queCount + 'option' + optionCount + 'Feedback').summernote().addClass('col-9');

    if (selectedType == 2) {
        $('#que' + count + 'option' + optionCount + 'FeedbackDiv').hide();
        //$('#que' + count + 'option2Feedback').summernote();
    }
}

function removeOption(queCount, optionCountToRemove) {
    $("#dvQue" + queCount + "Option" + optionCountToRemove).remove();
}

function deleteQuestion(queCount) {    
    Swal.fire({
        title: 'Are you sure?',
        text: "You want to delete this Question!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            //questionObj
            questionObj = questionObj.filter(function (item) {
                return item.QuestionId != queCount;
            })
            //$("#" + queCount).remove();
            Swal.fire(
                'Deleted!',
                'Question has been deleted.',
                'success'
            )
            DrawTable();
            console.log(questionObj)
            $("#hdnData").val(JSON.stringify(questionObj));
        }
    })

}

function ChangeType(id) {

    var selectedType = $("#queType" + id + " option:selected").val();
    var IDs = $("#dvQue" + id + "Options input[id^='que" + id + "rbtn']");
    //console.log(IDs);

    $.each(IDs, function (index, value) {
        if (selectedType == 3) {
            //dvQue1Options
            $("#dvQue" + id + "Media").hide();
            $("#dvQue" + id + "Options").hide();
            $('#que' + id + 'CorrectFeedbackDiv').hide();
            $('#que' + id + 'InCorrectFeedbackDiv').hide();
            $('#dvRandom').hide();
        }
        if (selectedType == 2) {
            $("#dvQue" + id + "Media").hide();
            $("#dvQue" + id + "Options").show();
            $('#dvRandom').show();
            $('#' + value.id).get(0).type = 'checkbox';

            var optionIndex = value.id.indexOf("Option");
            var actualOptionId = value.id.substring(optionIndex + 6, value.id.length);
            //console.log('que' + id + 'option' + actualOptionId + 'FeedbackDiv')
            //que1option2FeedbackDiv
            $('#que' + id + 'option' + actualOptionId + 'FeedbackDiv').hide();
            $('#que' + id + 'CorrectFeedbackDiv').show();
            $('#que' + id + 'InCorrectFeedbackDiv').show();
        }
        if (selectedType == 1) {
            $("#dvQue" + id + "Media").hide();
            $("#dvQue" + id + "Options").show();
            $('#dvRandom').show();
            $('#' + value.id).get(0).type = 'radio';
            var optionIndex = value.id.indexOf("Option");
            var actualOptionId = value.id.substring(optionIndex + 6, value.id.length);
            $('#que' + id + 'option' + actualOptionId + 'FeedbackDiv').show();

            $('#que' + id + 'CorrectFeedbackDiv').hide();
            $('#que' + id + 'InCorrectFeedbackDiv').hide();
        }
        if (selectedType == 4 || selectedType == 5) {
            $("#dvQue" + id + "Media").show();
            $("#dvQue" + id + "Media").empty();
            $("#dvQue" + id + "Options").hide();
            $('#que' + id + 'CorrectFeedbackDiv').hide();
            $('#que' + id + 'InCorrectFeedbackDiv').hide();
            $('#dvRandom').hide();
            var type = "video/*";
            if (selectedType == 5) {
                type = "audio/*";
            }

            var fileHTML = "<input type=\"file\" name=\"file" + id + "\" id=\"file" + id + "\" style=\"width:100%;\" accept=" + type + " />";

            $("#dvQue" + id + "Media").append(fileHTML);
            document.getElementById("file" + id).addEventListener('change', handleFileSelect, false);

        }
    });

}
function handleFileSelect(evt) {
    var id = evt.currentTarget.id;
    var qId = id.substring(4, id.length);
    var f = evt.target.files[0]; // FileList object    
    var reader = new FileReader();
    // Closure to capture the file information.    
    reader.onload = (function (theFile) {
        return function (e) {
            var binaryData = e.target.result;
            //Converting Binary Data to base 64    

            var base64String = window.btoa(binaryData);
            item = {}
            item["QId"] = qId;
            item["mediaFile"] = base64String;
            item["qTypeId"] = $("#file" + qId)[0].files[0].name;
            base64StringArray.push(item);

            alert('File converted to base64 successfuly!');
        };
    })(f);
    // Read in the image file as a data URL.    
    reader.readAsBinaryString(f);
}  

function SaveQuestion() {
    var returnStatus = true;
    var IDs = $("#questionDetails div[id^='dvQues']");
    if (IDs.length == 0) {
       
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Please add Question!'
        })
        returnStatus = false;
        return false;
    }
    //console.log(IDs.length)

    $.each(IDs, function (index, value) {
        console.log(value)
        console.log(value.id)
        var id = value.id.substring(6, value.id.length);
        console.log(id)

        if ($("#que" + id).val() == null || $("#que" + id).val() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Please enter Question Text!'                
            })

            returnStatus = false;
            return false;
        }
        item = {}
        item["QuestionId"] = id;
        item["QuestionTypeId"] = $("#queType" + id + " option:selected").val();
        item["QuestionText"] = $("#que" + id).val();
        item["isRandomOption"] = $("#chkIsRandom" + id).is(":checked");
        item["mediaFile"] = "";
        item["qTypeId"] = "";
        item["QuestionPoints"] = $("#que" + id + "points").val();

        //Swal.fire($("#que" + id + "points").val());

        if ($("#queType" + id + " option:selected").val() == 2) {
           
            item["CorrectFeedback"] = $("#que" + id + "CorrectFeedback").val();//que2CorrectFeedback
            item["InCorrectFeedback"] = $("#que" + id + "InCorrectFeedback").val();
        }
        else {
            item["CorrectFeedback"] = "";
            item["InCorrectFeedback"] = "";
        }
        if ($("#queType" + id + " option:selected").val() == 1 || $("#queType" + id + " option:selected").val() == 2) {
            var OptionCheck = false;
            var optionIDs = $("#dvQue" + id + "Options input[id^='que" + id + "rbtnOption']"); //que1rbtnOption1. dvQue1Options
            var optionObj = [];
            $.each(optionIDs, function (index, value) {
                var indexId = value.id.indexOf("Option");//dvQue1021Option1026

                var optionId = value.id.substring((indexId + 6), value.id.length);

                optionItem = {}
                console.log($('#' + value.id).is(':checked'));
                if ($('#' + value.id).is(':checked')) {
                    console.log("111--  " + value.id);
                    OptionCheck = true;
                }
                if ($('#que' + id + 'optionText' + optionId).val() == null || $('#que' + id + 'optionText' + optionId).val() == "") {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Please enter Option Text!'
                    })
                   
                    returnStatus = false;
                    return false;
                }
                optionItem["OptionId"] = optionId;
                optionItem["CorrectOption"] = $('#' + value.id).is(':checked');
                optionItem["OptionText"] = $('#que' + id + 'optionText' + optionId).val();
                if ($("#queType" + id + " option:selected").val() == 1) {
                    optionItem["OptionFeedback"] = $('#que' + id + 'option' + optionId + 'Feedback').val();
                }
                else {
                    optionItem["OptionFeedback"] = "";
                }
                //console.log($('#que' + id + 'option' + optionId + 'Feedback').val())
                optionObj.push(optionItem);
            });

            if (!OptionCheck) {
                returnStatus = false;
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Please check Correct Answer!'
                })
                //alert("Please check Correct Answer");
                return false;
            }
            item["Options"] = optionObj;
        }
        if ($("#queType" + id + " option:selected").val() == 4) {
            $.each(base64StringArray, function (index, value) {
                if (value.QId == id) {
                    item["mediaFile"] = value.mediaFile;
                    item["qTypeId"] = value.qTypeId;
                }
            });
        }
        if ($("#queType" + id + " option:selected").val() == 5) {
            $.each(base64StringArray, function (index, value) {
                if (value.QId == id) {
                    item["mediaFile"] = value.mediaFile;
                    item["qTypeId"] = value.qTypeId;
                }
            });
        }
        var sorted = SortedData();
        if (statisQueId == 0) { 
            
            questionObj.push(item);          
            console.log(questionObj);
        }
        else {
            index = questionObj.findIndex(x => x.QuestionId == statisQueId);
            questionObj[index] = item;
            statisQueId = 0;
        }
    });
    $("#hdnData").val(JSON.stringify(questionObj));
    //console.log(questionObj);
    //return false;
    return returnStatus;
}

function DrawTable() {
    var arrayOfQuestions = [];
    var srNo = 0;
    $.map(questionObj, function (item) {
        srNo++;
        optionItem = {}
        optionItem["index"] = srNo;
        optionItem["QuestionTitle"] = item.QuestionText.substring(0,20);
        //optionItem["DueDate"] = item.DueDate;
        optionItem["QuestionId"] = item.QuestionId;
        optionItem["delete"] = "<div style=\"float:right; cursor:pointer;\"  onclick=\"deleteQuestion(" + item.QuestionId + ")\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i> &nbsp;&nbsp;</div>";
        optionItem["delete"] += "<div style=\"float:right; cursor:pointer;\" onclick=\"editQuestion(" + item.QuestionId + ")\"><i class=\"fa fa-pencil-square-o\" aria-hidden=\"true\"></i>&nbsp;&nbsp;</div>"
        arrayOfQuestions.push(optionItem);
    });
    
    var table1 = $('#tblQuestions').DataTable();
    table1.destroy();

    var table = $('#tblQuestions').DataTable({
        "data": arrayOfQuestions,
        "paging": false,
        "responsive": true,
        "columns": [
            { data: "index" },
            { data: "QuestionTitle" },            
            { data: "QuestionId" },
            { data: "delete" }
            
        ],
        "aoColumnDefs": [{ "sClass": "hide_me", "aTargets": [2] }],
        "rowReorder": {
            dataSrc: "index"
        }
    });
    $('#tblQuestions_filter').hide();
}

function SetEditData(editJSON) {    
    questionObj = editJSON.TblQuestions;
    console.log(questionObj);
    $.each(questionObj, function (index, value) {
        optionObj = {}
        //optionObj["Options"] = value.TblQuestionOptions;
        value["Options"] = value.TblQuestionOptions;
        value["qTypeId"] = value.MediaType;
        value["mediaFile"] = "";
    });
    
    console.log(questionObj);
    $("#hdnData").val(JSON.stringify(questionObj));
    DrawTable();
}

function ViewQuiz(QuizViewData) {
    //Reorder actual Questions Array      
    QuizViewData = SortedData();
    $("#hdnData").val(JSON.stringify(questionObj));
    //Reorder ENDED
    
    //Reset and Unbind events
    $('#btnNext').unbind();
    $('#btnPrev').unbind();
    quizQueIds = [];
    currentIndex = 0;
    $('#paginator-list').empty();
    $('#dvQuestions').empty();    
    $('#btnPrev').hide();
    $('#btnResponseSubmit').hide();
    //Reset and Unbind events ENDED

    var queHTML = "<div class=\"user-quiz-container container-fluid\">";
    $.each(QuizViewData, function (index, value) {
        item = {}
        item["QuestionId"] = value.QuestionId;

        quizQueIds.push(item);
        queHTML += "<div class=\"que-container row mt-4 pt-2 pb-2\" id=dvQue" + value.QuestionId + ">";
        queHTML += "<div class=\"col-12 que-text font-weight-bold\" >";
        queHTML += "<label>" + value.QuestionText + " </label>";
        queHTML += "</div>";
        //queHTML += "<div class=\"col-2 blue-color text-right\">Weightage Point: 1</div>";
        queHTML += "<div class=\"col-12 mt-3\">";
        if (value.QuestionTypeId == 1 || value.QuestionTypeId == 2) {
            queHTML += "<h6 class=\"font-weight-bold\">Options</h6>";
        }
        queHTML += "<ul class=\"option-list list-unstyled\">";

        if (value.QuestionTypeId == 1) {
            $.each(value.Options, function (indexOption, valueOption) {
                queHTML += "<li>";
                queHTML += "<label>";
                queHTML += "<input type=\"radio\" value=1 name=Options" + value.QuestionId + " id=que" + value.QuestionId + "rbtnOption" + valueOption.OptionId + " name=que" + value.QuestionId + "rbtnOption" + valueOption.OptionId + " />";
                queHTML += "<span class=\"ml-2\"> " + valueOption.OptionText + " </span>";
                queHTML += "</label>";
                queHTML += "</li>";

            });
        }
        if (value.QuestionTypeId == 2) {
            $.each(value.Options, function (indexOption, valueOption) {
                queHTML += "<li>";
                queHTML += "<label>";
                queHTML += "<input type=\"checkbox\" value=1 name=Options" + value.QuestionId + " id=que" + value.QuestionId + "rbtnOption" + valueOption.OptionId + " name=que" + value.QuestionId + "rbtnOption" + valueOption.OptionId + " />";
                queHTML += "<span class=\"ml-2\"> " + valueOption.OptionText + " </span>";
                queHTML += "</label>";
                queHTML += "</li>";
            });
        }
        if (value.QuestionTypeId == 3) {
            queHTML += "<li>";
            queHTML += "<textarea class=\"form-control\" id=que" + value.QuestionId + "ParaOption  name=que" + value.QuestionId + "ParaOption rows=\"3\" placeholder=\"Write your answer here\" ></textarea>"
            queHTML += "</li>";
        }
        if (value.QuestionTypeId == 4) {
            var source = VideoAudioLink + value.QuestionId + ".mp4";
            queHTML += "<li>";
            queHTML += "<video src=" + source + " width=\"90%\" controls=\"controls\" controlsList=\"nodownload\" oncontextmenu=\"return false;\" />";
            queHTML += "</li>";

        }
        if (value.QuestionTypeId == 5) {
            var source = VideoAudioLink + value.QuestionId + ".mp3";
            queHTML += "<li>";
            queHTML += "<audio controls=\"controls\" width=\"90%\" controlsList=\"nodownload\" oncontextmenu=\"return false; \">";
            queHTML += "<source src=" + source + " type=\"audio/mpeg\" /></audio>";
            queHTML += "</li>";

        }
        queHTML += "</ul>";
        queHTML += "</div>";
        queHTML += "</div>";

        // append paginator list
        const paginatorHtml = " <li class=\"list-inline-item\" data-queid='dvQue" + value.QuestionId + "'>" + (index + 1) + "</li>";
        
        $('#paginator-list').append(paginatorHtml);
    });
    queHTML += "</div>";
    $('#dvQuestions').append(queHTML);

    // click handler for paginator
    $('#paginator-list > li').on('click', function () {
        $('#paginator-list > li').removeClass('active-page');
        $(this).addClass('active-page');
        const getCurrQueId = $(this).attr('data-queid');
        $('.que-container').hide();
        $('#' + getCurrQueId).show();

        var queId = getCurrQueId.substring(5, getCurrQueId.length)

        $.each(quizQueIds, function (indexQue, valueQue) {
            if (valueQue["QuestionId"] == queId) {
                currentIndex = indexQue;
                //alert(quizQueIds.length);
                //alert(currentIndex);
                if (quizQueIds.length == (currentIndex + 1)) {
                    $('#btnNext').hide();
                    $('#btnPrev').show();
                    $('#btnResponseSubmit').show();
                }
                else {
                    if (currentIndex != 0) {
                        $('#btnPrev').show();
                        $('#btnNext').show();
                        $('#btnResponseSubmit').hide();
                    }
                    else {
                        $('#btnPrev').hide();
                        $('#btnNext').show();
                        $('#btnResponseSubmit').hide();
                    }
                }
            }
        });
    });

    $.each(quizQueIds, function (indexQue, valueQue) {
        $('#queFeedback' + valueQue.QuestionId).summernote();
        if (indexQue == 0) {
            $('#dvQue' + valueQue.QuestionId).show();
        }
        else {
            $('#dvQue' + valueQue.QuestionId).hide();
        }
    });
    NextPrevQuestion();

    $('#btnPrev').on("click", function () {
        if (currentIndex > 0) {
            currentIndex--;
            NextPrevQuestion();
            if (currentIndex == 0) {
                $('#btnPrev').hide();
            }
            if (currentIndex != (quizQueIds.length - 1)) {
                //$('#btnPrev').hide();
                $('#btnNext').show();
                $('#btnResponseSubmit').hide();
            }
        }
    });

    $('#btnNext').on("click", function () {    
        console.log(111)
        if (currentIndex != (quizQueIds.length - 1)) {
            currentIndex++;
            NextPrevQuestion();
            if (currentIndex > 0) {
                $('#btnPrev').show();
            }
        }
        if (currentIndex == (quizQueIds.length - 1)) {
            $('#btnNext').hide();
            $('#btnResponseSubmit').show();
        }

    });

    $('#btnCancel').on("click", function () {
        location.reload();
    });

    if (QuizViewData.Duration > 0) {
        min = QuizViewData.Duration;
        examTimer();
    }
    if (QuizViewData.length == 1) {
        $('#btnNext').click();
    }
}

function SortedData() {
    var questionObjSorted = [];
    var table = $('#tblQuestions').DataTable();
    var questions = table.rows().data().toArray();
    $.each(questions, function (index, value) {
        var questionItem = questionObj.filter(function (item) {
            return item.QuestionId == value.QuestionId;
        })
        questionObjSorted.push(questionItem[0]);
    });
    questionObj = questionObjSorted;
    return questionObjSorted;
}

function NextPrevQuestion() {
    
    console.log(currentIndex)
    $.each(quizQueIds, function (indexQue, valueQue) {
        if (indexQue == currentIndex) {
            $('#dvQue' + valueQue.QuestionId).show();
            const id = valueQue.QuestionId;
            const p = $('[data-queid=dvQue' + id + ']');
            $(p).addClass('active-page');
        }
        else {
            $('#dvQue' + valueQue.QuestionId).hide();
            const id = valueQue.QuestionId;
            const p = $('[data-queid=dvQue' + id + ']');
            $(p).removeClass('active-page');
        }
        //$('#paginator-list > li').removeClass('active-paginator');
    });

}