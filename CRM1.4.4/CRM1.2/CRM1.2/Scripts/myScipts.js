﻿var Users = []

function LoadUser(element) {
    if (Users.length == 0) {
        $.ajax({
            type: "GET",
            url: '/request/getUser',
            data: { 'UserID': $(element).val() },
            success: function (data) {
                Users = data;
                renderUser(element);
            },
            error: function (error) {
                console.log(error);
            }
        })
    }
    //else {
    //    //render catagory to the element
    //    renderUser(element);
    //}
}

function renderUser(element) {
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(Users, function (i, val) {
        $ele.append($('<option/>').val(val.UserID).text(val.UserName));
    })
}

var Status = []

function LoadStatus(element) {
    if (Status.length == 0) {
        $.ajax({
            type: "GET",
            url: '/request/getStatus',
            data: { 'StatusID': $(element).val() },
            success: function (data) {
                Status = data;
                renderStatus(element);
            },
            error: function (error) {
                console.log(error);
            }
        })
    }
    //else {
    //    //render catagory to the element
    //    renderStatus(element);
    //}
}

function renderStatus(element) {
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(Status, function (i, val) {
        $ele.append($('<option/>').val(val.StatusID).text(val.StatusName));
    })
}

var Client = []

function LoadClient(element) {
    if (Client.length == 0) {
        $.ajax({
            type: "GET",
            url: '/request/getClient',
            data: { 'ClientID': $(element).val() },
            success: function (data) {
                Client = data;
                renderClient(element);
            },
            error: function (error) {
                console.log(error);
            }
        })
    }
    //else {
    //    //render catagory to the element
    //    renderStatus(element);
    //}
}

function renderClient(element) {
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(Client, function (i, val) {
        $ele.append($('<option/>').val(val.ClientID).text(val.CompanyName));
    })
}

var Type = []

function LoadType(element) {
    if (Type.length == 0) {
        $.ajax({
            type: "GET",
            url: '/request/getType',
            data: { 'TypeID': $(element).val() },
            success: function (data) {
                Type = data;
                renderType(element);
            },
            error: function (error) {
                console.log(error);
            }
        })
    }
    //else {
    //    //render catagory to the element
    //    renderStatus(element);
    //}
}

function renderType(element) {
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(Type, function (i, val) {
        $ele.append($('<option/>').val(val.TypeID).text(val.TypeName));
    })
}

$(document).ready(function () {
    $('#add').click(function () {
        var isAllValid = true;
        if ($('#user').val() == "0") {
            isAllValid = false;
            $('#user').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#user').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#status').val() == "0") {
            isAllValid = false;
            $('#status').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#status').siblings('span.error').css('visibility', 'hidden');
        }

        if (!($('#stageNumber').val().trim() != '' && (parseInt($('#stageNumber').val()) || 0))) {
            isAllValid = false;
            $('#stageNumber').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#stageNumber').siblings('span.error').css('visibility', 'hidden');
        }

        if (!($('#stageTime').val().trim() != '' && !isNaN($('#stageTime').val().trim()))) {
            isAllValid = false;
            $('#stageTime').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#stageTime').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#stageDate').val().trim() == '') {
            $('#stageDate').siblings('span.error').css('visibility', 'visible');
            isAllValid = false;
        }
        else {
            $('#stageDate').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#stageDesc').val().trim() == '') {
            $('#stageDesc').siblings('span.error').css('visibility', 'visible');
            isAllValid = false;
        }
        else {
            $('#stageDesc').siblings('span.error').css('visibility', 'hidden');
        }
        
        if (isAllValid) {
            var $newRow = $('#mainrow').clone().removeAttr('id');
            $('.user', $newRow).val($('#user').val());
            $('.status', $newRow).val($('#status').val());
            //$('.client', $newRow).val($('#client').val());
            //$('.type', $newRow).val($('#type').val());

            //Replace add button with remove button
            $('#add', $newRow).addClass('remove').val('Remove').removeClass('btn-success').addClass('btn-danger');

            //remove id attribute from new clone row
            $('#user,#status,#stageNumber,#stageTime, #stageDate,#stageDesc,#add', $newRow).removeAttr('id');
            $('span.error', $newRow).remove();
            //append clone row
            $('#requestdetailsItems').append($newRow);

            //clear select data
            $('#user,#status').val('0');
            $('#stageNumber,#stageTime, #stageDate, #stageDesc').val('');
            $('#requestItemError').empty();
        }

    })

    //remove button click event
    $('#requestdetailsItems').on('click', '.remove', function () {
        $(this).parents('tr').remove();
    });

    $('#submit').click(function () {
        var isAllValid = true;


        //validate
        $('#requestItemError').text('');
        var list = [];
        var errorItemCount = 0;
        $('#requestdetailsItems tbody tr').each(function (index, ele) {
            if (
                $('select.status', this).val() == "0" ||
                $('select.user', this).val() == "0" ||
                //$('select.client', this).val() == "0" ||
                //$('select.type', this).val() == "0" ||
                (parseInt($('.stageNumber', this).val()) || 0) == 0 ||
                $('.stageTime', this).val() == "" ||
                isNaN($('.stageTime', this).val()) ||
                (($('.stageDate', this).val()) || 0) == 0 ||
                (String($('.stageDesc', this).val()) || 0) == 0
                ) {
                errorItemCount++;
                $(this).addClass('error');
            } else {
                var requestItem = {

                    StatusID: $('select.status', this).val(),
                    UserID: $('select.user', this).val(),
                    StageNumber: parseInt($('.stageNumber', this).val()),
                    StageTime: parseFloat($('.stageTime', this).val()),
                    StageDate: ($('.stageDate', this).val()),
                    StageDesc: String($('.stageDesc', this).val()),
                }
                list.push(requestItem);
            }
        })


        //---------------------------------------------------------


        if (errorItemCount > 0) {
            $('#requestItemError').text(errorItemCount + " Nieprawidlowy wpis.");
            isAllValid = false;
        }

        if (list.length == 0) {
            $('#requestItemError').text('Przynajmniej 1 etap jest wymagany.');
            isAllValid = false;
        }

        if ($('#title').val().trim() == '') {
            $('#title').siblings('span.error').css('visibility', 'visible');
            isAllValid = false;
        }
        else {
            $('#title').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#requestDate').val().trim() == '') {
            $('#requestDate').siblings('span.error').css('visibility', 'visible');
            isAllValid = false;
        }
        else {
            $('#requestDate').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#client').val() == "0") {
            isAllValid = false;
            $('#client').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#client').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#type').val() == "0") {
            isAllValid = false;
            $('#type').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#type').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#description').val().trim() == '') {
            $('#description').siblings('span.error').css('visibility', 'visible');
            isAllValid = false;
        }
        else {
            $('#description').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#solution').val().trim() == '') {
            $('#solution').siblings('span.error').css('visibility', 'visible');
            isAllValid = false;
        }
        else {
            $('#solution').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#requestTime').val().trim() == '') {
            $('#requestTime').siblings('span.error').css('visibility', 'visible');
            isAllValid = false;
        }
        else {
            $('#requestTime').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#userr').val() == "0") {
            isAllValid = false;
            $('#userr').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#userr').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#statuss').val() == "0") {
            isAllValid = false;
            $('#statuss').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#statuss').siblings('span.error').css('visibility', 'hidden');
        }

        if (isAllValid) {
            var data = {
                //RequestDetailsID: $('requestDetailsID').val().trim(),
                Title: $('#title').val().trim(),
                RequestDateString: $('#requestDate').val().trim(),
                Description: $('#description').val().trim(),
                Solution: $('#solution').val().trim(),
                RequestTime: $('#requestTime').val().trim(),
                ClientID: $('#client').val(),
                TypeID: $('#type').val(),
                StatusID: $('#statuss').val(),
                UserID: $('#userr').val(),

                RequestDetails: list
            }

            $(this).val('Prosze czekac...');

            $.ajax({
                type: 'POST',
                url: '/request/save',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        alert('Zapisane pomyslnie');
                        //clear the form
                        list = [];
                        $('#title,#requestDate,#description,#client,#type,#solution,#requestTime,#statuss,#userr').val('');
                        $('#requestdetailsItems').empty();
                    }
                    else {
                        alert('Blad');
                    }
                    $('#submit').text('Save');
                },
                error: function (error) {
                    console.log(error);
                    $('#submit').text('Save');
                }
            });
        }

    });

});

LoadUser($('#user'));
LoadStatus($('#status'));
LoadClient($('#client'));
LoadType($('#type'));
LoadUser($('#userr'));
LoadStatus($('#statuss'));

// -----------------------------------------------

//if (isAllValid) {
//    var $newRow = $('#secrow').removeAttr('id');
//    $('.client', $newRow).val($('#client').val());
//    $('.type', $newRow).val($('#type').val());

//    //remove id attribute from new clone row
//    //$('#submit, #client, #type', $newRow).removeAttr('id');
//    //$('span.error', $newRow).remove();
//    //append clone row
//    //$('#requestdetailsItems').append($newRow);

//    //clear select data
//    //$('#client, #type').val('0');
//    //$('#requestItemError').empty();
//}